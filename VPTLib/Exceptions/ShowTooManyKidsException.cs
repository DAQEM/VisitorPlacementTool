namespace VPTLib.Exceptions;

public class ShowTooManyKidsException : TooManyKidsException
{
    public ShowTooManyKidsException() : base("Show")
    {
    }
}