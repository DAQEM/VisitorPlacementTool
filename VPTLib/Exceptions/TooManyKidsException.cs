namespace VPTLib.Exceptions;

public class TooManyKidsException : Exception
{
    protected TooManyKidsException(string str) : base($"{str} contains too many kids.")
    {
    }
    
    public override string ToString()
    {
        return Message;
    }
}