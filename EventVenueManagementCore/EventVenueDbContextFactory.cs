using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventVenueManagementCore;

public class EventVenueDbContextFactory : IDesignTimeDbContextFactory<EventVenueDB>
{
    public EventVenueDB CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

        var optionsBuilder = new DbContextOptionsBuilder<EventVenueDB>();
        optionsBuilder.UseNpgsql(connectionString);

        return new EventVenueDB(optionsBuilder.Options);
    }
}