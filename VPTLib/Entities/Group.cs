using VPTLib.Exceptions;

namespace VPTLib;

public class Group
{
    private readonly List<Visitor> _visitors;
    
    public Group(List<Visitor> visitors)
    {
        _visitors = SortVisitors(visitors);
    }

    public IReadOnlyList<Visitor> Visitors => _visitors;
    public int AmountOfVisitors => _visitors.Count;
    public int AmountOfKids => _visitors.Count(v => v.IsKid);
    public bool HasKids => AmountOfKids > 0;
    public int AmountOfAdults => _visitors.Count(v => !v.IsKid);
    public int AmountOfOtherParents => AmountOfAdults - (int) Math.Ceiling((double) AmountOfKids / 2);
    public int AmountWithoutOtherParents => AmountOfVisitors - AmountOfOtherParents;
    public string CoolString => string.Join("", _visitors.Select(v => v.IsKid ? "|" : "^"));

    public List<Group> SplitKidsGroupUp()
    {
        //Split the group up into groups of as many groups as possible. Max 2 kids per group. And Min 1 adult per group.
        List<Group> groups = new();
        List<Visitor> visitors = new(_visitors);
        while (visitors.Count > 0)
        {
            List<Visitor> groupVisitors = new();
            if (visitors[0].IsKid)
            {
                groupVisitors.Add(visitors[0]);
                visitors.RemoveAt(0);
                if (visitors.Count > 0 && visitors[0].IsKid)
                {
                    groupVisitors.Add(visitors[0]);
                    visitors.RemoveAt(0);
                }
            }
            if (!visitors[0].IsKid)
            {
                groupVisitors.Add(visitors[0]);
                visitors.RemoveAt(0);
                if (visitors.Count > 0 && visitors[0].IsKid)
                {
                    groupVisitors.Add(visitors[0]);
                    visitors.RemoveAt(0);
                }
            }

            groups.Add(new Group(groupVisitors));
        }

        return groups;
    }

    /// <summary>
    /// Sorts the visitors in the group alternately between kids and adults, starting with kids.
    /// </summary>
    /// <param name="visitors"></param>
    /// <returns>The sorted list</returns>
    private static List<Visitor> SortVisitors(List<Visitor> visitors)
    {
        List<Visitor> kidsList = new();
        List<Visitor> adultList = new();
        List<Visitor> sortedList = new();

        foreach (Visitor visitor in visitors)
        {
            if (visitor.IsKid)
                kidsList.Add(visitor);
            else
                adultList.Add(visitor);
        }

        int i = 0;
        if (kidsList.Count % 2 == 0)
        {
            while (kidsList.Count > 0 && adultList.Count > 0)
            {
                if (i % 2 == 0)
                {
                    sortedList.Add(kidsList[0]);
                    kidsList.RemoveAt(0);
                    sortedList.Add(adultList[0]);
                    adultList.RemoveAt(0);
                }
                else
                {
                    sortedList.Add(kidsList[0]);
                    kidsList.RemoveAt(0);
                }

                i++;
            }
        }
        else
        {
            while (kidsList.Count > 0 && adultList.Count > 0)
            {
                if (i == 0)
                {
                    sortedList.Add(kidsList[0]);
                    kidsList.RemoveAt(0);
                    sortedList.Add(adultList[0]);
                    adultList.RemoveAt(0);
                }
                else
                {
                    if (i % 2 != 0)
                    {
                        sortedList.Add(kidsList[0]);
                        kidsList.RemoveAt(0);
                        sortedList.Add(adultList[0]);
                        adultList.RemoveAt(0);
                    }
                    else
                    {
                        sortedList.Add(kidsList[0]);
                        kidsList.RemoveAt(0);
                    }
                }
                
                i++;
            }
        }
        

        if (kidsList.Count > 1) throw new GroupTooManyKidsException();
        if (kidsList.Count == 1) sortedList.Add(kidsList[0]);
        
        sortedList.AddRange(adultList.GetRange(0, adultList.Count));

        return sortedList;
    }

    public List<Group> SplitUp()
    {
        List<Group> groups = new();
        //split the group up into 2 groups with roughly the same amount of visitors.
        int separator = (int) Math.Ceiling(AmountOfVisitors / 2D);
        groups.Add(new Group(_visitors.GetRange(0, separator)));
        groups.Add(new Group(_visitors.GetRange(separator, _visitors.Count - separator)));
        return groups;
    }
}