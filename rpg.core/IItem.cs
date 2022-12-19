namespace RPG.Core;

public interface IItem
{
    int Key { get; }
    string Id { get; }
    IEnumerable<string> Tags { get; }

    bool HasTags(params string[] tags);
    bool HasTags(IEnumerable<string> tags);
}
