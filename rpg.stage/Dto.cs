namespace RPG.Stage;

internal class StageDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<string> Actors { get; set; } = new List<string>();
    public IEnumerable<SceneDto> Scenes { get; set; } = new List<SceneDto>();
    public IEnumerable<string> ClearFlags { get; set; } = new List<string>();
    public IEnumerable<string> ClearData { get; set; } = new List<string>();
}

internal class SceneDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ActionDto? Do { get; set; }
    public string? Next { get; set; }
}

internal class ActionDto
{
}

internal class NextStageActionDto : ActionDto
{
    public string Stage { get; set; } = null!;
    public string? Scene { get; set; }
}

internal class NextSceneActionDto : ActionDto
{
    public string Scene { get; set; } = null!;
}

internal class HangActionDto : ActionDto
{
    public string Dummy { get; set; } = string.Empty;
}

internal class MonologueActionDto : ActionDto
{
    public string From { get; set; } = null!;
    public IEnumerable<string> Texts { get; set; } = new List<string>();
    public ActionDto Then { get; set; } = null!;
}

internal class ChoicesActionDto : ActionDto
{
    public string? Title { get; set; }
    public IEnumerable<ChoiceDto> Choices { get; set; } = new List<ChoiceDto>();
}

internal class ChoiceDto
{
    public CondDto? If { get; set; }
    public string Text { get; set; } = null!;
    public ActionDto Do { get; set; } = null!;
}

internal class CheckFlagActionDto : ActionDto
{
    public string Flag { get; set; } = null!;
    public bool? Set { get; set; } = null;
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class SetFlagActionDto : ActionDto
{
    public string Flag { get; set; } = null!;
    public ActionDto Then { get; set; } = null!;
}

internal class UnsetFlagActionDto : ActionDto
{
    public string Flag { get; set; } = null!;
    public ActionDto Then { get; set; } = null!;
}

internal class SetDataActionDto : ActionDto
{
    public string Data { get; set; } = null!;
    public object? Value { get; set; }
    public ActionDto Then { get; set; } = null!;
}

internal class GiveHealthActionDto : ActionDto
{
    public int Health { get; set; }
    public ActionDto Then { get; set; } = null!;
}

internal class CheckHasHealthActionDto : ActionDto
{
    public int Health { get; set; }
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class CheckHasntHealthActionDto : ActionDto
{
    public int Health { get; set; }
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class TakeHealthActionDto : ActionDto
{
    public int Health { get; set; }
    public ActionDto Then { get; set; } = null!;
}

internal class GiveMoneyActionDto : ActionDto
{
    public int Money { get; set; }
    public ActionDto Then { get; set; } = null!;
}

internal class CheckHasMoneyActionDto : ActionDto
{
    public int Money { get; set; }
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class TakeMoneyActionDto : ActionDto
{
    public int Money { get; set; }
    public ActionDto Then { get; set; } = null!;
}

internal class GiveItemActionDto : ActionDto
{
    public string Item { get; set; } = null!;
    public ActionDto Then { get; set; } = null!;
}

internal class CheckHasItemActionDto : ActionDto
{
    public string Item { get; set; } = null!;
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class EquipItemActionDto : ActionDto
{
    public string Item { get; set; } = null!;
    public ActionDto Then { get; set; } = null!;
}

internal class CheckHasEquipActionDto : ActionDto
{
    public string Item { get; set; } = null!;
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class CheckRandomActionDto : ActionDto
{
    public double Chance { get; set; }
    public ActionDto Yes { get; set; } = null!;
    public ActionDto No { get; set; } = null!;
}

internal class QuitActionDto : ActionDto
{
    public string Dummy { get; set; } = string.Empty;
}

internal class CombatActionDto : ActionDto
{
    public IEnumerable<string> Entities { get; set; } = null!;
    public ActionDto Won { get; set; } = null!;
    public ActionDto Lost { get; set; } = null!;
    public ActionDto Fled { get; set; } = null!;
}

internal class CondDto
{
}

internal class IsFlagSetCondDto : CondDto
{
    public string Flag { get; set; } = null!;
}

internal class IsFlagUnsetCondDto : CondDto
{
    public string Flag { get; set; } = null!;
}

internal class HasEquipCondDto : CondDto
{
    public string Item { get; set; } = null!;
}

internal class HasItemCondDto : CondDto
{
    public string Item { get; set; } = null!;
}
