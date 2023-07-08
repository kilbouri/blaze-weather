namespace BlazeWeather.Models.Domain;

public class GeocodeOption
{
	public float Confidence { get; set; }

	public string City { get; set; } = "";
	public string State { get; set; } = "";
	public string Country { get; set; } = "";

	public Geocode ResolvedGeocode { get; set; }

	public override string ToString()
	{
		return $"{City}, {State}, {Country} -- {ResolvedGeocode}";
	}
}
