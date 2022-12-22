#nullable disable

namespace RPG.Data.Db.Models;

public class Item
{
    public string Id { get; set; }
    public string Tags { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
}
