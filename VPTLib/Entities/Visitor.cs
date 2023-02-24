namespace VPTLib;

public class Visitor
{
    private readonly DateTime _birthDate;
    
    public Visitor(DateTime birthDate)
    {
        _birthDate = birthDate;
    }
    
    public bool IsKid => _birthDate > DateTime.Now.AddYears(-12);
}