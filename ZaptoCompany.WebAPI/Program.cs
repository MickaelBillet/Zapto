using ZaptoCompany.WebAPI.Configuration;
using ZaptoCompany.WebAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();
app.RegisterUserEndpoints();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
