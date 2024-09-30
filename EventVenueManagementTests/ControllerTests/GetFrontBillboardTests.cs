using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Moq;

namespace EventVenueManagementTests.ControllerTests;

public class GetFrontBillboardTests
{
    [Test]
    public void GetFrontBillboardWithFilms()
    {
        var model = new Mock<Venue>();
        model.Setup(x => x.GetEvents()).Returns(new List<Event>
        {
            new()
            {
                Name = "Test Event",
                Summary = "Test Summary",
                ImageUrl = "Test Image",
                RecommendedAge = 18,
            },
            new()
            {
                Name = "Test Event 2",
                Summary = "Test Summary 2",
                ImageUrl = "Test Image 2",
                RecommendedAge = 21
            }
        });
        
        var sut = new GetFrontBillboard(model.Object);
        
        sut.ExecuteInternal().Should().BeEquivalentTo(new List<Event.EventBrief>
        {
            new("Test Event", "Test Summary", "Test Image", 18),
            new("Test Event 2", "Test Summary 2", "Test Image 2", 21)
        });
    }
}