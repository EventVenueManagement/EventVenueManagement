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
    
    [Test]
    public void RegisterMultipleEvents()
    {
        var sut = new Venue();
        var event1 = EventTests.GenerateRandomEvent();
        var event2 = EventTests.GenerateRandomEvent();
        var input = new List<Event> {event1, event2};
        sut.AddMultipleEvents(input);

        sut.GetEvents().Should().Contain(input);
    }

    [Test]
    public void GetOneEvent()
    {
        var sut = new Venue();
        var randomEvent = EventTests.GenerateRandomEvent();
        sut.AddEvent(randomEvent);
        
        sut.GetEvent(randomEvent.Name).Should().Be(randomEvent);
    }

    [Test]
    public void GetInvalidEvent()
    {
        var sut = new Venue();
        
        sut.GetEvent("RandomName").Should().BeNull();
    }
}