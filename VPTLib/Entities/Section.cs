using VPTLib.Exceptions;

namespace VPTLib;

public class Section
{
    private List<Seat> _seats;
    private readonly int _rows;
    private readonly int _columns;
    
    public Section(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        CreateSeats();
    }

    public int Rows => _rows;
    public int Columns => _columns;
    public IReadOnlyList<Seat> Seats => _seats;

    public bool HasAvailableSeat => _seats.Any(s => s.IsAvailable);
    public int MaxAvailableSeatsOnRow => _seats.GroupBy(s => s.Row).Max(g => g.Count(s => s.IsAvailable));
    public int MaxKidsAmount => (int) Math.Floor((2D * _columns - 1D) / 3D + 0.5D);
    public List<Seat> AvailableFirstRowSeats => _seats.Where(s => s is { IsAvailable: true, Row: 0 }).ToList();

    private void CreateSeats()
    {
        _seats = new List<Seat>();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                _seats.Add(new Seat(i, j));
            }
        }

        _seats = _seats.OrderBy(seat => seat.Row).ThenBy(seat => seat.Column).ToList();
    }

    public bool IsFirstRowOptimalForGroup(Group group)
    {
        return AvailableFirstRowSeats.Count == group.AmountOfVisitors;
    }

    public void PlaceGroupOnFirstRow(Group group)
    {
        List<Seat> availableSeats = AvailableFirstRowSeats;
        for (int i = 0; i < group.Visitors.Count; i++)
        {
            try
            {
                Visitor visitor = group.Visitors[i];
                Seat seat = availableSeats[i];
                seat.PlaceVisitor(visitor);
            }
            catch (IndexOutOfRangeException)
            {
                throw new OutOfFirstRowSeatsException();
            }
        }
    }

    public int GetOptimalRowForGroup(Group group)
    {
        int optimalRow = -1;
        for (int i = 0; i < _rows; i++)
        {
            List<Seat> rowSeats = _seats.Where(s => s.Row == i && s.IsAvailable).ToList();
            if (rowSeats.Count == group.AmountOfVisitors)
            {
                optimalRow = i;
                break;
            }
        }

        return optimalRow;
    }

    public void PlaceGroupOptimalSpot(Group group)
    {
        for (int i = 0; i < _rows; i++)
        {
            List<Seat> rowSeats = _seats.Where(s => s.Row == i && s.IsAvailable).ToList();
            if (rowSeats.Count == group.AmountOfVisitors)
            {
                for (int j = 0; j < group.Visitors.Count; j++)
                {
                    Visitor visitor = group.Visitors[j];
                    Seat seat = rowSeats[j];
                    seat.PlaceVisitor(visitor);
                }
                break;
            }
        }
    }

    public int GetAvailableRowForGroup(Group group)
    {
        int availableRow = -1;
        for (int i = 0; i < _rows; i++)
        {
            List<Seat> rowSeats = _seats.Where(s => s.Row == i && s.IsAvailable).ToList();
            if (rowSeats.Count >= group.AmountOfVisitors)
            {
                availableRow = i;
                break;
            }
        }

        return availableRow;
    }

    public void PlaceGroupOnAvailableSpot(Group group)
    {
        for (int i = 0; i < _rows; i++)
        {
            List<Seat> rowSeats = _seats.Where(s => s.Row == i && s.IsAvailable).ToList();
            if (rowSeats.Count >= group.AmountOfVisitors)
            {
                for (int j = 0; j < group.Visitors.Count; j++)
                {
                    Visitor visitor = group.Visitors[j];
                    Seat seat = rowSeats[j];
                    seat.PlaceVisitor(visitor);
                }
                break;
            }
        }
    }

    public int GetAvailableRowForIndividual()
    {
        int availableRow = -1;
        for (int i = 0; i < _rows; i++)
        {
            List<Seat> rowSeats = _seats.Where(s => s.Row == i && s.IsAvailable).ToList();
            if (rowSeats.Count > 0)
            {
                availableRow = i;
                break;
            }
        }
        return availableRow;
    }

    public void PlaceIndividual(Visitor individualVisitor)
    {
        for (int i = 0; i < _rows; i++)
        {
            List<Seat> rowSeats = _seats.Where(s => s.Row == i && s.IsAvailable).ToList();
            if (rowSeats.Count > 0)
            {
                Seat seat = rowSeats[0];
                seat.PlaceVisitor(individualVisitor);
                break;
            }
        }
    }
}