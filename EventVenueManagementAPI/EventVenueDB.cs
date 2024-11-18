using EventVenueManagementCore;
using Microsoft.EntityFrameworkCore;

namespace EventVenueManagementAPI;

public class EventVenueDB(DbContextOptions<EventVenueDB> options) : DbContext(options)
{
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }
}