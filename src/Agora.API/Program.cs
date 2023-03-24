using Agora.Shared.Infrastructure;
using Agora.Stores.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Authentication services.
// * https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/7.0/default-authentication-scheme
builder.Services.AddAuthentication().AddCookie(); // ! cookie only for now

// PostgreSQL services.
builder.Services.AddPostgreSql(builder.Configuration.GetConnectionString("Default")!);
// Store services.
builder.Services.AddStores();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
