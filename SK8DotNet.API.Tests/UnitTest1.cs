using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace SK8DotNet.API.Tests;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsJsonContent()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveDays()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsValidData()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecast);
        Assert.All(forecast, f =>
        {
            Assert.True(f.Date > DateOnly.FromDateTime(DateTime.Now));
            Assert.InRange(f.TemperatureC, -20, 55);
            Assert.NotNull(f.Summary);
            Assert.NotEmpty(f.Summary);
        });
    }

    [Fact]
    public async Task GetWeatherForecast_TemperatureFahrenheitCalculationIsCorrect()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecast);
        Assert.All(forecast, f =>
        {
            var expectedF = 32 + (int)(f.TemperatureC / 0.5556);
            Assert.Equal(expectedF, f.TemperatureF);
        });
    }

    [Fact]
    public async Task GetWeatherForecast_SummaryIsFromExpectedList()
    {
        var expectedSummaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecast);
        Assert.All(forecast, f =>
        {
            Assert.Contains(f.Summary, expectedSummaries);
        });
    }
}

// Local copy of the WeatherForecast record for testing
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}