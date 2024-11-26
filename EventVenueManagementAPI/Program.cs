using System.Security.Claims;
using System.Text;
using EventVenueManagementAPI.Controller;
using EventVenueManagementCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
    options => options.UseNpgsql( GetEnvironmentVariable(builder, "DATABASE_CONNECTION"))
);

builder.Services.AddSingleton<Supabase.Client>(_ =>
{
    var supabaseUrl = GetEnvironmentVariable(builder, "SUPABASE_URL");
    var supabaseKey = GetEnvironmentVariable(builder, "SUPABASE_KEY");
    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = true,
    };
    return new Supabase.Client(supabaseUrl, supabaseKey, options);
});

var supabaseSignatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetEnvironmentVariable(builder, "SUPABASE_SIGNATURE_KEY") ?? string.Empty));
const string validIssuers = "https://epffdwtxkoxgdfdoyemj.supabase.co/auth/v1";
var validAudiences = new List<string> { "authenticated" };
 
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Venue>(sp =>
{   
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var db = sp.GetRequiredService<EventVenueDB>();
    
    var claims = httpContextAccessor.HttpContext?.User?.Claims.ToList() ?? [];
    var sub = claims.FirstOrDefault(c => c.Type == "sub", new Claim("sub", "")).Value;
    var subGuid = Guid.Parse(sub);
    var venue = db.Venues.Include(v => v.Events).FirstOrDefault(
        v => v.OwnerId == subGuid) ?? db.Venues.Add(new Venue { OwnerId = subGuid }).Entity;
    db.SaveChanges();
    return venue;
});


var app = builder.Build();
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication()
    .UseAuthorization();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapGet("/", (Venue venue) => venue.Id + " " + venue.OwnerId + venue.GetEvents().Count()).RequireAuthorization();
app.MapPost("/event" , (Event @event, Venue venue, EventVenueDB db) => new RegisterEvent(venue, db).Execute(@event)).RequireAuthorization();
app.MapPost("/events" , (List<Event> events, Venue venue) => new RegisterEvents(venue).Execute(events));
app.MapGet("/event/{name}" , (string name, Venue venue) => new GetEvent(venue).Execute(name));
app.MapGet("/frontbillboard" , (Venue venue) => new GetFrontBillboard(venue).Execute());
app.MapGet("/login", async (Supabase.Client sc) =>
{
    var session = await sc.Auth.SignIn("javiertorralbocortes@gmail.com", "123");
    return session.AccessToken;
});
app.Run();
return;

static string? GetEnvironmentVariable(WebApplicationBuilder webApplicationBuilder, string variable)
{
    return webApplicationBuilder.Configuration.GetConnectionString(variable) ?? webApplicationBuilder.Configuration[variable] ?? Environment.GetEnvironmentVariable(variable);
}