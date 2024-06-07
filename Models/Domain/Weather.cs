using System;

namespace BlazeWeather.Models.Domain;

public struct Weather
{
    public Geocode Location { get; set; }

    public Temperature CurrentTemp { get; set; }
    public Temperature FeelsLike { get; set; }
    public Temperature MinTemp { get; set; }
    public Temperature MaxTemp { get; set; }

    public int HumidityPerecent { get; set; }

    public double SeaPressureHPA { get; set; }
    public double GroundPressureHPA { get; set; }
    
    public double RainMillimeters { get; set; }
    public double SnowMillimeters { get; set; }
    
    public double WindSpeed { get; set; }
    public double WindGust { get; set; }
    public int WindDegrees { get; set; }

    public int CloudsPercent { get; set; }

    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }

    public string IconUrl { get; set; }
    public string Description { get; set; }

    public override readonly string ToString()
    {
        return $"{Location} - {CurrentTemp}";
    }

    public readonly string ToString(TemperatureUnit unit)
    {
        return $"{Location} - {CurrentTemp.ToUnit(unit)}";
    }
}
