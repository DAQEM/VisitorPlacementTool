namespace VPTLib;

public class Organization
{
    private readonly List<Show> _shows = new();
    
    public void AddShow(Show show)
    {
        _shows.Add(show);
    }
    
    public IReadOnlyList<Show> Shows => _shows;
}