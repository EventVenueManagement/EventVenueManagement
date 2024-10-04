using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementTests.ControllerTests;

public class RegisterEventsTests
{
    [Test]
    public void RegisterMultipleEventsWithDuplication()
    {
        var model = new Venue();
        var sut = new RegisterEvents(model);
        var event1 = EventTests.GenerateRandomEvent();
        var event2 = EventTests.GenerateRandomEvent();
        var input = new List<Event> {event1, event2};

        
        sut.Execute(input);
        sut.Execute(input).Result.As<Conflict<string>>().Value.Should().Be("The following events already exist: " + event1.Name + ", " + event2.Name);
    }
}