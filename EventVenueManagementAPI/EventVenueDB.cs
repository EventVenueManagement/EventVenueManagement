using EventVenueManagementCore;
using Microsoft.EntityFrameworkCore;

namespace EventVenueManagementAPI;

public class EventVenueDB : DbContext
{
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }
}