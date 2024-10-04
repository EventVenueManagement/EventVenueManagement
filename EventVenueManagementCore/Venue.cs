namespace EventVenueManagementCore;

public class Venue
{
    private readonly List<Event> events = new();
    public virtual IEnumerable<Event> GetEvents()
    {
        return events;
    }

    public bool AddEvent(Event newEvent)
    {
        if (events.Contains(newEvent)) return false;
        
        events.Add(newEvent);
        
        return true;
    }
}