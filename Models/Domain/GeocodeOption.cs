namespace BlazeWeather.Models.Domain;

public class GeocodeOption
{
	public string City { get; set; } = "";
	public string State { get; set; } = "";
	public string Country { get; set; } = "";

    /// <summary>The distance from the geobias, if one was given in the search.</summary>
	public double? Distance { get; set; }

    public double Confidence { get; set; }

	public Geocode ResolvedGeocode { get; set; }

	public override string ToString()
	{
		return $"{City}, {State}, {Country} -- {ResolvedGeocode}";
	}
}
