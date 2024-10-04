using System.Net;
using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class RegisterEvents(Venue model) : PostController<List<Event>, Results<Created, Conflict<string>>>
{
    public Results<Created, Conflict<string>> Execute(List<Event> input)
    {
        var duplicatedEvents = 
            model.AddMultipleEvents(input).Select(e => e.Name).ToList();
        if (duplicatedEvents.Count != input.Count)
        {
            return TypedResults.Created();
        }
        return TypedResults.Conflict("The following events already exist: " + string.Join(", ", duplicatedEvents));
    }
}