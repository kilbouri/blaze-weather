# Blaze Weather

A weather app written in Blazor Server. Provides weather data using the OpenWeather API.

## Setup

You will need the [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download). Full Blazor setup instructions are [here](https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-cli-tutorial/intro).

## What APIs does this project use?

### [TomTom Developer](https://developer.tomtom.com/)

- [Fuzzy Search](https://developer.tomtom.com/search-api/documentation/search-service/fuzzy-search) for city-based search results
- [Reverse Geocoder](https://developer.tomtom.com/reverse-geocoding-api/documentation/reverse-geocode) for looking up location names from latitude and longitude, when not provided by a prior search. For example, when a URL containing `lat` and `lon` query parameters was pasted into the address bar, rather than being navigated to by a search result.

Thanks TomTom ❤️

### [IpGeolocation](https://ipgeolocation.io/)

- [IP Geolocation](https://ipgeolocation.io/documentation/ip-geolocation-api.html) for determining the approximate location of clients. This information is then included in location searches to bias results to the client's area. For example, an IP that is geolocated to Canada will see London, Ontario instead of London, England as the top result for "London".

Thanks IpGeolocation ❤️

### [OpenWeatherAPI](https://openweathermap.org/api)

- [Current Weather Data](https://openweathermap.org/current) for weather data at searched locations.

Thanks OpenWeatherAPI ❤️
