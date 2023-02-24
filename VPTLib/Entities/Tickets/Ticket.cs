namespace VPTLib;

public abstract class Ticket
{
    private readonly DateTime _orderDate;

    protected Ticket(DateTime orderDate)
    {
        _orderDate = orderDate;
    }
    
    public DateTime OrderDate => _orderDate;
}