#nullable disable

namespace RPG.Data;

internal class EntityDto
{
    public string Id { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public IEnumerable<string> BagItemIds { get; set; }
    public string DefaultEquippedItemId { get; set; }
    public string EquippedItemId { get; set; }
    public string Name { get; set; }
    public int Initiative { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Magic { get; set; }
    public int MaxMagic { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Money { get; set; }
    public bool IsPlayer { get; set; }
}
