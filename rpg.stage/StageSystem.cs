using System.Text.RegularExpressions;
using RPG.Core;
using RPG.Stage.UI;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using IUISystem = RPG.Stage.UI.IUISystem;

namespace RPG.Stage;

public class StageSystem : IStageSystem
{
    public IGame Game { get; set; } = null!;

    private readonly string _bootPath;
    private readonly string _bootScene;
    private readonly string _stageDir;
    private readonly string _heroName;
    private IEntity _hero = null!;
    private StageDto _stage = null!;
    private readonly Dictionary<string, IEntity> _actors = new();
    private readonly Dictionary<string, SceneDto> _scenes = new();
    private readonly IUISystem _uiSystem = new UISystem();

    public StageSystem(string bootPath, string bootScene, string heroName)
    {
        _stageDir = Path.GetDirectoryName(bootPath)!;
        _bootPath = bootPath;
        _bootScene = bootScene;
        _heroName = heroName;
    }

    public void Boot()
    {
        LoadStage(_bootPath, _bootScene);

        _hero = Game.EntitySource.Create("hero");
        _hero.Rename(_heroName);
        Game.SetData("HeroName", _heroName);
        _uiSystem.SetHero(_hero);

        Start();
    }

    private void LoadStage(string path, string scene)
    {
        using var reader = new StreamReader(path);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping("!NextStage", typeof(NextStageActionDto))
            .WithTagMapping("!NextScene", typeof(NextSceneActionDto))
            .WithTagMapping("!Hang", typeof(HangActionDto))
            .WithTagMapping("!Monologue", typeof(MonologueActionDto))
            .WithTagMapping("!Choices", typeof(ChoicesActionDto))
            .WithTagMapping("!CheckFlag", typeof(CheckFlagActionDto))
            .WithTagMapping("!SetFlag", typeof(SetFlagActionDto))
            .WithTagMapping("!UnsetFlag", typeof(UnsetFlagActionDto))
            .WithTagMapping("!SetData", typeof(SetDataActionDto))
            .WithTagMapping("!GiveHealth", typeof(GiveHealthActionDto))
            .WithTagMapping("!CheckHasHealth", typeof(CheckHasHealthActionDto))
            .WithTagMapping("!CheckHasntHealth", typeof(CheckHasntHealthActionDto))
            .WithTagMapping("!TakeHealth", typeof(TakeHealthActionDto))
            .WithTagMapping("!GiveMoney", typeof(GiveMoneyActionDto))
            .WithTagMapping("!CheckHasMoney", typeof(CheckHasMoneyActionDto))
            .WithTagMapping("!TakeMoney", typeof(TakeMoneyActionDto))
            .WithTagMapping("!GiveItem", typeof(GiveItemActionDto))
            .WithTagMapping("!CheckHasItem", typeof(CheckHasItemActionDto))
            .WithTagMapping("!EquipItem", typeof(EquipItemActionDto))
            .WithTagMapping("!CheckHasEquip", typeof(CheckHasEquipActionDto))
            .WithTagMapping("!CheckRandom", typeof(CheckRandomActionDto))
            .WithTagMapping("!Quit", typeof(QuitActionDto))
            .WithTagMapping("!Combat", typeof(CombatActionDto))
            .WithTagMapping("!IsFlagSet", typeof(IsFlagSetCondDto))
            .WithTagMapping("!IsFlagUnset", typeof(IsFlagUnsetCondDto))
            .WithTagMapping("!HasEquip", typeof(HasEquipCondDto))
            .WithTagMapping("!HasItem", typeof(HasItemCondDto))
            .Build();
        _stage = deserializer.Deserialize<StageDto>(reader);

        foreach (var i in _stage.Actors)
            _actors[i] = Game.EntitySource.Create(i);
        foreach (var i in _stage.Scenes)
            _scenes[i.Id] = i;
    }

    private string InterpolateText(string text)
    {
        var matches = Regex.Matches(text, "\\$[a-zA-Z0-9]*");
        foreach (var i in matches)
        {
            var match = i.ToString()!;
            var data = match[1..];
            if (Game.HasData(data))
                text = text.Replace(match, Game.GetData(data).ToString());
        }
        return text;
    }

    private void UpdateStageSceneUI(SceneDto scene)
    {
        _uiSystem.SetStageName(InterpolateText(_stage.Name));
        _uiSystem.SetStageDescription(InterpolateText(_stage.Description));
        _uiSystem.SetSceneName(InterpolateText(scene.Name));
        _uiSystem.SetSceneName(InterpolateText(scene.Description));
        _uiSystem.Invalidate();
    }

    private void Start()
    {
        var scene = _scenes[_bootScene];
        var nextAction = scene.Do;
        UpdateStageSceneUI(scene);

        while (true)
        {
            switch (nextAction)
            {
                case NextStageActionDto dto:
                {
                    scene = Scene_DoNextStageAction(dto);
                    nextAction = scene.Do;
                    UpdateStageSceneUI(scene);
                    break;
                }
                case NextSceneActionDto dto:
                {
                    scene = Scene_DoNextSceneAction(dto);
                    nextAction = scene.Do;
                    UpdateStageSceneUI(scene);
                    break;
                }
                case HangActionDto dto:
                    Scene_DoHangAction(dto);
                    break;
                case MonologueActionDto dto:
                    nextAction = Scene_DoMonologueAction(scene, dto);
                    break;
                case ChoicesActionDto dto:
                    nextAction = Scene_DoChoicesAction(dto);
                    break;
                case CheckFlagActionDto dto:
                    nextAction = Scene_DoCheckFlagActionDto(dto);
                    break;
                case SetFlagActionDto dto:
                    nextAction = Scene_DoSetFlagActionDto(dto);
                    break;
                case UnsetFlagActionDto dto:
                    nextAction = Scene_DoUnsetFlagActionDto(dto);
                    break;
                case SetDataActionDto dto:
                    nextAction = Scene_DoSetDataActionDto(dto);
                    break;
                case GiveHealthActionDto dto:
                    nextAction = Scene_DoGiveHealthActionDto(dto);
                    break;
                case CheckHasHealthActionDto dto:
                    nextAction = Scene_DoCheckHasHealthActionDto(dto);
                    break;
                case CheckHasntHealthActionDto dto:
                    nextAction = Scene_DoCheckHasntHealthActionDto(dto);
                    break;
                case TakeHealthActionDto dto:
                    nextAction = Scene_DoTakeHealthActionDto(dto);
                    break;
                case GiveMoneyActionDto dto:
                    nextAction = Scene_DoGiveMoneyActionDto(dto);
                    break;
                case CheckHasMoneyActionDto dto:
                    nextAction = Scene_DoCheckHasMoneyActionDto(dto);
                    break;
                case TakeMoneyActionDto dto:
                    nextAction = Scene_DoTakeMoneyActionDto(dto);
                    break;
                case GiveItemActionDto dto:
                    nextAction = Scene_DoGiveItemActionDto(dto);
                    break;
                case CheckHasItemActionDto dto:
                    nextAction = Scene_DoCheckHasItemActionDto(dto);
                    break;
                case EquipItemActionDto dto:
                    nextAction = Scene_DoEquipItemActionDto(dto);
                    break;
                case CheckHasEquipActionDto dto:
                    nextAction = Scene_DoCheckHasEquipActionDto(dto);
                    break;
                case CheckRandomActionDto dto:
                    nextAction = Scene_DoCheckRandomActionDto(dto);
                    break;
                case QuitActionDto:
                    _uiSystem.Clear();
                    return;
                case CombatActionDto dto:
                    nextAction = Scene_DoCombatActionDto(dto);
                    break;
                default:
                    Console.WriteLine($"unhandled action {nextAction}");
                    break;
            }
        }
    }

    private bool EvaluateCond(CondDto cond)
    {
        return cond switch
        {
            IsFlagSetCondDto dto => Game.HasFlag(dto.Flag) && Game.GetFlag(dto.Flag),
            IsFlagUnsetCondDto dto => !Game.HasFlag(dto.Flag) || Game.HasFlag(dto.Flag) && !Game.GetFlag(dto.Flag),
            HasEquipCondDto dto => _hero.EquippedItem.Id == dto.Item,
            HasItemCondDto dto => _hero.Bag.HasItemById(dto.Item),
            _ => false
        };
    }

    private SceneDto Scene_DoNextStageAction(NextStageActionDto dto)
    {
        foreach (var i in _stage.ClearFlags)
            Game.SetFlag(i, false);
        foreach (var i in _stage.ClearData)
            Game.SetData(i, null!);
        _actors.Clear();
        _scenes.Clear();

        var sceneName = InterpolateText(dto.Scene ?? "start");
        LoadStage(Path.Join(_stageDir, InterpolateText(dto.Stage) + ".yaml"), sceneName);
        return _scenes[sceneName];
    }

    private SceneDto Scene_DoNextSceneAction(NextSceneActionDto dto)
        => _scenes[dto.Scene];

    private void Scene_DoHangAction(HangActionDto dto)
    {
        Console.WriteLine(dto.Dummy);
        while (true) ;
    }

    private ActionDto Scene_DoMonologueAction(SceneDto scene, MonologueActionDto dto)
    {
        var from = _actors[dto.From];
        foreach (var i in dto.Texts)
            _uiSystem.Say(from, InterpolateText(i));
        return dto.Then;
    }

    private ActionDto Scene_DoChoicesAction(ChoicesActionDto dto)
    {
        var choiceTexts = (from i in dto.Choices
            where i.If is null || (i.If != null) && EvaluateCond(i.If)
            select InterpolateText(i.Text)).ToList();
        var choice = _uiSystem.Choices(dto.Title != null ? InterpolateText(dto.Title) : null, choiceTexts);
        return dto.Choices.First(x => InterpolateText(x.Text) == choice).Do;
    }

    private ActionDto Scene_DoCheckFlagActionDto(CheckFlagActionDto dto)
    {
        var next = Game.HasFlag(dto.Flag) && Game.GetFlag(dto.Flag) ? dto.Yes : dto.No;
        if (dto.Set != null) Game.SetFlag(dto.Flag, dto.Set.Value);
        return next;
    }

    private ActionDto Scene_DoSetFlagActionDto(SetFlagActionDto dto)
    {
        Game.SetFlag(dto.Flag, true);
        return dto.Then;
    }

    private ActionDto Scene_DoUnsetFlagActionDto(UnsetFlagActionDto dto)
    {
        Game.SetFlag(dto.Flag, false);
        return dto.Then;
    }

    private ActionDto Scene_DoSetDataActionDto(SetDataActionDto dto)
    {
        Game.SetData(dto.Data, dto.Value!);
        return dto.Then;
    }

    private ActionDto Scene_DoGiveHealthActionDto(GiveHealthActionDto dto)
    {
        _hero.Health = Math.Clamp(_hero.Health + dto.Health, 0, _hero.MaxHealth);
        return dto.Then;
    }

    private ActionDto Scene_DoCheckHasHealthActionDto(CheckHasHealthActionDto dto)
        => _hero.Health >= dto.Health ? dto.Yes : dto.No;

    private ActionDto Scene_DoCheckHasntHealthActionDto(CheckHasntHealthActionDto dto)
        => dto.Health <= _hero.MaxHealth - _hero.Health ? dto.Yes : dto.No;

    private ActionDto Scene_DoTakeHealthActionDto(TakeHealthActionDto dto)
    {
        _hero.Health -= dto.Health;
        if (_hero.Health < 0) _hero.Health = 0;
        return dto.Then;
    }

    private ActionDto Scene_DoGiveMoneyActionDto(GiveMoneyActionDto dto)
    {
        _hero.Money += dto.Money;
        return dto.Then;
    }

    private ActionDto Scene_DoCheckHasMoneyActionDto(CheckHasMoneyActionDto dto)
        => _hero.Money >= dto.Money ? dto.Yes : dto.No;

    private ActionDto Scene_DoTakeMoneyActionDto(TakeMoneyActionDto dto)
    {
        _hero.Money -= dto.Money;
        if (_hero.Money < 0) _hero.Money = 0;
        return dto.Then;
    }

    private ActionDto Scene_DoGiveItemActionDto(GiveItemActionDto dto)
    {
        _hero.Bag.AddItemById(dto.Item);
        return dto.Then;
    }

    private ActionDto Scene_DoCheckHasItemActionDto(CheckHasItemActionDto dto)
        => _hero.Bag.HasItemById(dto.Item) ? dto.Yes : dto.No;

    private ActionDto Scene_DoEquipItemActionDto(EquipItemActionDto dto)
    {
        _hero.EquipNewItemById(dto.Item);
        return dto.Then;
    }

    private ActionDto Scene_DoCheckHasEquipActionDto(CheckHasEquipActionDto dto)
        => _hero.EquippedItem.Id == dto.Item ? dto.Yes : dto.No;

    private ActionDto Scene_DoCheckRandomActionDto(CheckRandomActionDto dto)
        => Random.Shared.NextDouble() <= dto.Chance ? dto.Yes : dto.No;

    private ActionDto Scene_DoCombatActionDto(CombatActionDto dto)
    {
        _uiSystem.Clear();
        var entities = dto.Entities.Select(i => Game.EntitySource.Create(i)).ToList();
        return Game.CombatSystem.BeginCombat(_hero, entities) switch
        {
            CombatResult.Won => dto.Won,
            CombatResult.Lost => dto.Lost,
            CombatResult.Fled => dto.Fled,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
