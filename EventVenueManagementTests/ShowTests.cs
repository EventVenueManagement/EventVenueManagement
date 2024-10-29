using System.Collections;
using Moq;

namespace EventVenueManagementTests;

public class ShowTests
{

    [Test]
    public void GetPriceOfSeatFromRoom()
    {
        var zone = new Zone(new List<Zone.Seat>(){
            new("0", 10),
        });
        
        var sut = new Room(zone);
        
        sut.PriceOf("0").Match(
            Some: x => x.Should().Be(10),
            None: () => Assert.Fail()
        );
        sut.PriceOf("-1").IsNone.Should().BeTrue();
    }
}