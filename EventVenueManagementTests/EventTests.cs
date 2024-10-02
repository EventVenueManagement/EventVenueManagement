using EventVenueManagementCore;

namespace EventVenueManagementTests;

public class EventTests
{
    
    public static Event GenerateRandomEvent()
    {
        return new Event
        {
            Name = Guid.NewGuid().ToString(),
            RecommendedAge = 0,
            ImageUrl = "",
            Summary = "",
            Price = 0,
            Type = EventType.Concert
        };
    }
    [Test]
    public void GetBrief()
    {
        var sut = GenerateRandomEvent();

        sut.GetBrief().Should().BeEquivalentTo(new Event.EventBrief(
            sut.Name,
            sut.Summary,
            sut.ImageUrl,
            sut.RecommendedAge
        ));
    }
}