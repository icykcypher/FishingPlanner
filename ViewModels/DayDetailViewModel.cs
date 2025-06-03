using FishingPlanner.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace FishingPlanner.ViewModels
{
    public class DayDetailViewModel : INotifyPropertyChanged
    {
        public CalendarDay SelectedDay { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _weatherDescription;
        public string WeatherDescription
        {
            get => _weatherDescription;
            set { _weatherDescription = value; OnPropertyChanged(nameof(WeatherDescription)); }
        }

        private string _temperature;
        public string Temperature
        {
            get => _temperature;
            set { _temperature = value; OnPropertyChanged(nameof(Temperature)); }
        }

        private string _pressure;
        public string Pressure
        {
            get => _pressure;
            set { _pressure = value; OnPropertyChanged(nameof(Pressure)); }
        }

        private string _humidity;
        public string Humidity
        {
            get => _humidity;
            set { _humidity = value; OnPropertyChanged(nameof(Humidity)); }
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get => _windSpeed;
            set { _windSpeed = value; OnPropertyChanged(nameof(WindSpeed)); }
        }

        private string _sunrise;
        public string Sunrise
        {
            get => _sunrise;
            set { _sunrise = value; OnPropertyChanged(nameof(Sunrise)); }
        }

        private string _sunset;
        public string Sunset
        {
            get => _sunset;
            set { _sunset = value; OnPropertyChanged(nameof(Sunset)); }
        }

        private string _fishingForecast;
        public string FishingForecast
        {
            get => _fishingForecast;
            set { _fishingForecast = value; OnPropertyChanged(nameof(FishingForecast)); }
        }

        public double Latitude { get; set; } = 49.748;
        public double Longitude { get; set; } = 13.377;

        public DayDetailViewModel(CalendarDay selectedDay)
        {
            SelectedDay = selectedDay;
            LoadLocationAsync();
            LoadWeatherAsync();
        }

        private async Task LoadWeatherAsync()
        {
            try
            {
                var apiKey = "ba6ed06f4e76bc73aca0824a510d8b59";
                var url = $"https://api.openweathermap.org/data/2.5/weather?lat={Latitude}&lon={Longitude}&units=metric&appid={apiKey}";

                using var client = new HttpClient();
                var json = await client.GetStringAsync(url);
                dynamic data = JsonConvert.DeserializeObject(json)!;

                WeatherDescription = data.weather[0].description;
                Temperature = $"{data.main.temp} °C";
                Pressure = $"{data.main.pressure} hPa";
                Humidity = $"{data.main.humidity}%";
                WindSpeed = $"{data.wind.speed} m/s";

                long sunriseUnix = data.sys.sunrise;
                long sunsetUnix = data.sys.sunset;
                var sunriseTime = DateTimeOffset.FromUnixTimeSeconds(sunriseUnix).ToLocalTime();
                var sunsetTime = DateTimeOffset.FromUnixTimeSeconds(sunsetUnix).ToLocalTime();

                Sunrise = sunriseTime.ToString("HH:mm");
                Sunset = sunsetTime.ToString("HH:mm");

                string mainWeather = data.weather[0].main.ToString();
                FishingForecast = (mainWeather == "Clear" || mainWeather == "Clouds")
                    ? "Good fishing conditions"
                    : "Poor fishing conditions";

            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка загрузки погоды: {e.Message}");
                WeatherDescription = "Ошибка загрузки";
                Temperature = "N/A";
                Pressure = "N/A";
                Humidity = "N/A";
                WindSpeed = "N/A";
                Sunrise = "N/A";
                Sunset = "N/A";
                FishingForecast = "Unknown";
            }
        }

        private async Task LoadLocationAsync()
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync("http://ip-api.com/json/");
                dynamic data = JsonConvert.DeserializeObject(json)!;

                Latitude = data.lat;
                Longitude = data.lon;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error loading location data: {e.Message}");
                Latitude = 49.748; 
                Longitude = 13.377;
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}