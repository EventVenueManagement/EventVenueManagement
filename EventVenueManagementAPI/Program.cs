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
    options => options.UseNpgsql( GetEnvironmentVariable(builder, "DATABASE_CONNECTION"),
        optionsBuilder =>
        {
            optionsBuilder.CommandTimeout(300);
        } 
    )
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


 
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    var supabaseSignatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetEnvironmentVariable(builder, "SUPABASE_SIGNATURE_KEY")));
    const string validIssuers = "https://epffdwtxkoxgdfdoyemj.supabase.co/auth/v1";
    var validAudiences = new List<string> { "authenticated" };
    
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
builder.Services.AddScoped<Task<Venue>>(async sp =>
{   
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var db = sp.GetRequiredService<EventVenueDB>();
    
    var claims = httpContextAccessor.HttpContext?.User.Claims.ToList() ?? [];
    var sub = claims.FirstOrDefault(c => c.Type == "sub", new Claim("sub", "")).Value;

    var subGuid = Guid.Parse(sub);
    var venue = await db.Venues.Include(v => v.Events).FirstOrDefaultAsync(
        v => v.OwnerId == subGuid) ?? (await db.Venues.AddAsync(new Venue { OwnerId = subGuid })).Entity;
    await db.SaveChangesAsync();
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

app.MapGet("/", () => "hello").RequireAuthorization();
app.MapPost("/event" , (Event @event, Task<Venue> venue, EventVenueDB db) => new RegisterEvent(venue, db).Execute(@event)).RequireAuthorization();
app.MapPost("/events" , (List<Event> events, Task<Venue> venue, EventVenueDB db) => new RegisterEvents(venue, db).Execute(events));
app.MapGet("/event/{name}" , (string name, Task<Venue> venue) => new GetEvent(venue).Execute(name));
app.MapGet("/frontbillboard" , (Task<Venue> venue) => new GetFrontBillboard(venue).Execute());
app.MapGet("/env", () => builder.Configuration.GetChildren().Select(c => $"{c.Key}={c.Value}"));
app.MapGet("/login", async (Supabase.Client sc) =>
{
    var session = await sc.Auth.SignIn("javiertorralbocortes@gmail.com", "123");
    return session?.AccessToken;
});
app.Run();
return;

static string GetEnvironmentVariable(WebApplicationBuilder builder, string variable)
{
    return (builder.Configuration.GetConnectionString(variable) ?? builder.Configuration[variable]) ?? string.Empty;
}