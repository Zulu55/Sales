namespace Sales.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Plugin.Geolocator;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;

    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Locator();
        }

        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();
            var position = new Position(location.Latitude, location.Longitude);
            this.MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            try
            {
                this.MyMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            var pins = await this.GetPins();
            this.ShowPins(pins);
        }

        private void ShowPins(List<Pin> pins)
        {
            foreach (var pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }
        }

        private async Task<List<Pin>> GetPins()
        {
            var pins = new List<Pin>();
            var apiService = new ApiService();
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await apiService.GetList<Product>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            var products = (List<Product>)response.Result;
            foreach (var product in products.Where(p => p.Latitude != 0 && p.Longitude != 0).ToList())
            {
                var position = new Position(product.Latitude, product.Longitude);
                pins.Add(new Pin
                {
                    Address = product.Remarks,
                    Label = product.Description,
                    Position = position,
                    Type = PinType.Place,
                });
            }

            return pins;
        }

        private void Handle_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            var zoomLevel = double.Parse(e.NewValue.ToString()) * 18.0;
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            this.MyMap.MoveToRegion(new MapSpan(this.MyMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }
    }
}