using Agora.Shared.Infrastructure;
using Agora.Stores.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Authentication services.
// * https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/7.0/default-authentication-scheme
// builder.Services.AddAuthentication().AddCookie(); // ! cookie only for now

// PostgreSQL services.
var connectionString = builder.Configuration.GetConnectionString("PostgreSql")!;
builder.Services.AddPostgreSql(connectionString);

// Store services.
builder.Services.AddStoreMigrations(connectionString);
builder.Services.AddStores();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();