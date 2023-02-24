namespace VPTLib.Exceptions;

public class GroupTooManyKidsException : TooManyKidsException
{
    public GroupTooManyKidsException() : base("Group")
    {
    }
}