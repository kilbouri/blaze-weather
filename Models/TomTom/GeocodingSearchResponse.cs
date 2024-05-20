using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazeWeather.Models.TomTom;

// https://developer.tomtom.com/search-api/documentation/search-service/fuzzy-search
public class GeocodingSearchResponse
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

			[JsonPropertyName("POI")] PointOfInterest = 1,
			[JsonPropertyName("Street")] Street = 2,
			[JsonPropertyName("Geography")] Geography = 3,
			[JsonPropertyName("Point Address")] PointAddress = 4,
			[JsonPropertyName("Address Range")] AddressRange = 5,
			[JsonPropertyName("Cross Street")] CrossStreet = 6
            
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
			public double Lat { get; set; }
			public double Lon { get; set; }
		}

		public ResultType Type { get; set; }

        /// <summary>Distance to the geobias, if one was provided in the query</summary>
        public double Dist {get;set;}

        /// <summary>Approximately, the confidence in the result. Higher means more likely to meet search criteria</summary>
        public double Score {get;set;}

		public ResultAddress Address { get; set; } = null!;
		public LatitudeLongitude Position { get; set; } = null!;
	}

	public SearchSummary Summary { get; set; } = null!;
	public List<SearchResult> Results { get; set; } = null!;
}
