using VPTLib;

namespace VPTTests;

public class OrganizationTests
{
    [Test]
    public void AddShowTest()
    {
        // Arrange
        Organization organization = new();
        Show show = new(new List<Ticket>());
        
        // Act
        organization.AddShow(show);
        
        // Assert
        Assert.That(organization.Shows, Has.Exactly(1).Items);
    }
}