using LanguageExt;

namespace EventVenueManagementTests;

public class Room(Zone zone)
{
    public Option<int> PriceOf(string id)
    {
        return zone.GetSeat(id).Map(x => x.Price);
    }

    public Option<Zone.Seat> GetSeat(string id)
    {
        return zone.GetSeat(id);
    }

    public bool SeatExist(string id)
    {
        throw new NotImplementedException();
    }
}