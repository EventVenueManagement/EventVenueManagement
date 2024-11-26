using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace EventVenueManagementCore;

public class EventVenueDB(DbContextOptions<EventVenueDB> options) : DbContext(options)
{
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Event>()
            .HasOne<Venue>()
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .IsRequired();
    }
}