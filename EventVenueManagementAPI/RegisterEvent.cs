using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;

namespace EventVenueManagementAPI;

public class RegisterEvent(Venue model) : PostController<Event>
{
    public void Execute(Event input)
    {
        model.AddEvent(input);
    }
}