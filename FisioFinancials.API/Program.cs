using FisioFinancials.API.Config;
using FisioFinancials.API.Services;
using FisioFinancials.API.Services.Interfaces;
using FisioFinancials.Domain.Loader;
using FisioFinancials.Domain.Model.Entities;
using FisioFinancials.Infrastructure.Data.Context;
using FisioFinancials.Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
       x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddIdentity<User, IdentityRole>(opts =>
    {
        opts.ClaimsIdentity.UserIdClaimType = "UserID";
    })
    .AddEntityFrameworkStores<FisioFinancialsDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ITokenService, TokenService>();

var jwtSection = builder.Configuration.GetSection("JwtBearerTokenSettings");
builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);
var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtBearerTokenSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtBearerTokenSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.User.AllowedUserNameCharacters = null;
});

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()));

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

DependencyInjectorHelper.Register(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("MyPolicy");
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.RunLoaders();
app.Run();

static void MigrateNewChangesToDatabase(IApplicationBuilder app)
{
    using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

    var context = serviceScope.ServiceProvider.GetRequiredService<FisioFinancialsDbContext>();
    context.Database.Migrate();
}
