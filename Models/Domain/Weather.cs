using System;

namespace BlazeWeather.Models.Domain;

public struct Weather
{
    public Geocode Location { get; set; }

    public Temperature Current { get; set; }
    public Temperature FeelsLike { get; set; }
    public Temperature Min { get; set; }
    public Temperature Max { get; set; }

    public int HumidityPerecent { get; set; }

    public double SeaPressureHPA { get; set; }
    public double GroundPressureHPA { get; set; }
    
    public Length Rain { get; set; }
    public Length Snow { get; set; }
    
    public Speed Wind { get; set; }
    public Speed WindGust { get; set; }
    public Direction WindDirection { get; set; }

    public int CloudsPercent { get; set; }
    public Length Visibility { get; set; }

    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }
    public DateTime Updated { get; set; }

    public string IconUrl { get; set; }
    public string Description { get; set; }

    public override readonly string ToString()
    {
        return ToString(Current.Unit);
    }

    public readonly string ToString(TemperatureUnit unit)
    {
        return $"{Location} - {Current.ToUnit(unit)}";
    }
}
