using MeDirect.Exchange.API.Middlewares;
using MeDirect.Exchange.Application.Registration;
using MeDirect.Exchange.Persistence.Registration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(new JsonFormatter(),
        "logs.json",
        restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


builder.Services.AddPersistenceRegistration();
builder.Services.AddApplicationRegistration();
builder.Services.AddMemoryCache();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<RateLimiterMiddleware>();

app.Run();
