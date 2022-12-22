using RPG.Core;

namespace RPG.Stage.UI;

public interface IUISystem
{
    void Clear();
    void Invalidate();
    void SetHero(IEntity hero);
    void SetStageName(string name);
    void SetStageDescription(string description);
    void SetSceneName(string name);
    void SetSceneDescription(string description);
    void Say(IEntity from, string text);
    string Choices(string? title, IEnumerable<string> choices);
}
