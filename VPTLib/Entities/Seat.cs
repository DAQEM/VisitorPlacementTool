namespace VPTLib;

public class Seat
{
    private Visitor? _visitor;
    private readonly int _row;
    private readonly int _column;
    
    public Seat(int row, int column)
    {
        _row = row;
        _column = column;
    }
    
    public bool IsAvailable => _visitor == null;
    public int Row => _row;
    public int Column => _column;
    public Visitor? Visitor => _visitor;
    
    public void PlaceVisitor(Visitor visitor)
    {
        _visitor = visitor;
    }
}