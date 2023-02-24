using VPTLib;

namespace VPTWebApp.Models;

public class ShowViewModel
{
    public Show Show { get; }
    
    public ShowViewModel(TicketViewModel model)
    {
        List<Ticket> tickets = model.IndividualTickets.Select(modelIndividualTicket => TicketGenerator.GenerateIndividualTicket(modelIndividualTicket.OrderDate)).Cast<Ticket>().ToList();
        tickets.AddRange(model.GroupTickets.Select(modelGroupTicket => TicketGenerator.GenerateGroupTicket(modelGroupTicket.OrderDate, modelGroupTicket.Kids ?? 0, modelGroupTicket.Adults ?? 0)).Cast<Ticket>().ToList());
        
        List<Section> sections = model.Sections.Select(modelSection => SectionGenerator.GenerateSection(modelSection.Rows, modelSection.Columns)).ToList();

        Show = new VisitorPlacer(new Show(tickets, sections)).PlaceVisitors().Show;
    }
}