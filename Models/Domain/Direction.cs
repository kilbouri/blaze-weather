using System;

namespace BlazeWeather.Models.Domain;

public readonly struct Direction
{
    public readonly double Value;
    public readonly DirectionUnit Unit;

    public readonly string UnitAbbreviation => Unit switch
    {
        DirectionUnit.MeteorologicalDegree => "°",
        _ => throw new NotImplementedException($"{Unit} is not a known DirectionUnit")
    };

    public readonly string? ShortRepresentation => Unit switch
    {
        DirectionUnit.MeteorologicalDegree => GetMeteorlogicalDegreeAbbreviation(Value),
        _ => throw new NotImplementedException($"{Unit} is not a known DirectionUnit")
    };

    public Direction(DirectionUnit unit, double value)
        => (Unit, Value) = (unit, value);

    public Direction ToUnit(DirectionUnit newUnit)
    {
        // This method is included for parity with classes such as Speed and Temperature.
        // As it stands DirectionUnit contains only one value, so no conversion is
        // required.
        _ = newUnit;
        
        return this;
    }

    public override string ToString()
    {
        return ToString(format: null);
    }

    public string ToString(string? format)
    {
        string? @short = ShortRepresentation;
        if (@short != null)
        {
            return $"{Value.ToString(format)}{UnitAbbreviation} ({@short})";
        }
        else
        {
            return $"{Value.ToString(format)}{UnitAbbreviation}";
        }
    }

    private static string GetMeteorlogicalDegreeAbbreviation(double degrees)
    {
        string[] directions = {
                "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
                "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"
        };

        // We want to evenly spread the array contents over the range [0, 360).
        // To do this, we scale the range down to [0, 1) by dividing by 360,
        // then multiply by the number of elements in the array. The final
        // modulo is required to ensure that technically-correct values such as
        // 450 degrees don't go out of bounds.
        int numDirections = directions.Length;
        return directions[(int)Math.Round(numDirections * degrees / 360.0) % numDirections];
    }
}
