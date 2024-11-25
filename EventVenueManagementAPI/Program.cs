using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventVenueDB>(
    options => options.UseNpgsql( builder.Configuration.GetConnectionString("DATABASE_CONNECTION") ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION"))
);

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

var venue = new Venue();
venue.AddEvent(new Event() {
    Name = "Test Event",
    Summary = "Test Summary",
    ImageUrl = "Test Image",
    RecommendedAge = 18,
});
venue.AddEvent(new Event() {
    Name = "Test Event 2",
    Summary = "Test Summary 2",
    ImageUrl = "Test Image 2",
    RecommendedAge = 21
});


app.MapGet("/", () => Console.WriteLine("hola"));
app.MapPost("/event" , (Event @event, EventVenueDB db) => new RegisterEvent(venue, db).Execute(@event));
app.MapPost("/events" , (List<Event> events) => new RegisterEvents(venue).Execute(events));
app.MapGet("/event/{name}" , (string name) => new GetEvent(venue).Execute(name));
app.MapGet("/frontbillboard" , () => new GetFrontBillboard(venue).Execute());

app.Run();