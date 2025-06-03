using FishingPlanner.Interfaces;
using FishingPlanner.Models;
using FishingPlanner.Repositories;
using FishingPlanner.ViewModels; // чтобы видеть ViewModel
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace FishingPlanner.Views
{
    public partial class DayDetailView : UserControl
    {
        private readonly IFishingEventRepository repository;
        private readonly DayDetailViewModel vm;  // твой ViewModel с Today

        private double? selectedLatitude;
        private double? selectedLongitude;

        public DayDetailView(IFishingEventRepository repository, DayDetailViewModel viewModel)
        {
            InitializeComponent();

            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.vm = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            DataContext = vm;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var apiKey = "AIzaSyBDRPio_89BFdjBMvDvHe9EY6-L4JkJLPM";

            var html = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8'>
            <title>Fishing Location Picker</title>
            <style>
                html, body, #map {{
                    height: 100%;
                    margin: 0;
                    padding: 0;
                }}
            </style>
            <script src='https://maps.googleapis.com/maps/api/js?key={apiKey}&callback=initMap' async defer></script>
            <script>
                let map;
                let marker;

                function initMap() {{
                    map = new google.maps.Map(document.getElementById('map'), {{
                        center: {{ lat: 55.751244, lng: 37.618423 }},
                        zoom: 8
                    }});

                    map.addListener('click', function(e) {{
                        placeMarker(e.latLng);
                        window.chrome.webview.postMessage({{
                            lat: e.latLng.lat(),
                            lng: e.latLng.lng()
                        }});
                    }});
                }}

                function placeMarker(location) {{
                    if (marker) {{
                        marker.setMap(null);
                    }}
                    marker = new google.maps.Marker({{
                        position: location,
                        map: map
                    }});
                }}
            </script>
        </head>
        <body>
            <div id='map'></div>
        </body>
        </html>";

            await MapBrowser.EnsureCoreWebView2Async();

            MapBrowser.CoreWebView2.WebMessageReceived += MapBrowser_WebMessageReceived;

            MapBrowser.NavigateToString(html);
        }

        private void MapBrowser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var json = e.WebMessageAsJson;

            try
            {
                var point = System.Text.Json.JsonSerializer.Deserialize<GeoPoint>(json);
                if (point != null)
                {
                    selectedLatitude = point.lat;
                    selectedLongitude = point.lng;

                    MessageBox.Show($"You have chosen point: {point.lat}, {point.lng}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retriving location: " + ex.Message);
            }
        }

        private void SaveFishingSpot_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DayDetailViewModel viewModel)
            {
                string location = MapBrowser.Source?.ToString() ?? "Unknown";
                var selectedDate = DateOnly.FromDateTime(viewModel.SelectedDay.Date);

                var saveWindow = new SaveFishingWindow(location, selectedDate)
                {
                    Owner = Window.GetWindow(this)
                };

                if (saveWindow.ShowDialog() == true)
                {
                    var newEvent = saveWindow.FishingEvent;

                    _ = repository.AddAsync(newEvent);

                    MessageBox.Show("Fishing was saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BackToCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (App.CalendarView is CalendarView calendarView)
            {
                if (this.Parent is ContentControl contentControl)
                {
                    contentControl.Content = calendarView;
                }
            }
        }

        private class GeoPoint
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }
}