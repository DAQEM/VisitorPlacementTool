namespace VPTLib.Exceptions;

public class OutOfFirstRowSeatsException : Exception
{
    public OutOfFirstRowSeatsException() : base("There are not enough seats on the first row to place visitor.")
    {
    }
}