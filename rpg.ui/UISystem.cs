using RPG.Core;

namespace RPG.UI;

public class UISystem : IUISystem
{
    public IGame Game { get; set; } = null!;
}
