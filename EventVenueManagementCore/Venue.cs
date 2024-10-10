namespace EventVenueManagementCore;

public class Venue
{
    private readonly List<Event> events = new();
    public virtual IEnumerable<Event> GetEvents() => events;

    public bool AddEvent(Event newEvent)
    {
        if (events.Contains(newEvent)) return false;
        
        events.Add(newEvent);
        
        return true;
    }

    public List<Event> AddMultipleEvents(IEnumerable<Event> input)
    {
        return input.Where(e => !AddEvent(e)).ToList();
    }

    public Event? GetEvent(string name)
    {
        return events.FirstOrDefault(e => e.Name == name);
    }
    
    public IEnumerable<Event.EventBrief> GetEventsBrief()
    {
        return events.Select(e => e.GetBrief());
    }
}