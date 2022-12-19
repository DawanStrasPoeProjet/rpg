namespace RPG.Core;

public interface IItem
{
    int Key { get; }
    string Id { get; }
    IEnumerable<string> Tags { get; }
    int Price { get; set; }

    bool HasTags(params string[] tags);
    bool HasTags(IEnumerable<string> tags);
}
