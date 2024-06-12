using System;

namespace BlazeWeather.Models.Domain;

public readonly struct Speed {
    public readonly double Value;
    public readonly SpeedUnit Unit;
    
    public readonly string UnitAbbreviation => Unit switch
    {
        SpeedUnit.MetersPerSecond => "m/s",
        SpeedUnit.KilometersPerHour => "km/h",
        SpeedUnit.MilesPerHour => "mph",
        _ => throw new NotImplementedException($"{Unit} is not a known SpeedUnit")
    };

    public Speed(SpeedUnit unit, double speed)
        => (Unit, Value) = (unit, speed);

    /// <summary>
    /// Returns a new Speed object which has been converted to the requested
    /// SpeedUnit. If the speed is already measured in the correct unit,
    /// a copy of the object will NOT be made.
    /// </summary>
    /// <param name="newUnit">The new unit the speed should be measured in</param>
    /// <returns>A Speed object converted to the requested SpeedUnit</returns>
    /// <exception cref="NotImplementedException">
    ///     <paramref name="newUnit"/> does not have an implemented conversion from meters per second, -or- 
    ///     this.Unit does not have an implemented conversion to meters per second.
    /// </exception>
    public Speed ToUnit(SpeedUnit newUnit)
    {
        if (newUnit == Unit)
        {
            return this;
        }

        // From https://en.wikipedia.org/wiki/Mile
        // > The statute mile was standardised between the Commonwealth of Nations 
        // > and the United States by an international agreement in 1959, when it
        // > was formally redefined with respect to SI units as exactly 1,609.344 
        // > metres. 

        double metersPerSecond = Unit switch
        {
            SpeedUnit.MetersPerSecond => Value,
            SpeedUnit.KilometersPerHour => Value / 3.6,
            SpeedUnit.MilesPerHour => Value * 1609.344 / 3600.0,
            _ => throw new NotImplementedException($"{Unit} is not a known SpeedUnit")
        };

        double asNewUnit = newUnit switch
        {
            SpeedUnit.MetersPerSecond => metersPerSecond,
            SpeedUnit.KilometersPerHour => metersPerSecond * 3.6,
            SpeedUnit.MilesPerHour => metersPerSecond * 3600.0 / 1609.344,
            _ => throw new NotImplementedException($"{Unit} is not a known SpeedUnit")
        };

        return new Speed(newUnit, asNewUnit);
    }

    public override string ToString()
    {
        return ToString(format: null);
    }

    public string ToString(string? format)
    {
        return Value.ToString(format) + " " + UnitAbbreviation;
    }
}
