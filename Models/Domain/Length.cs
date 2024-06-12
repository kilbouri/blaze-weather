using System;

namespace BlazeWeather.Models.Domain;

public readonly struct Length {
    public readonly double Value;
    public readonly LengthUnit Unit;
    
    public readonly string UnitAbbreviation => Unit switch
    {
        LengthUnit.Millimeters => "mm",
        LengthUnit.Centimeters => "cm",
        LengthUnit.Meters => "m",
        LengthUnit.Kilometers => "km",
        LengthUnit.Inches => "in",
        LengthUnit.Feet => "ft",
        LengthUnit.Yards => "yd",
        LengthUnit.Miles => "mi",
        _ => throw new NotImplementedException($"{Unit} is not a known LengthUnit")
    };

    public Length(LengthUnit unit, double length)
        => (Unit, Value) = (unit, length);

    /// <summary>
    /// Returns a new Length object which has been converted to the requested
    /// LengthUnit. If the length is already measured in the correct unit,
    /// a copy of the object will NOT be made.
    /// </summary>
    /// <param name="newUnit">The new unit the length should be measured in</param>
    /// <returns>A Length object converted to the requested LengthUnit</returns>
    /// <exception cref="NotImplementedException">
    ///     <paramref name="newUnit"/> does not have an implemented conversion from centimeters, -or- 
    ///     this.Unit does not have an implemented conversion to centimeters.
    /// </exception>
    public Length ToUnit(LengthUnit newUnit)
    {
        if (newUnit == Unit)
        {
            return this;
        }

        // Centimeters is chosen as the reference unit because the smallest US Customary
        // measure supported is Inches, which are larger than centimeters (2.54x). Further,
        // the only supported measure smaller than centimeters is millimeters, which,
        // because the metric system is sensible, is exactly a factor of 10 smaller.
        // Thus, the conversion to both the next smallest and next largest measure
        // are nice. Millimeters would work too, but centimeters allows the intermediate
        // value to stay away from extremes in most cases and thus maintains better precision.
        double centimeters = Unit switch
        {
            LengthUnit.Millimeters => Value / 10,
            LengthUnit.Centimeters => Value,
            LengthUnit.Meters => Value * 100,
            LengthUnit.Kilometers => Value * 100_000,
            LengthUnit.Inches => Value * 2.54, // 1 inch = 25.4mm or 2.54cm (https://en.wikipedia.org/wiki/Inch)
            LengthUnit.Feet => Value * 12 * 2.54, // 1 foot = 12 inches
            LengthUnit.Yards => Value * 3 * 12 * 2.54, // 1 yard = 3 feet
            LengthUnit.Miles => Value * 1760 * 3 * 12 * 2.54, // 1 mile = 1760 yards
            _ => throw new NotImplementedException($"{Unit} is not a known LengthUnit")
        };

        double asNewUnit = newUnit switch
        {
            LengthUnit.Millimeters => centimeters * 10,
            LengthUnit.Centimeters => centimeters,
            LengthUnit.Meters => centimeters / 100,
            LengthUnit.Kilometers => centimeters / 100_000,
            LengthUnit.Inches => centimeters / 2.54,
            LengthUnit.Feet => centimeters / 12 / 2.54,
            LengthUnit.Yards => centimeters / 3 / 12 / 2.54,
            LengthUnit.Miles => centimeters / 1760 / 3 / 12 / 2.54,
            _ => throw new NotImplementedException($"{Unit} is not a known LengthUnit")
        };

        return new Length(newUnit, asNewUnit);
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
