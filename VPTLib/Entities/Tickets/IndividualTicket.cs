namespace VPTLib;

public class IndividualTicket : Ticket
{
    private readonly Visitor _visitor;
    
    public IndividualTicket(DateTime orderDate, Visitor visitor) : base(orderDate)
    {
        _visitor = visitor;
    }
    
    public Visitor Visitor => _visitor;
}