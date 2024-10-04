using EventVenueManagementCore;

namespace EventVenueManagementTests;

public class VenueTests
{
    [Test]
    public void NewVenueWithoutEvents()
    {
        var sut = new Venue();

        sut.GetEvents().Should().BeEmpty();
    }
    
    [Test]
    public void VenueWithOneEvent()
    {
        var sut = new Venue();
        var randomEvent = EventTests.GenerateRandomEvent();
        sut.AddEvent(randomEvent);
        
        sut.GetEvents().Should().BeEquivalentTo(new List<Event> { randomEvent });
    }

    [Test]
    public void EventsDontDuplicateOnVenue()
    {
        var sut = new Venue();
        var randomEvent = EventTests.GenerateRandomEvent();
        sut.AddEvent(randomEvent);
        sut.AddEvent(randomEvent);

        sut.GetEvents().Count().Should().Be(1);
    }

    
}