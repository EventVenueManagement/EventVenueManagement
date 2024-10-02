using EventVenueManagementCore;

namespace EventVenueManagementAPI.Controller;

public class GetFrontBillboard(Venue model) : GetController<IEnumerable<Event.EventBrief>>
{
    public IEnumerable<Event.EventBrief> Execute()
    {
        return model.GetEvents().Select(x => x.GetBrief());
    }
}