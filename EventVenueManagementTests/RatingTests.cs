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

    [Test]
    public void DisplayTest()
    {
        var rating = new Rating(3.5f);
        rating.GetStars().Should().Be("***/");
        var rating2 = new Rating(4.0f);
        rating2.GetStars().Should().Be("****");
    }
}

public class Rating
{
    public readonly string Comment;
    public readonly DateTime Date = DateTime.Now;
    public Rating(float score, string comment = "")
    {
        if (score is < 0 or > 5)
            throw new ArgumentException("Score must be between 0 and 5");
        Score = (int)(score*2);
        
        Comment = comment;
    }

    public int Score { get; private set; }

    public string GetStars()
    {
        var stars = new string('*', Score / 2);
        if (Score % 2 == 1)
            stars += "/";
        return stars;
    }
}