using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
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
using TravelApp.Model;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for PojedinacnaAtrakcija.xaml
    /// </summary>
    public partial class PojedinacnaAtrakcija : Window, INotifyPropertyChanged
    {
        private string KEY = "";

        private Model.Atrakcija _atrakcija { get; set; }

        public Model.Atrakcija Atrakcija
        {
            get { return _atrakcija; }
            set
            {
                _atrakcija = value;
                OnPropertyChanged(nameof(Atrakcija));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PojedinacnaAtrakcija(Atrakcija atrakcija)
        {
            InitializeComponent();
            DataContext = this;
            this.Atrakcija = atrakcija;

            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + Uri.EscapeDataString(atrakcija.Address) + "?o=xml&key=" + KEY;

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
                pin.Background = new SolidColorBrush(Colors.Blue);
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
    }
}
