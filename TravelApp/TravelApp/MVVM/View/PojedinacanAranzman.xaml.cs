using BingMapsRESTToolkit;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using TravelApp.Model;
using Location = Microsoft.Maps.MapControl.WPF.Location;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for PojedinacanAranzman.xaml
    /// </summary>

    public partial class PojedinacanAranzman : Window, INotifyPropertyChanged
    {
        private string KEY = "";

        private Aranzman _aranzman { get; set; }
        public Aranzman Aranzman
        {
            get { return _aranzman; }
            set
            {
                _aranzman = value;
                OnPropertyChanged(nameof(Aranzman));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PojedinacanAranzman(Aranzman aranzman)
        {
            InitializeComponent();
            DataContext = this;

            Aranzman = aranzman;
            if (Aranzman.PictureLocation == null || Aranzman.PictureLocation == "")
            {
                SelectedImage.Source = new BitmapImage(new Uri("..\\..\\Images\\placeholder-image.jpg", UriKind.Relative));
            }
            else
            {
                SelectedImage.Source = new BitmapImage(new Uri("..\\..\\Images\\placeholder-image.png", UriKind.Relative));

                //SelectedImage.Source = new BitmapImage(new Uri(aranzman.PictureLocation, UriKind.Relative));
            }
            using (var dbContext = new MyDbContext())
            {
                Aranzman = dbContext.Arrangements.SingleOrDefault(a => a.Id == aranzman.Id);
                if (Aranzman != null)
                {
                    dbContext.Arrangements.Include(Aranzman => Aranzman.Restorani).Include(Aranzman => Aranzman.Atrakcije).Include(Aranzman => Aranzman.Smestaji);
                    Trace.WriteLine($"Restorani: {Aranzman.Restorani.Count()}");
                    Trace.WriteLine($"Atrakcije: {Aranzman.Atrakcije.Count()}");
                    Trace.WriteLine($"Smestaji: {Aranzman.Smestaji.Count()}");
                }
                

            }
            AddPin(Aranzman.StartLocation, Colors.Red);
            AddPin(Aranzman.EndLocation, Colors.Red);
            foreach (Atrakcija attr in Aranzman.Atrakcije)
            {
                AddPin(attr.Address, Colors.Blue);
            }
            foreach (Restoran rest in Aranzman.Restorani)
            {
                AddPin(rest.Address, Colors.Green);
            }
            foreach (Smestaj attr in Aranzman.Smestaji)
            {
                AddPin(attr.Address, Colors.Purple);
            }

            DrawRoute(Aranzman.StartLocation, Aranzman.EndLocation);
        }

        private void AddPin(string address, Color color)
        {
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + Uri.EscapeDataString(address) + "?o=xml&key=" + KEY;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(geocodeResponse.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            //Get all locations in the response and then extract the coordinates for the top location
            XmlNodeList locationElements = geocodeResponse.SelectNodes("//rest:Location", nsmgr);
            if (locationElements.Count == 0)
            {
                string messageBoxText = "Adresa odabranog elementa ne moze da se pronadje na mapi";
                string caption = "Greska prilikom geolokacije";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult mbResult;

                mbResult = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

            }
            else
            {
                //Get the geocode points that are used for display (UsageType=Display)
                XmlNodeList displayGeocodePoints =
                        locationElements[0].SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);
                string latitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Latitude", nsmgr).InnerText;
                string longitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Longitude", nsmgr).InnerText;
                // The pushpin to add to the map.
                Pushpin pin = new Pushpin();
                double lat = double.Parse(latitude, CultureInfo.InvariantCulture);
                double lon = double.Parse(longitude, CultureInfo.InvariantCulture);
                pin.Location = new Location(lat, lon);
                pin.Background = new SolidColorBrush(color);
                Trace.WriteLine(pin.Location.ToString());
                // Adds the pushpin to the map.
                myMap.Children.Add(pin);

            }
        }

        private XmlDocument GetXmlResponse(string requestUrl)
        {

            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }

        private async void DrawRoute(string startLocation, string endLocation)
        {
            // Define the start and end locations for the route
            var request = new RouteRequest()
            {
                RouteOptions = new RouteOptions()
                {
                    RouteAttributes = new List<RouteAttributeType>()
                    {
                        RouteAttributeType.RoutePath
                    }
                },
                Waypoints = new List<SimpleWaypoint>()
                {
                    new SimpleWaypoint() {
                        Address = startLocation
                    },
                    new SimpleWaypoint() {
                        Address = endLocation
                    }
                },
                BingMapsKey = KEY
            };
            var response = await ServiceManager.GetResponseAsync(request);

            if (response != null &&
                response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
            {

                var route = response.ResourceSets[0].Resources[0] as Route;
                var coords = route.RoutePath.Line.Coordinates; //This is 2D array of lat/long values.
                var locs = new LocationCollection();

                for (int i = 0; i < coords.Length; i++)
                {
                    locs.Add(new Location(coords[i][0], coords[i][1]));
                }

                var routeLine = new MapPolyline()
                {
                    Locations = locs,
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue),
                    StrokeThickness = 5
                };

                myMap.Children.Add(routeLine);
            }
        }
        
    }
}
