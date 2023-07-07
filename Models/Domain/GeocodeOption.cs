namespace BlazeWeather.Models.Domain;

public class GeocodeOption
{
	public string City { get; set; } = "";
	public string State { get; set; } = "";
	public string Country { get; set; } = "";

	public Geocode ResolvedGeocode { get; set; }
}
