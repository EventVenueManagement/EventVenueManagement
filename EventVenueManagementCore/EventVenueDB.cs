using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace EventVenueManagementCore;

public class EventVenueDB : DbContext
{
    public EventVenueDB(DbContextOptions<EventVenueDB> options) : base(options)
    {
        // var connString = options.Extensions.OfType<NpgsqlOptionsExtension>().FirstOrDefault()?.ConnectionString;
        // Console.WriteLine($"Connection String: {connString}");
    }

    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Event>()
            .HasOne<Venue>()
            .WithMany()
            .HasForeignKey(e => e.VenueId)
            .IsRequired();
    }
}