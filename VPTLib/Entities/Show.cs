namespace VPTLib;

public class Show
{
    private readonly List<Ticket> _tickets;
    private List<Section> _sections;

    public Show() : this(new List<Ticket>())
    {
    }
    
    public Show(List<Ticket> tickets) : this(tickets, new List<Section>())
    {
    }
    
    public Show(List<Section> sections) : this(new List<Ticket>(), sections)
    {
    }
    
    public Show(List<Ticket> tickets, List<Section> sections)
    {
        _tickets = tickets;
        _sections = sections;
    }

    public IReadOnlyList<Ticket> Tickets => _tickets;
    public IReadOnlyList<Section> Sections => _sections;
    public int MaxKidsAmount => _sections.Sum(section => section.MaxKidsAmount);
    public int AmountOfFirstRowSeats => _sections.Sum(section => section.Columns);
    public int AmountOfSeats => _sections.Sum(section => section.Rows * section.Columns);
    public int AmountOfVisitors => _tickets.Where(ticket => ticket is GroupTicket).Cast<GroupTicket>().Sum(gt => gt.Group.Visitors.Count) + _tickets.Where(ticket => ticket is IndividualTicket).Cast<IndividualTicket>().Count();

    public void AddTicket(Ticket ticket)
    {
        _tickets.Add(ticket);
    }
    
    public void AddTickets(List<Ticket> tickets)
    {
        _tickets.AddRange(tickets);
    }

    public void PlaceGroupOnFirstRow(Group group)
    {
        Section? optimalSection = GetFirstRowOptimalSectionForGroup(group);
        if (optimalSection != null)
        {
            optimalSection.PlaceGroupOnFirstRow(group);
        }
        else
        {
            Dictionary<Section, int> sections = GetFirstRowSectionsForGroup(group).OrderByDescending(pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            if (sections.Count == 0)
            {
                List<Group> splitGroups = group.SplitKidsGroupUp();
                if (splitGroups.Count > 1)
                {
                    splitGroups.ForEach(PlaceGroupOnFirstRow);
                }
                else
                {
                    throw new Exception("Why is this happening?");
                }
            }
            else
            {
                KeyValuePair<Section, int> firstPair = sections.First();
                KeyValuePair<Section, int> lastPair = sections.Last();
                Section section = lastPair.Value == 0 ? lastPair.Key : firstPair.Key;
                section.PlaceGroupOnFirstRow(group);
            }
        }
    }

    public void PlaceGroup(Group group)
    {
        Section? optimalSection = GetOptimalSectionForGroup(group);
        if (optimalSection != null)
        {
            optimalSection.PlaceGroupOptimalSpot(group);
        }
        else
        {
            Section? availableSection = GetAvailableSectionForGroup(group);
            if (availableSection != null)
            {
                availableSection.PlaceGroupOnAvailableSpot(group);
            }
            else
            {
                List<Group> splitGroups = group.SplitUp();
                if (splitGroups.Count > 1)
                {
                    splitGroups.ForEach(PlaceGroup);
                }
                else
                {
                    throw new Exception("Why is this happening?");
                }
            }
        }
    }

    public void PlaceIndividual(Visitor individualVisitor)
    {
        Dictionary<Section, int> availableRows = new();
        _sections.ForEach(section =>
        {
            int rowForIndividual = section.GetAvailableRowForIndividual();
            if (rowForIndividual != -1) availableRows.Add(section, rowForIndividual);
        });
        if (!availableRows.Any()) throw new Exception("No available rows for individual visitor.");
        availableRows
            .MinBy(pair => pair.Value)
            .Key
            .PlaceIndividual(individualVisitor);
    }

    private Section? GetAvailableSectionForGroup(Group group)
    {
        Dictionary<Section, int> availableRows = new();
        _sections.ForEach(section =>
        {
            int availableRowForGroup = section.GetAvailableRowForGroup(group);
            if (availableRowForGroup != -1) availableRows.Add(section, availableRowForGroup);
        });
        if (!availableRows.Any()) return null;
        return availableRows
            .MinBy(pair => pair.Value)
            .Key;
    }

    /// <summary>
    /// Gets all sections where the group can be placed on the first row.
    /// </summary>
    /// <param name="group">The group</param>
    /// <returns>The available sections and the number of left over first row seats after the group has taken it.</returns>
    private Dictionary<Section, int> GetFirstRowSectionsForGroup(Group group)
    {
        Dictionary<Section, int> sections = new();
        foreach (Section section in _sections.Where(section => section.AvailableFirstRowSeats.Count > group.AmountOfVisitors))
        {
            int leftOverSeats = section.AvailableFirstRowSeats.Count - group.AmountOfVisitors;
            if (leftOverSeats >= 0)
            {
                sections.Add(section, leftOverSeats);
            }
        }
        return sections;
    }

    private Section? GetOptimalSectionForGroup(Group group)
    {
        Dictionary<Section, int> optimalRows = new();
        _sections.ForEach(section =>
        {
            int optimalRowForGroup = section.GetOptimalRowForGroup(group);
            if (optimalRowForGroup != -1) optimalRows.Add(section, optimalRowForGroup);
        });
        if (!optimalRows.Any()) return null;
        return optimalRows
            .MinBy(pair => pair.Value)
            .Key;
    }

    private Section? GetFirstRowOptimalSectionForGroup(Group group)
    {
        return _sections.FirstOrDefault(section => section.IsFirstRowOptimalForGroup(group));
    }
}