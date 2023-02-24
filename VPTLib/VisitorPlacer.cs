using VPTLib.Exceptions;

namespace VPTLib;

public class VisitorPlacer
{
    private readonly Show _show;
    
    public VisitorPlacer(Show show)
    {
        _show = show;
    }
    
    public Show Show => _show;
    
    public VisitorPlacer PlaceVisitors()
    {
        List<Ticket> tickets = GetAcceptedTicketsByOrderDate();

        CheckKids(tickets);
        
        List<IndividualTicket> individualTickets = tickets.Where(t => t is IndividualTicket).Cast<IndividualTicket>().ToList();
        List<GroupTicket> groupTickets = tickets.Where(t => t is GroupTicket).Cast<GroupTicket>().ToList();
        
        List<GroupTicket> groupTicketsWithKids = groupTickets.Where(gt => gt.Group.HasKids).ToList();
        List<GroupTicket> groupTicketsWithoutKids = groupTickets.Where(gt => !gt.Group.HasKids).ToList();

        PlaceGroupsWithKids(groupTicketsWithKids);
        PlaceGroupsWithoutKids(groupTicketsWithoutKids);
        PlaceIndividuals(individualTickets);
        return this;
    }

    private List<Ticket> GetAcceptedTicketsByOrderDate()
    {
        List<Ticket> ticketsByOrderDate = _show.Tickets.OrderBy(t => t.OrderDate).ToList();
        if (ShowHasEnoughSeatsForTickets()) return ticketsByOrderDate;

        List<Ticket> acceptedTickets = new();
        int amountOfVisitors = 0;
        
        using IEnumerator<Ticket> ticketEnumerator = ticketsByOrderDate.GetEnumerator();
        while (ticketEnumerator.MoveNext())
        {
            Ticket ticket = ticketEnumerator.Current;
            switch (ticket)
            {
                case IndividualTicket:
                {
                    if (amountOfVisitors + 1 <= _show.AmountOfSeats)
                    {
                        acceptedTickets.Add(ticket);
                        amountOfVisitors++;
                    }

                    break;
                }
                case GroupTicket groupTicket:
                {
                    if (amountOfVisitors + groupTicket.Group.AmountOfVisitors <= _show.AmountOfSeats)
                    {
                        acceptedTickets.Add(ticket);
                        amountOfVisitors += groupTicket.Group.AmountOfVisitors;
                    }

                    break;
                }
            }

            if (amountOfVisitors == _show.AmountOfSeats) break;
        }
        return acceptedTickets;
    }

    private bool ShowHasEnoughSeatsForTickets()
    {
        return _show.AmountOfSeats >= _show.AmountOfVisitors;
    }

    private void PlaceIndividuals(List<IndividualTicket> individualTickets)
    {
        individualTickets.ForEach(ticket =>
        {
            _show.PlaceIndividual(ticket.Visitor);
        });
    }

    private void PlaceGroupsWithoutKids(List<GroupTicket> groupTickets)
    {
        groupTickets.Select(gt => gt.Group).ToList().ForEach(group =>
        {
            _show.PlaceGroup(group);
        });
    }

    private void PlaceGroupsWithKids(List<GroupTicket> groupTickets)
    {
        bool doAllGroupsFitOnFirstRow = groupTickets.Sum(gt => gt.Group.AmountOfVisitors) <= _show.AmountOfFirstRowSeats;
        List<Group> groups = groupTickets.Select(gt => gt.Group).OrderByDescending(g => g.AmountOfVisitors).ToList();
        if (doAllGroupsFitOnFirstRow)
        {
            groups.ForEach(group =>
            {
                _show.PlaceGroupOnFirstRow(group);
            });
        }
        else
        {
            List<Group> firstRowGroups = new();
            List<Group> otherGroups = new();
            int amountOfTakenSeats = groupTickets.Sum(gt => gt.Group.AmountWithoutOtherParents);
            if (amountOfTakenSeats > _show.AmountOfFirstRowSeats)
            {
                throw new ShowTooManyKidsException();
            }
            groups.ForEach(group =>
            {
                firstRowGroups.Add(new Group(group.Visitors.Take(group.AmountWithoutOtherParents).ToList()));
                otherGroups.Add(new Group(group.Visitors.Skip(group.AmountWithoutOtherParents).ToList()));
            });
                
            firstRowGroups.ForEach(group =>
            {
                _show.PlaceGroupOnFirstRow(group);
            });
                
            otherGroups.ForEach(group =>
            {
                _show.PlaceGroup(group);
            });
        }
    }

    private void CheckKids(List<Ticket> tickets)
    {
        int totalAmountOfKids = tickets.Where(t => t is GroupTicket).Cast<GroupTicket>().Sum(gt => gt.Group.AmountOfKids);
        if (totalAmountOfKids > _show.MaxKidsAmount)
        {
            throw new ShowTooManyKidsException();
        }
    }
}