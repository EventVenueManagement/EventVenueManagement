using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class GetEvent(Venue model) : GetController<string, Results<NotFound, Ok<Event>>>
{
    public Results<NotFound, Ok<Event>> Execute(string name)
    {
        var @event = model.GetEvent(name);
        return @event == null ? TypedResults.NotFound() : TypedResults.Ok(@event);
    }
}