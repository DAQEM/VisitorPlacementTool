using VPTLib;

namespace VPTTests;

public class ShowTests
{
    [Test]
    public void ShowConstructorTest()
    {
        // Arrange
        List<Ticket> tickets = TicketGenerator.GenerateIndividualTickets(5).Cast<Ticket>().ToList();
        Show show = new(tickets);

        // Assert
        Assert.That(show.Tickets, Has.Exactly(5).Items);
    }

    [Test]
    public void AddTicketTest()
    {
        // Arrange
        Show show = new();
        IndividualTicket ticket = TicketGenerator.GenerateIndividualTicket();

        // Act
        show.AddTicket(ticket);

        // Assert
        Assert.That(show.Tickets, Has.Exactly(1).Items);
    }
    
    [Test]
    public void AddTicketsTest()
    {
        // Arrange
        Show show = new();
        List<Ticket> tickets = TicketGenerator.GenerateIndividualTickets(5).Cast<Ticket>().ToList();

        // Act
        show.AddTickets(tickets);

        // Assert
        Assert.That(show.Tickets, Has.Exactly(5).Items);
    }
}