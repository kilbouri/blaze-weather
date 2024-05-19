namespace BlazeWeather.Models.Domain;

public struct Temperature {

    public enum TemperatureUnit {
        Celcius
    }

    public TemperatureUnit Unit;
    public double Degrees;

    public static Temperature FromCelsius(double celsius) {
        return new Temperature() {
            Unit = TemperatureUnit.Celcius,
            Degrees = celsius
        };
    }
}
