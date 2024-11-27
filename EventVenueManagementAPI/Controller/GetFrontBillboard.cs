using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class GetFrontBillboard(Task<Venue> modelPromise) : GetController<Ok<IEnumerable<Event.EventBrief>>>
{
    public async Task<Ok<IEnumerable<Event.EventBrief>>> Execute()
    {
        return TypedResults.Ok((await modelPromise).GetEventsBrief());
    }
}