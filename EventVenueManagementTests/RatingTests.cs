namespace EventVenueManagementTests;

public class RatingTests
{
    [Theory]
    [TestCase(1.0f, 2)]
    [TestCase(1.5f, 3)]
    [TestCase(4.5f, 9)]
    public void CreateRating(float input, int real)
    {
        var rating = new Rating(input);
        rating.Score.Should().Be(real);
    }
    
}

public class Rating
{
    public Rating(float score)
    {
        Score = score*2;
    }

    public object Score { get; set; }
}