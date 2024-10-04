namespace EventVenueManagementCore;

public class Event
{
    public record EventBrief(string Name, string Summary, string ImageUrl, int RecommendedAge);

    public string Name { get; set; }
    public string Summary { get; set; }
    public string ImageUrl { get; set; }
    public int RecommendedAge { get; set; }
    public float Price { get; set; }
    public EventType Type { get; set; }
    
    public EventBrief GetBrief()
    {
        return new EventBrief(Name, Summary, ImageUrl, RecommendedAge);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Event other)
        {
            return Name == other.Name;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

public enum EventType
{
    Concert
}