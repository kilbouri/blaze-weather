using BlazeWeather;
using BlazeWeather.Models;
using BlazeWeather.Services;
using BlazeWeather.Services.Geocoders;
using BlazeWeather.Services.Geolocation;
using BlazeWeather.Services.Weather;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Add the app context. It is required to be Scoped so that it is re-created for each client.
builder.Services.AddScoped<BlazeWeatherContext>();

// Blaze Weather services
builder.Services.AddSingleton<IGeocoderService, TomTomGeocoderService>();
builder.Services.AddSingleton<IGeolocationService, IpGeolocationService>();
builder.Services.AddSingleton<IWeatherService, OpenWeatherAPIWeatherService>();
builder.Services.AddScoped<LocationSearchService>();

// Utility services
builder.Services.AddTransient<PeriodicJobService>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
