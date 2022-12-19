#nullable disable

namespace RPG.Data;

internal class ItemDto
{
    public string Id { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
}

internal class WeaponItemDto : ItemDto
{
    public int BaseDamage { get; set; }
    public int NumDiceRolls { get; set; }
    public int NumDiceFaces { get; set; }
}

internal class PotionItemDto : ItemDto
{
    public int Health { get; set; }
    public int Magic { get; set; }
}

internal class QuestItemDto : ItemDto
{
}
