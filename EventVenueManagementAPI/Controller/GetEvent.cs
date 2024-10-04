using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;

namespace EventVenueManagementAPI.Controller;

public class GetEvent(Venue model) : GetController<string, Event?>
{
    public Event? Execute(string name)
    {
        return model.GetEvent(name);
    }
}