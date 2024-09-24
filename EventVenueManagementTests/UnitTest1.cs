using FluentAssertions;

namespace EventVenueManagementTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        1231.Should().Be(23423);
    }
}