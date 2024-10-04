using System.Net;
using EventVenueManagementAPI.Controller;
using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;

namespace EventVenueManagementAPI;

public class RegisterEvent(Venue model) : PostController<Event>
{
    public HttpResponseMessage Execute(Event input)
    {
        bool isAdded = model.AddEvent(input);
        return new HttpResponseMessage(isAdded ? HttpStatusCode.Accepted : HttpStatusCode.Conflict);
    }
}