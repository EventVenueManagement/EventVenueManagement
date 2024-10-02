using EventVenueManagementAPI;
using EventVenueManagementCore;

namespace EventVenueManagementTests.ControllerTests;

public class RegisterEventTests
{
    [Test]
    public void RegisterOneEvent()
    {
        var model = new Venue();
        var sut = new RegisterEvent(model);
        var input = EventTests.GenerateRandomEvent();
        sut.Execute(input);
        
        model.GetEvents().Should().Contain(input);
    }
}