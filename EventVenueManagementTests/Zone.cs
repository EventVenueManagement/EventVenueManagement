using LanguageExt;

namespace EventVenueManagementTests;

public class Zone(List<Zone.Seat> seats)
{
    public Option<Seat> GetSeat(string id)
    {
        return seats.Find(x => x.Id == id);
    }

    public record Seat(string Id, int Price);
}