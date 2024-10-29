namespace EventVenueManagementTests;

public class Show(Room room)
{
    private List<Zone.Seat> seats = new();
    public bool BuySeat(string id)
    {
        if (!IsAvailable(id)) return false;

        return room.PriceOf(id).Map(x => new Zone.Seat(id, x)).Match(
            Some: x => { 
                seats.Add(x);
                return true;
            },
            None: false
        );
    }

    public bool IsAvailable(string s)
    {
        return seats.All(x => x.Id != s);
    }
}