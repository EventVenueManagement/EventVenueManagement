﻿using System.Net;
using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class RegisterEvents(Task<Venue> modelPromise, EventVenueDB db) : PostController<List<Event>, Results<Created, Conflict<string>>>
{
    public async Task<Results<Created, Conflict<string>>> Execute(List<Event> input)
    {
        var duplicatedEvents =
            (await modelPromise).AddMultipleEvents(input).Select(e => e.Name).ToList();
        await db.SaveChangesAsync();
        if (duplicatedEvents.Count != input.Count)
        {
            return TypedResults.Created();
        }
        return TypedResults.Conflict("The following events already exist: " + string.Join(", ", duplicatedEvents));
    }
}