using System.Net;
using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class RegisterEvent(Task<Venue> modelPromise, EventVenueDB db) : PostController<Event, Results<Created, Conflict>>
{
    public async Task<Results<Created, Conflict>> Execute(Event input)
    {
        var isAdded = (await modelPromise).AddEvent(input);

        if (!isAdded) return TypedResults.Conflict();
        
        // await db.SaveChangesAsync();
        return TypedResults.Created();
    }
}