using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class GetEvent(Venue model) : GetController<string, Ok<Event>>
{
    public Ok<Event> Execute(string name)
    {
        return TypedResults.Ok(model.GetEvent(name));
    }
}