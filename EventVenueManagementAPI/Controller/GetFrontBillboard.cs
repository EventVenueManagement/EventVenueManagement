using EventVenueManagementAPI.Controller.MethodControllers;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller;

public class GetFrontBillboard(Venue model) : GetController<Ok<IEnumerable<Event.EventBrief>>>
{
    public Ok<IEnumerable<Event.EventBrief>> Execute()
    {
        return TypedResults.Ok(model.GetEventsBrief());
    }
}