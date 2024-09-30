using System.Net;
using EventVenueManagementCore;

namespace EventVenueManagementAPI.Controller;

public class GetFrontBillboard : OurController
{
    private readonly Venue model;

    public GetFrontBillboard(Venue model)
    {
        this.model = model;
    }

    public HttpResponseMessage Execute()
    {
        return new HttpResponseMessage(HttpStatusCode.Accepted);
    }
    public IEnumerable<Event.EventBrief> ExecuteInternal()
    {
        return model.GetEvents().Select(x => x.GetBrief());
    }
}

public class Venue
{
    public virtual List<Event> GetEvents()
    {
        throw new NotImplementedException();
    }
}

