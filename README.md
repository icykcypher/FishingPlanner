# FishingPlanner

FishingPlanner is a WPF desktop application designed to help anglers plan their fishing trips by providing detailed daily weather information and fishing forecasts. It includes an interactive map to select fishing spots and allows saving fishing events.

## Features

- Display detailed weather information for a selected day:
  - Weather description
  - Temperature
  - Pressure
  - Humidity
  - Wind speed
  - Sunrise and sunset times
- Fishing forecast based on weather conditions
- Interactive Google Maps-based location picker for selecting fishing spots
- Save fishing events with location and date
- Easy navigation with a calendar view (planned feature)

## Technologies Used

- WPF (.NET)
- MVVM design pattern
- Microsoft.Web.WebView2 for embedding the interactive map
- OpenWeatherMap API for weather data
- IP-API for approximate user location detection
- Newtonsoft.Json for JSON parsing

## Getting Started

### Prerequisites

- Windows 10 or later
- .NET 6.0 SDK or later
- Visual Studio 2022 or later recommended
- Internet connection (for map and weather API calls)
- Google Maps API key (replace in code where indicated)
- OpenWeatherMap API key (replace in code where indicated)

### Setup

1. Clone this repository.
2. Open the solution in Visual Studio.
3. Replace the API keys in `DayDetailView.xaml.cs`:
   - Google Maps API key
   - OpenWeatherMap API key
4. Build and run the project.

## Usage

- Select a day in the calendar.
- View detailed weather and fishing forecast.
- Click on the map to select a fishing location.
- Click "Save event" to save the fishing spot and details.

## Future Improvements

- Fix navigation button to return to the calendar view.
- CRUD operations for fishing events.
- Add detailed fishing tips and recommendations.
- Improve UI/UX design and responsiveness.

## License

This project is open-source and available under the MIT License.

---

If you have any questions or want to contribute, feel free to open an issue or a pull request.