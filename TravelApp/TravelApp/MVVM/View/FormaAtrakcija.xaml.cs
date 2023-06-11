using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
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
    /// Interaction logic for FormaAtrakcija.xaml
    /// </summary>
    public partial class FormaAtrakcija : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        Dictionary<string, Pushpin> pinMap = new Dictionary<string, Pushpin>();

        private string KEY = "";

        private string _adresa { get; set; }
        public string Adresa
        {
            get { return _adresa; }
            set
            {
                _adresa = value;
                OnPropertyChanged(nameof(Adresa));
            }
        }

        private string _naziv;
        public string Naziv
        {
            get { return _naziv; }
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(Naziv));
            }
        }

  
        private string _opis { get; set; }
        public string Opis
        {
            get { return _opis; }
            set
            {
                _opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        private string _nazivError { get; set; }
        public string NazivError
        {
            get { return _nazivError; }
            set
            {
                _nazivError = value;
                OnPropertyChanged(nameof(NazivError));
            }
        }

        private string _adresaError { get; set; }
        public string AdresaError
        {
            get { return _adresaError; }
            set
            {
                _adresaError = value;
                OnPropertyChanged(nameof(AdresaError));
            }
        }

        private string _opisError { get; set; }
        public string OpisError
        {
            get { return _opisError; }
            set
            {
                _opisError = value;
                OnPropertyChanged(nameof(OpisError));
            }
        }

        public string Slika { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool edit { get; set; }

        private Atrakcija editAtrakcija { get; set; }

        private int _indeks { get; set; }
        public int indeks
        {
            get { return _indeks; }
            set
            {
                _indeks = value;
                OnPropertyChanged(nameof(indeks));
            }
        }

        private int _brOdabranih { get; set; }
        public int brOdabranih
        {
            get { return _brOdabranih; }
            set
            {
                _brOdabranih = value;
                OnPropertyChanged(nameof(brOdabranih));
            }
        }

        private bool _hasNoErrors;
        public bool HasNoErrors
        {
            get { return _hasNoErrors; }
            set
            {
                _hasNoErrors = value;
                OnPropertyChanged(nameof(HasNoErrors));
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Naziv" && string.IsNullOrEmpty(Naziv))
                {
                    HasNoErrors = false;
                    NazivError = "Mora da postoji naziv.";
                    return "Mora da postoji naziv.";
                }
                if (columnName == "Naziv" && !string.IsNullOrEmpty(Naziv))
                {
                    Trace.WriteLine("TRUE NAZIV");
                    NazivError = "";
                }
                if (columnName == "Adresa" && string.IsNullOrEmpty(Adresa))
                {
                    HasNoErrors = false;
                    AdresaError = "Mora da postoji adresa";
                    return "Mora da postoji adresa";
                }
                if (columnName == "Adresa" && !string.IsNullOrEmpty(Adresa))
                {
                    Trace.WriteLine("TRUE ADRESA");
                    AdresaError = "";
                }
                if ( columnName == "Opis" && string.IsNullOrEmpty(Opis))
                {
                    HasNoErrors = false;
                    OpisError = "Mora da postoji opis";
                    return "Mora da postoji opis";
                }
                if (columnName == "Opis" && !string.IsNullOrEmpty(Opis))
                {
                    Trace.WriteLine("TRUE OPIS");
                    OpisError = "";
                }
                if (!string.IsNullOrEmpty(Naziv) && !string.IsNullOrEmpty(Adresa) && !string.IsNullOrEmpty(Opis))
                {
                    Trace.WriteLine("TRUE");
                    HasNoErrors = true;
                    return null;
                }
                return null;
            }
        }
        public string Error
        {
            get { return null; }
        }
        public FormaAtrakcija()
        {
            InitializeComponent();
            DataContext = this;
            HasNoErrors = false;
        }

        public FormaAtrakcija(Atrakcija atrakcija, int brOdabranih, int indeks)
        {
            edit = true;
            editAtrakcija = atrakcija;
            this.indeks = indeks;
            this.brOdabranih = brOdabranih;
            

            InitializeComponent();
            this.Title = "Izmeni atrakciju";

            var elem = this.FindName("ListForEdit") as StackPanel;
            elem.Visibility = Visibility.Visible;

            DataContext = this;
            HasNoErrors = false;          

            Naziv = atrakcija.Name;
            Opis = atrakcija.Description;
            Adresa = atrakcija.Address;
            Slika = atrakcija.PictureLocation;
            SelectedImage.Source = new BitmapImage(new Uri(atrakcija.PictureLocation, UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (edit)
                {
                    using (var dbContext = new MyDbContext())
                    {
                        var atrakcija = dbContext.Attractions.SingleOrDefault(a => a.Id == editAtrakcija.Id);

                        if (atrakcija != null)
                        {
                            atrakcija.Name = Naziv;
                            atrakcija.Description = Opis;
                            atrakcija.Address = Adresa;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            string messageBoxText = "Greška prilikom izmene atrakcije";
                            string caption = "Izmena atrakcije";
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Information;
                            MessageBoxResult mbResult;

                            mbResult = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                        }
                    }
                    this.Close();
                }
                else 
                {
                    Atrakcija atrakcija = new Atrakcija(Naziv, Opis, Adresa);
                    using (var dbContext = new MyDbContext())
                    {
                        dbContext.Attractions.Add(atrakcija);
                        dbContext.SaveChanges();
                    }

                    string messageBoxText = "Nova atrakcija je uspešno sačuvana! Da li zelite da dodate jos?";
                    string caption = "Čuvanje";
                    MessageBoxButton button = MessageBoxButton.YesNo;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBoxResult result;

                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                    if (result == MessageBoxResult.Yes)
                    {
                        Naziv = "";
                        Opis = "";
                        Adresa = "";
                        Slika = "";
                        SelectedImage.Source = new BitmapImage(new Uri("..\\..\\Images\\placeholder-image.png"));
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                }                
            }
            catch (Exception ex) {
                string messageBoxText = "Došlo je do greške prilikom sačuvanja atrakcije. Pokušajte ponovo.";
                string caption = "Čuvanje";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string messageBoxText = "Jeste li sigurni da želite odustati? Svi podaci koji nisu sačuvani će se izgubiti.";
            string caption = "Odustajanje od čuvanja aranžmana";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                Slika = selectedImagePath;
                SelectedImage.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        public void AddStartLocation(object sender, RoutedEventArgs e)
        {
            RemovePin("startLocation");
            AddPin("startLocation", Adresa, Colors.Red);
        }

        private void AddPin(string key, string address, Color color)
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
                Trace.WriteLine(myMap.Children.Count);
                pinMap.Add(key, pin);

            }
        }

        private void RemovePin(string key)
        {
            try
            {
                Pushpin pin = pinMap[key];
                pinMap.Remove(key);
                myMap.Children.Remove(pin);
            }
            catch {
                var pushpinsToRemove = myMap.Children.OfType<Pushpin>().ToList();
                foreach (var pushpin in pushpinsToRemove)
                {
                    myMap.Children.Remove(pushpin);
                }
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

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var pushpinsToRemove = myMap.Children.OfType<Pushpin>().ToList();
            foreach (var pushpin in pushpinsToRemove)
            {
                myMap.Children.Remove(pushpin);
            }

            e.Handled = true;

            Point mousePosition = e.GetPosition(myMap);
            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + pin.Location.Latitude.ToString().Replace(",", ".") + "," + pin.Location.Longitude.ToString().Replace(",", ".") + "?o=xml&key=" + KEY;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(geocodeResponse.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            //Get all locations in the response and then extract the coordinates for the top location
            XmlNodeList locationElements = geocodeResponse.SelectNodes("//rest:Location", nsmgr);
            string address =
                        locationElements[0].SelectSingleNode(".//rest:Name", nsmgr).InnerText;

            Adresa = address;
            RemovePin("startLocation");
            AddPin("startLocation", Adresa, Colors.Red);

        }

        private void MapWithPushpins_TouchDown(object sender, TouchEventArgs e)
        {

            // Get the touch position relative to the MapWithPushpins control
            TouchPoint touchPosition = e.GetTouchPoint(myMap);

            // Convert the touch position to a location on the map
            Location pinLocation = myMap.ViewportPointToLocation(touchPosition.Position);

            // Create and add the pushpin to the map
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            var pushpinsToRemove = myMap.Children.OfType<Pushpin>().ToList();
            foreach (var pushpin in pushpinsToRemove)
            {
                myMap.Children.Remove(pushpin);
            }

            myMap.Children.Add(pin);
        }
    }
}
