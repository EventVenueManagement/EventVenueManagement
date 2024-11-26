using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EventVenueManagementCore;

public class Venue
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OwnerId { get; set; }
    public ICollection<Event> Events { get; set; } = [];

    public virtual IEnumerable<Event> GetEvents() => Events;

    public bool AddEvent(Event newEvent)
    {
        newEvent.VenueId = Id;
        if (Events.Contains(newEvent)) return false;
        
        Events.Add(newEvent);

        return true;
    }

    public List<Event> AddMultipleEvents(IEnumerable<Event> input)
    {
        return input.Where(e => !AddEvent(e)).ToList();
    }

    public Event? GetEvent(string name)
    {
        return Events.FirstOrDefault(e => e.Name == name);
    }
    
    public IEnumerable<Event.EventBrief> GetEventsBrief()
    {
        return Events.Select(e => e.GetBrief());
    }
}