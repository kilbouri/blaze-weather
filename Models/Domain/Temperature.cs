using System;
using System.Reflection.Metadata.Ecma335;

namespace BlazeWeather.Models.Domain;

public readonly struct Temperature {

    public readonly TemperatureUnit Unit;
    public readonly double Degrees;

    public Temperature(TemperatureUnit unit, double degrees)
    {
        Unit = unit;
        Degrees = degrees;
    }

    /// <summary>
    /// Returns a new Temperature object which has been converted to the requested
    /// TemperatureUnit. If the temperature is already measured in the correct unit,
    /// a copy of the object will NOT be made.
    /// </summary>
    /// <param name="newUnit">The new unit the temperature should be measured in</param>
    /// <returns>A new Temperature object converted to the requested TemperatureUnit</returns>
    /// <exception cref="NotImplementedException">
    ///     <paramref name="newUnit"/> does not have an implemented conversion from celsius, -or- 
    ///     this.Unit does not have an implemented conversion to celsius.
    /// </exception>
    public Temperature ToUnit(TemperatureUnit newUnit)
    {
        // This is an immutable struct, we don't need to make a copy.
        if (newUnit == this.Unit)
        {
            return this;
        }

        double celsius = this.Unit switch
        {
            TemperatureUnit.Celsius => this.Degrees,
            TemperatureUnit.Fahrenheit => (this.Degrees - 32.0) / 1.8,
            TemperatureUnit.Kelvin => this.Degrees - 273.15,
            _ => throw new NotImplementedException($"{Unit} is not a known TemperatureUnit")
        };

        return newUnit switch
        {
            TemperatureUnit.Celsius => new Temperature(newUnit, celsius),
            TemperatureUnit.Fahrenheit => new Temperature(newUnit, (1.8 * celsius) + 32.0),
            TemperatureUnit.Kelvin => new Temperature(newUnit, celsius + 273.15),
            _ => throw new NotImplementedException($"{newUnit} is not a known TemperatureUnit"),
        };
    }

    public override string ToString()
    {
        return Degrees.ToString(".##") + "°" + (Unit switch {
            TemperatureUnit.Celsius => "C",
            TemperatureUnit.Fahrenheit => "F",
            TemperatureUnit.Kelvin => "K",
            _ => throw new NotImplementedException($"{Unit} is not a known TemperatureUnit")
        });
    }
}
