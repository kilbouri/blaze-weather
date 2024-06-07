using System;

namespace BlazeWeather.Models.TomTom;

public class ReverseGeocodingSearchResponse
{
    public class SummaryPayload
    {
        public int QueryTime { get; set; }
        public int NumResults { get; set; }
    }

    public class AddressesPayload
    {
        public class AddressInfo
        {
            public string Municipality { get; set; } = "";
            public string CountrySubdivision { get; set; } = "";
            public string Country { get; set; } = "";
        }

        public AddressInfo Address { get; set; } = new();
        public string Position { get; set; } = "";
    }

    public SummaryPayload Summary { get; set; } = new();
    public AddressesPayload[] Addresses { get; set; } = Array.Empty<AddressesPayload>();
}
