namespace BlazeWeather.Models.Domain;

public struct Weather
{
    public Geocode Location { get; set; }
    public Temperature Temperature { get; set; }
    public Temperature FeelsLike { get; set; }
    public Temperature TemperatureMin { get; set; }
    public Temperature TemperatureMax { get; set; }
    public float HumidityPerecent { get; set; }
    public float PressureHPA { get; set; }

    public override readonly string ToString()
    {
        return $"{Location} - {Temperature}";
    }

    public readonly string ToString(TemperatureUnit unit)
    {
        return $"{Location} - {Temperature.ToUnit(unit)}";
    }
}
