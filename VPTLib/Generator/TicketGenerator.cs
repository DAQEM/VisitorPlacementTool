namespace VPTLib;

public static class TicketGenerator
{
    private static int _orderDate;
    
    public static List<IndividualTicket> GenerateIndividualTickets(int count)
    {
        List<IndividualTicket> tickets = new();
        for (int i = 0; i < count; i++)
        {
            tickets.Add(GenerateIndividualTicket());
        }
        return tickets;
    }
    
    public static List<GroupTicket> GenerateGroupTickets(int[,] kidsAndAdults)
    {
        List<GroupTicket> tickets = new();
        for (int i = 0; i < kidsAndAdults.GetLength(0); i++)
        {
            tickets.Add(GenerateGroupTicket(kidsAndAdults[i, 0], kidsAndAdults[i, 1]));
        }
        return tickets;
    }
    
    public static IndividualTicket GenerateIndividualTicket()
    {
        return GenerateIndividualTicket(DateTime.Now.AddDays(++_orderDate));
    }
    
    public static IndividualTicket GenerateIndividualTicket(DateTime orderDate)
    {
        return new IndividualTicket(orderDate, new Visitor(DateTime.Now.AddYears(-34)));
    }
    
    public static GroupTicket GenerateGroupTicket(int kids, int adults)
    {
        return GenerateGroupTicket(DateTime.Now.AddDays(++_orderDate), kids, adults);
    }
    
    public static GroupTicket GenerateGroupTicket(DateTime orderDate, int kids, int adults)
    {
        List<Visitor> visitors = new();
        for (int i = 0; i < kids; i++)
        {
            visitors.Add(new Visitor(DateTime.Now.AddYears(-10)));
        }
        for (int i = 0; i < adults; i++)
        {
            visitors.Add(new Visitor(DateTime.Now.AddYears(-34)));
        }
        return new GroupTicket(orderDate, new Group(visitors));
    }
}
