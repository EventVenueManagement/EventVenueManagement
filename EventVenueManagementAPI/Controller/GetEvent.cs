using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class GetEvent(Task<Venue> modelPromise) : GetController<string, Results<NotFound, Ok<Event>>>
{
    public async Task<Results<NotFound, Ok<Event>>> Execute(string name)
    {
        var @event = (await modelPromise).GetEvent(name);
        return @event == null ? TypedResults.NotFound() : TypedResults.Ok(@event);
    }
}