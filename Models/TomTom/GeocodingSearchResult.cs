using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazeWeather.Models.TomTom;

public class GeocodingSearchResult
{

	public class SearchSummary
	{
		public string Query { get; set; } = null!;
		public int NumResults { get; set; }
	}

	public class SearchResult
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public enum ResultType
		{
			Unknown = 0, // not part of API, intended to be used for "invalids"

			[JsonPropertyName("Street")]
			Street = 1,

			[JsonPropertyName("Geography")]
			Geography = 2,

			[JsonPropertyName("Point Address")]
			PointAddress = 3,

			[JsonPropertyName("Address Range")]
			AddressRange = 4,

			[JsonPropertyName("Cross Street")]
			CrossStreet = 5
		}

		public class ResultConfidence
		{
			public float Score { get; set; }
		}

		public class ResultAddress
		{
			public string StreetNumber { get; set; } = null!;
			public string StreetName { get; set; } = null!;
			public string MunicipalitySubdivision { get; set; } = null!;
			public string Municipality { get; set; } = null!;
			public string CountrySecondarySubdivision { get; set; } = null!;
			public string CountryTertiarySubdivision { get; set; } = null!;
			public string CountrySubdivision { get; set; } = null!;
			public string PostalCode { get; set; } = null!;
			public string PostalName { get; set; } = null!;
			public string ExtendedPostalCodeCountryCode { get; set; } = null!;
			public string Country { get; set; } = null!;
			public string CountryCodeISO3 { get; set; } = null!;
			public string FreeformAddress { get; set; } = null!;
			public string CountrySubdivisionName { get; set; } = null!;
			public string CountrySubdivisionCode { get; set; } = null!;
			public string LocalName { get; set; } = null!;
		}

		public class LatitudeLongitude
		{
			public float Lat { get; set; }
			public float Lon { get; set; }
		}

		public ResultType Type { get; set; }
		public ResultConfidence MatchConfidence { get; set; } = null!;
		public ResultAddress Address { get; set; } = null!;
		public LatitudeLongitude Position { get; set; } = null!;

	}

	public SearchSummary Summary { get; set; } = null!;
	public List<SearchResult> Results { get; set; } = null!;
}
