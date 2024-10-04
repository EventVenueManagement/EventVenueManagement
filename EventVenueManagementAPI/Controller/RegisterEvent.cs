using System.Net;
using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class RegisterEvent(Venue model) : PostController<Event, Results<Created, Conflict>>
{
    public Results<Created, Conflict> Execute(Event input)
    {
        bool isAdded = model.AddEvent(input);
        return isAdded ? TypedResults.Created() : TypedResults.Conflict();
    }
}