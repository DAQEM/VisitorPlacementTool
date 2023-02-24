namespace VPTLib;

public class GroupTicket : Ticket
{
    private readonly Group _group;
    
    public GroupTicket(DateTime orderDate, Group group) : base(orderDate)
    {
        _group = group;
    }
    
    public Group Group => _group;
}