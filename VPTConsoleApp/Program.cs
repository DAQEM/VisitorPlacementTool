using VPTLib;

Organization organization = new();

List<Ticket> tickets = TicketGenerator.GenerateGroupTickets(
    new [,] {{3, 3}, {1, 5}, {2, 1}, {1, 1}, {1, 1}, {2, 2}, {0, 4}, {0, 4}, {0, 4}, {0, 4}, {0, 4}, {0, 4}}
    ).Cast<Ticket>().ToList();

tickets.AddRange(TicketGenerator.GenerateIndividualTickets(10).Cast<Ticket>().ToList());

List<Section> generateSections = SectionGenerator.GenerateSections(
    new [,] {{3, 4}, {3, 3}, {3, 9}, {3, 8}});

Show show = new(
    tickets,
    generateSections);

new VisitorPlacer(show).PlaceVisitors();

List<Group> groups = show.Tickets.Where(t => t is GroupTicket).Cast<GroupTicket>().Select(gt => gt.Group).ToList();
List<Visitor> individualVisitors = show.Tickets.Where(t => t is IndividualTicket).Cast<IndividualTicket>().Select(it => it.Visitor).ToList();

show.Sections.ToList().ForEach(section =>
{
    section.Seats.OrderBy(seat => seat.Row).ThenBy(seat => seat.Column).ToList().ForEach(seat =>
    {
        Visitor? visitor = seat.Visitor;
        int groupIndex = groups.FindIndex(g => g.Visitors.Contains(visitor));
        int individualIndex = individualVisitors.FindIndex(v => v == visitor);
        Console.ForegroundColor = groupIndex == -1 ? individualIndex == -1 ? ConsoleColor.Gray : ConsoleColor.Black : (ConsoleColor) (groupIndex + 1);
        Console.Write(seat.Visitor == null ? "O" : seat.Visitor.IsKid ? "x" : "X");
        if (seat.Column == section.Columns - 1)
        {
            Console.WriteLine();
        }
    });
    Console.WriteLine();
});