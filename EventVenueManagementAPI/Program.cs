using System.Text;
using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supabase.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddDbContext<EventVenueDB>(
    options => options.UseNpgsql( builder.Configuration.GetConnectionString("DATABASE_CONNECTION") ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION"))
);

builder.Services.AddScoped<Venue>();
builder.Services.AddSingleton<Supabase.Client>(_ =>
{
    var supabaseUrl = builder.Configuration["SUPABASE_URL"];
    var supabaseKey = builder.Configuration["SUPABASE_KEY"];
    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = true,
    };
    return new Supabase.Client(supabaseUrl, supabaseKey, options);
});

var supabaseSignatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SUPABASE_SIGNATURE_KEY"]!));
var validIssuers = "https://epffdwtxkoxgdfdoyemj.supabase.co/auth/v1";
var validAudiences = new List<string>() { "authenticated" };
 
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = supabaseSignatureKey,
        ValidAudiences = validAudiences,
        ValidIssuer = validIssuers
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapGet("/", (EventVenueDB db) => db.Events).RequireAuthorization();
app.MapPost("/event" , (Event @event, EventVenueDB db, Venue venue) => new RegisterEvent(venue, db).Execute(@event));
app.MapPost("/events" , (List<Event> events, Venue venue) => new RegisterEvents(venue).Execute(events));
app.MapGet("/event/{name}" , (string name, Venue venue) => new GetEvent(venue).Execute(name));
app.MapGet("/frontbillboard" , (Venue venue) => new GetFrontBillboard(venue).Execute());
app.MapGet("/login", async (Supabase.Client sc) =>
{
    var session = await sc.Auth.SignIn("javiertorralbocortes@gmail.com", "123");
    return session.AccessToken;
});
app.Run();