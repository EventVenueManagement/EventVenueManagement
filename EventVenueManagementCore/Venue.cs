namespace EventVenueManagementCore;

public class Venue
{
    private readonly List<Event> events = new();
    public virtual IEnumerable<Event> GetEvents()
    {
        return events;
    }

    public void AddEvent(Event newEvent)
    {
        events.Add(newEvent);
    }
}