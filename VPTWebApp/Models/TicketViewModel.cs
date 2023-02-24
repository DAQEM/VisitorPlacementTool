namespace VPTWebApp.Models;

public class TicketViewModel
{
    public List<TicketModel> IndividualTickets { get; set; } = new ();
    public List<TicketModel> GroupTickets { get; set; } = new ();
    public List<SectionModel> Sections { get; set; } = new ();
}