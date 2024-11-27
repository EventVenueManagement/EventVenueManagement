using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EventVenueManagementTests.ControllerTests;

public class RegisterEventsTests
{
    [Test]
    public async Task RegisterMultipleEventsWithDuplication()
    {
        var model = new Venue();
        var dbOptions = new DbContextOptionsBuilder<EventVenueDB>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var sut = new RegisterEvents(Task.FromResult(model), new EventVenueDB(dbOptions));
        var event1 = EventTests.GenerateRandomEvent();
        var event2 = EventTests.GenerateRandomEvent();
        var input = new List<Event> {event1, event2};

        
        await sut.Execute(input);
        (await sut.Execute(input)).Result.As<Conflict<string>>().Value.Should().Be("The following events already exist: " + event1.Name + ", " + event2.Name);
    }
}