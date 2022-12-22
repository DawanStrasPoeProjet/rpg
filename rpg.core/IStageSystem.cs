namespace RPG.Core;

public interface IStageSystem
{
    IGame Game { get; set; }

    void Boot();
}
