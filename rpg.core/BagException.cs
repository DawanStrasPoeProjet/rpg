namespace RPG.Core;

public class BagException : Exception
{
    public BagException()
    {
    }

    public BagException(string message)
        : base(message)
    {
    }

    public BagException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
