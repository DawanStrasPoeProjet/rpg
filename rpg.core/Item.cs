namespace RPG.Core;

public class Item : IItem
{
    public int Key { get; }
    public string Id { get; }
    private readonly List<string> _tags = new();
    public IEnumerable<string> Tags => _tags;

    public Item(int key, string id, IEnumerable<string>? tags)
    {
        Key = key;
        Id = id;
        if (tags != null) _tags.AddRange(tags);
    }

    public override string ToString()
        => $"Item({nameof(Key)}={Key}, {nameof(Id)}={Id}, {nameof(Tags)}={{{string.Join(", ", Tags)}}})";

    protected void InsertFrontTag(string tag)
        => _tags.Insert(0, tag);

    public static IEnumerable<string> TagsOf(params string[] tags)
        => tags;

    public bool HasTags(params string[] tags)
        => tags.All(Tags.Contains);

    public bool HasTags(IEnumerable<string> tags)
        => tags.All(Tags.Contains);
}
