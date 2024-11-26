using System.Net;
using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class RegisterEvent(Venue model, EventVenueDB db) : PostController<Event, Results<Created, Conflict>>
{
    public Results<Created, Conflict> Execute(Event input)
    {
        var isAdded = model.AddEvent(input);

        if (!isAdded) return TypedResults.Conflict();
        
        db.SaveChanges();
        return TypedResults.Created();
    }
}