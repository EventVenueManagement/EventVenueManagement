using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Moq;

namespace EventVenueManagementTests.ControllerTests;

public class GetFrontBillboardTests
{
    [Test]
    public void GetFrontBillboardWithFilms()
    {
        var venue = new Venue();

        var event1 = new Event() {
            Name = "Test Event",
            Summary = "Test Summary",
            ImageUrl = "Test Image",
            RecommendedAge = 18,
        };
        var event2 = new Event() {
            Name = "Test Event 2",
            Summary = "Test Summary 2",
            ImageUrl = "Test Image 2",
            RecommendedAge = 21
        };
        
        venue.AddEvent(event1);
        venue.AddEvent(event2);
        
        var sut = new GetFrontBillboard(venue);
        
        sut.Execute().Value.Should().BeEquivalentTo(new List<Event.EventBrief>
        {
            event1.GetBrief(),
            event2.GetBrief(),
        });
    }
}