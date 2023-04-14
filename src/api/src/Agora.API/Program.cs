using System.Text;

using Agora.API.Stores.Services;
using Agora.Catalogs;
using Agora.Identity;
using Agora.Identity.Infrastructure.Tokens;
using Agora.Shared;
using Agora.Shared.Infrastructure;
using Agora.Shared.Infrastructure.DependencyInjection;
using Agora.Stores;

using ErrorOr;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("PostgreSql")!;
// Shared infrastructure.
builder.Services.AddShared(connectionString);
builder.Services.AddMigrations(connectionString,
    // ! there has to be a better way to do this :D
    typeof(Agora.Catalogs.CatalogsServiceCollectionExtensions).Assembly,
    typeof(Agora.Identity.IdentityServiceCollectionExtensions).Assembly,
    typeof(Agora.Stores.StoreServiceCollectionExtensions).Assembly
);

// Messaging infrastructure
builder.Services.AddMassTransit(options =>
{
    // TODO: Add outbox/inbox.
    // it seems like I can make EFcore work with ADO.net transactions.
    // https://learn.microsoft.com/en-us/ef/core/saving/transactions#using-external-dbtransactions-relational-databases-only

    // TODO: each service registers their sagas, etc here.

    // TODO: Use RabbitMQ once I set up docker stuff
    options.UsingInMemory();
});

// Catalog services.
builder.Services.AddCatalogs();

// Identity & authentication services.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
        };
    });

builder.Services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
builder.Services.AddIdentity(sp =>
{
    var options = sp.GetRequiredService<IOptions<JwtOptions>>();
    return () => options.Value;
});

// Store services.
builder.Services.AddStores();

// API services.
builder.Services.AddProxiedScoped<StoreService>();

builder.Services.AddControllers();
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
    {
        var errors = ctx.HttpContext.Items["errors"] as IList<Error>;
        if (errors is not null)
        {
            ctx.ProblemDetails.Extensions.Add("errorCodes", errors.Select(err => err.Code));
        }
    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Put **_ONLY_** your JWT Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // ! Perhaps not the best of ideas to run this on start? Should be fine for development.
    app.Services.MigrateUp();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();