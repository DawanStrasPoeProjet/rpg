using RPG.Core;

namespace RPG.Stage;

public class StageSystem : IStageSystem
{
    public IGame Game { get; set; } = null!;
}
