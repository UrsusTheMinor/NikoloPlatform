using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nikolo.Api.Authorization;
using Nikolo.Api.Helper;
using Nikolo.Data;
using Nikolo.Logic.Contracts;
using Nikolo.Logic.Services;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new 
        HasScopeRequirement("read:messages", domain)));
});


var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

	
// Add Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => 
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console());


builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IUserTimeService, UserTimeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
	
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Nikolo API",
        Version = "1.0.0"
    });
    
    options.CustomOperationIds(apiDesc =>
        apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) 
            ? methodInfo.Name 
            : null);
 
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));  
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    });
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowAngular");
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    bool seedDefaults = configuration.GetValue<bool>("SeedDefaults");

    if (seedDefaults)
    {
        var skillService = scope.ServiceProvider.GetRequiredService<ISkillService>();
        var defaultSkills = configuration.GetSection("DefaultSkills").Get<List<string>>() ?? new List<string>();
        await DefaultValueHelper.SeedDefaultSkills(skillService, defaultSkills);
    }
}

await app.RunAsync();