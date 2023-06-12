using BingMapsRESTToolkit;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
using Point = System.Windows.Point;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FormaAranzman : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        private string KEY = "";
        MapPolyline currentRoute = null;
        Point startPoint = new Point();
        Dictionary<string, Pushpin> pinMap = new Dictionary<string, Pushpin>();
        public ObservableCollection<Atrakcija> SveAtrakcije { get; set; }

        public ObservableCollection<Atrakcija> IzabraneAtrakcije { get; set; }

        public ObservableCollection<Restoran> SviRestorani { get; set; }

        public ObservableCollection<Restoran> IzabraniRestorani { get; set; }

        public ObservableCollection<Smestaj> SviSmestaji { get; set; }

        public ObservableCollection<Smestaj> IzabraniSmestaji { get; set; }

        private bool edit { get; set; }

        private Aranzman editAranzman { get; set; }

        private int _indeks { get; set; }
        public int indeks {
            get { return _indeks; }
            set
            {
                _indeks = value;
                OnPropertyChanged(nameof(indeks));
            }
        }

        private int _brOdabranih { get; set; }
        public int brOdabranih {
            get { return _brOdabranih; }
            set
            {
                _brOdabranih = value;
                OnPropertyChanged(nameof(brOdabranih));
            }
        }        

        private double _cena { get; set; }
        public double Cena {
            get { return _cena; }
            set
            {
                _cena = value;
                OnPropertyChanged(nameof(Cena));
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
        public string Opis {
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

        private string _cenaError { get; set; }
        public string CenaError
        {
            get { return _cenaError; }
            set
            {
                _cenaError = value;
                OnPropertyChanged(nameof(CenaError));
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
        private string _mestoPolaska { get; set; }
        public string MestoPolaska { 
            get { return _mestoPolaska; }
            set
            {
                _mestoPolaska = value;
                OnPropertyChanged(nameof(MestoPolaska));
            }
        }

        private string _destinacija { get; set; }
        public string Destinacija
        {
            get { return _destinacija; }
            set
            {
                _destinacija = value;
                OnPropertyChanged(nameof(Destinacija));
            }
        }

        public DateTime DatumPolaska { get; set; } = DateTime.Now;

        public DateTime DatumPovratka { get; set; } = DateTime.Now;

        public string Slika { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    NazivError = "Polje za naziv ne sme biti prazno";
                    return "Polje za naziv ne sme biti prazno";
                }
                if (columnName == "Naziv" && !string.IsNullOrEmpty(Naziv))
                {
                    NazivError = "";
                }
                if (columnName == "Opis" && string.IsNullOrEmpty(Opis))
                {
                    HasNoErrors = false;
                    OpisError = "Polje za opis ne sme biti prazno";
                    return "Polje za opis ne sme biti prazno";
                }
                if (columnName == "Opis" && !string.IsNullOrEmpty(Opis))
                {
                    OpisError = "";
                }
                if (columnName == "MestoPolaska" && string.IsNullOrEmpty(MestoPolaska))
                {
                    HasNoErrors = false;
                    return "Obavezno polje";
                }
                if (columnName == "Destinacija" && string.IsNullOrEmpty(Destinacija))
                {
                    HasNoErrors = false;
                    return "Obavezno polje";
                } 
                if (columnName == "Cena" && double.IsNegative(Cena))
                {
                    HasNoErrors = false;
                    CenaError = "Cena mora da bude pozitivan broj.";
                    return "Cena mora da bude pozitivan broj.";
                }
                if (columnName == "Cena" && !double.IsNegative(Cena))
                {
                    CenaError = "";
                }
                if (columnName == "DatumPolaska" && DatumPolaska < DateTime.Now)
                {
                    HasNoErrors = false;
                    return "Datum polaska ne sme biti u proslosti.";
                }
                if (columnName == "Datumpovratka" && DatumPovratka < DateTime.Now)
                {
                    HasNoErrors = false;
                    return "Datum povratka ne sme biti u proslosti.";
                }
                if (columnName == "DatumPovratka" && DatumPovratka < DatumPolaska)
                {
                    HasNoErrors = false;
                    return "Datum polaska ne sme biti posle datuma povratka.";
                }
                if (!string.IsNullOrEmpty(Naziv) && !string.IsNullOrEmpty(Opis) && Cena > 0
                    && !string.IsNullOrEmpty(MestoPolaska) && !string.IsNullOrEmpty(Destinacija) && DatumPovratka > DatumPolaska)
                {
                    HasNoErrors = true;
                }
                return null;
            }
        }

        public string Error
        {
            get { return null; }
        }

        public FormaAranzman()
        {
            edit = false;
            InitializeComponent();
            DataContext = this;
            HasNoErrors = false;
            SveAtrakcije = new ObservableCollection<Atrakcija>();
            SviSmestaji = new ObservableCollection<Smestaj>();
            SviRestorani = new ObservableCollection<Restoran>();
            IzabraneAtrakcije = new ObservableCollection<Atrakcija>();
            IzabraniSmestaji = new ObservableCollection<Smestaj>();
            IzabraniRestorani = new ObservableCollection<Restoran>();
            HasNoErrors = false;
            loadLists();

            CommandManager.RegisterClassCommandBinding(typeof(FormaAranzman), new CommandBinding(CustomCommands.Save, SaveExecuted, CanSaveExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FormaAranzman), new CommandBinding(CustomCommands.Close, CloseExecuted, CanCloseExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FormaAranzman), new CommandBinding(CustomCommands.Browse, BrowseExecuted, CanBrowseExecute));

        }

        public FormaAranzman(Aranzman aranzman, int brOdabranih, int indeks)
        {

            edit = true;
            editAranzman = aranzman;
            this.indeks = indeks;
            this.brOdabranih = brOdabranih;

            InitializeComponent();
            CommandManager.RegisterClassCommandBinding(typeof(FormaAranzman), new CommandBinding(CustomCommands.Save, SaveExecuted, CanSaveExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FormaAranzman), new CommandBinding(CustomCommands.Close, CloseExecuted, CanCloseExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FormaAranzman), new CommandBinding(CustomCommands.Browse, BrowseExecuted, CanBrowseExecute));

            this.Title = "Izmeni aranžman";

            var elem = this.FindName("ListForEdit") as StackPanel;
            elem.Visibility = Visibility.Visible;
            DataContext = this;
            HasNoErrors = false;
            SveAtrakcije = new ObservableCollection<Atrakcija>();
            SviSmestaji = new ObservableCollection<Smestaj>();
            SviRestorani = new ObservableCollection<Restoran>();
            IzabraneAtrakcije = new ObservableCollection<Atrakcija>();
            IzabraniSmestaji = new ObservableCollection<Smestaj>();
            IzabraniRestorani = new ObservableCollection<Restoran>();
            HasNoErrors = false;
            SveAtrakcije.Clear();
            SviRestorani.Clear();
            SviSmestaji.Clear();

            this.Naziv = aranzman.Name;
            this.Opis = aranzman.Description;
            this.MestoPolaska = aranzman.StartLocation;
            this.Destinacija = aranzman.EndLocation;
            this.Cena = aranzman.Price;
            this.Slika = aranzman.PictureLocation;
            if (aranzman.PictureLocation == null || aranzman.PictureLocation == "")
            {
                SelectedImage.Source = new BitmapImage(new Uri("..\\..\\Images\\placeholder-image.png", UriKind.RelativeOrAbsolute));
            }
            else 
            {
                SelectedImage.Source = new BitmapImage(new Uri(aranzman.PictureLocation, UriKind.RelativeOrAbsolute));
            }
            using (var dbContext = new MyDbContext())
            {
                var listaAtrakcija = dbContext.Attractions.ToList();
                foreach (var atrakcija in listaAtrakcija)
                {
                    SveAtrakcije.Add(atrakcija);
                }

                var listaSmestaja = dbContext.Hotels.ToList();
                foreach (var smestaj in listaSmestaja)
                {
                    SviSmestaji.Add(smestaj);
                }

                var listaRestorana = dbContext.Restaurants.ToList();
                foreach (var restoran in listaRestorana)
                {
                    SviRestorani.Add(restoran);
                }
                var Aranzman = dbContext.Arrangements.SingleOrDefault(a => a.Id == aranzman.Id);

                var rs = dbContext.Arrangements.Include(s => s.Atrakcije).ToList();
                var at = dbContext.Arrangements.Include(s => s.Atrakcije).ToList();
                var ss = dbContext.Arrangements.Include(s => s.Smestaji).ToList();
                Trace.WriteLine($"Br Aranz Atrakcija>> {Aranzman.Atrakcije.Count}");

                foreach (var atr in Aranzman.Atrakcije)
                {
                    SveAtrakcije.Remove(atr);
                    IzabraneAtrakcije.Add(atr);
                    AddPin(atr.Name, atr.Address, Colors.Blue);
                }

                foreach (var atr in Aranzman.Restorani)
                {
                    SviRestorani.Remove(atr);
                    IzabraniRestorani.Add(atr);
                    AddPin(atr.Name, atr.Address, Colors.Green);
                }

                foreach (var atr in Aranzman.Smestaji)
                {
                    SviSmestaji.Remove(atr);
                    IzabraniSmestaji.Add(atr);
                    AddPin(atr.Name, atr.Address, Colors.Purple);
                }
            }
            
            this.DatumPolaska = aranzman.StartDate;
            this.DatumPovratka = aranzman.EndDate;  
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

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void Atrakcija_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    // Find the data behind the ListViewItem
                    Atrakcija atrakcija = (Atrakcija)listView.ItemContainerGenerator.
                        ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", atrakcija);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private void Restoran_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    // Find the data behind the ListViewItem
                    Restoran restoran = (Restoran)listView.ItemContainerGenerator.
                        ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", restoran);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private void Smestaj_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    // Find the data behind the ListViewItem
                    Smestaj smestaj = (Smestaj)listView.ItemContainerGenerator.
                        ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", smestaj);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DodajAtrakciju(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Atrakcija atrakcija = e.Data.GetData("myFormat") as Atrakcija;
                if (atrakcija != null)
                {
                    SveAtrakcije.Remove(atrakcija);
                    IzabraneAtrakcije.Add(atrakcija);
                    AddPin(atrakcija.Name, atrakcija.Address, Colors.Blue);
                }
            }
        }

        private void IzbaciAtrakciju(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Atrakcija atrakcija = e.Data.GetData("myFormat") as Atrakcija;
                if (atrakcija != null)
                {
                    IzabraneAtrakcije.Remove(atrakcija);
                    SveAtrakcije.Add(atrakcija);
                    RemovePin(atrakcija.Name);
                }
            }
        }

        private void DodajRestoran(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Restoran restoran = e.Data.GetData("myFormat") as Restoran;
                if (restoran != null)
                {
                    SviRestorani.Remove(restoran);
                    IzabraniRestorani.Add(restoran);
                    AddPin(restoran.Name, restoran.Address, Colors.Green);
                }
            }
        }

        private void IzbaciRestoran(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Restoran restoran = e.Data.GetData("myFormat") as Restoran;
                if (restoran != null)
                {
                    IzabraniRestorani.Remove(restoran);
                    SviRestorani.Add(restoran);
                    RemovePin(restoran.Name);
                }
            }
        }

        private void DodajSmestaj(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Smestaj smestaj = e.Data.GetData("myFormat") as Smestaj;
                if (smestaj != null)
                {
                    SviSmestaji.Remove(smestaj);
                    IzabraniSmestaji.Add(smestaj);
                    AddPin(smestaj.Name, smestaj.Address, Colors.Purple);
                }
            }
        }

        private void IzbaciSmestaj(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Smestaj smestaj = e.Data.GetData("myFormat") as Smestaj;
                if (smestaj != null)
                {
                    IzabraniSmestaji.Remove(smestaj);
                    SviSmestaji.Add(smestaj);
                    RemovePin(smestaj.Name);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (edit)
                {
                    
                    using (var dbContext = new MyDbContext())
                    {
                        //dbContext.Arrangements.Edit(aranzman);
                        var result = dbContext.Arrangements.SingleOrDefault(x => x.Id == editAranzman.Id);

                        if (result != null)
                        {
                            result.Name = Naziv;
                            result.Description = Opis;
                            result.StartDate = DatumPolaska;
                            result.EndDate = DatumPovratka;
                            result.StartLocation = MestoPolaska;
                            result.EndLocation = Destinacija;
                            result.Price = Cena;
                            result.PictureLocation = Slika;
                            result.Restorani = IzabraniRestorani.ToList();
                            result.Atrakcije = IzabraneAtrakcije.ToList();
                            result.Smestaji = IzabraniSmestaji.ToList();

                            dbContext.SaveChanges();
                        }
                        else
                        {
                            string messageBoxText = "Greška prilikom izmene aranžmana";
                            string caption = "Izmena aranžmana";
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Information;
                            MessageBoxResult mbResult;

                            mbResult = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

                        }
                        this.Close();
                    }
                }
                else 
                {
                    Aranzman aranzman = new Aranzman(Naziv, Opis, DatumPolaska, DatumPovratka, MestoPolaska, Destinacija, Cena, Slika, IzabraniRestorani.ToList(), IzabraneAtrakcije.ToList(), IzabraniSmestaji.ToList());
                    using (var dbContext = new MyDbContext())
                    {
                        dbContext.Arrangements.Add(aranzman);
                        dbContext.SaveChanges();
                    }

                    string messageBoxText = "Nov aranžman je uspešno sačuvan";
                    string caption = "Čuvanje";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBoxResult result;

                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

                    if (result == MessageBoxResult.OK)
                    {
                        Naziv = "";
                        this.Opis = "";
                        this.MestoPolaska = "";
                        this.Destinacija = "";
                        this.Cena = 0;
                        Slika = "";
                        SelectedImage.Source = new BitmapImage(new Uri("..\\..\\Images\\placeholder-image.png"));
                        IzabraniRestorani.Clear();
                        IzabraneAtrakcije.Clear();
                        IzabraniSmestaji.Clear();
                        this.DatumPolaska = DateTime.Now;
                        this.DatumPovratka = DateTime.Now;

                        loadLists();
                    }
                }
            } catch (Exception ex) {
                Trace.WriteLine($"Greska>> {ex}");
                string messageBoxText = "Došlo je do greške prilikom sačuvanja aranžmana. Pokušajte ponovo.";
                string caption = "Čuvanje";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }


        public void loadLists()
        {
            SveAtrakcije.Clear();
            SviRestorani.Clear();
            SviSmestaji.Clear();
            using (var dbContext = new MyDbContext())
            {
                var listaAtrakcija = dbContext.Attractions.ToList();
                foreach (var atrakcija in listaAtrakcija)
                {
                    SveAtrakcije.Add(atrakcija);
                }

                var listaSmestaja = dbContext.Hotels.ToList();
                foreach (var smestaj in listaSmestaja)
                {
                    SviSmestaji.Add(smestaj);
                }

                var listaRestorana = dbContext.Restaurants.ToList();
                foreach (var restoran in listaRestorana)
                {
                    SviRestorani.Add(restoran);
                }
            }
        }

        public void OpenAtractionForm_Click(object sender, RoutedEventArgs e)
        {
            var w = new FormaAtrakcija();
            w.Closed += Forma_Closed;
            w.ShowDialog();
        }

        public void OpenRestoranForm_Click(object sender, RoutedEventArgs e)
        {
            var w = new FormaRestoran();
            w.Closed += Forma_Closed;
            w.ShowDialog();
        }

        private void Forma_Closed(object sender, EventArgs e)
        {
            loadLists();
        }

        public void OpenHotelForm_Click(object sender, RoutedEventArgs e)
        {
            var w = new FormaSmestaj();
            w.Closed += Forma_Closed;
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string messageBoxText = "Jeste li sigurni da želite odustati? Svi podaci koji nisu sačuvani će se izgubiti.";
            string caption = "Odustajanje od čuanja aranžmana";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
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
            } catch { }
        }

        public void AddStartLocation(object sender, RoutedEventArgs e)
        {
            RemovePin("startLocation");
            if (!string.IsNullOrEmpty(Destinacija))
            {
                AddPin("startLocation", MestoPolaska, Colors.Red);
                DrawRoute(MestoPolaska, Destinacija);
            }
        }

        public void AddDestination(object sender, RoutedEventArgs e)
        {
            RemovePin("destination");
            if (!string.IsNullOrEmpty(MestoPolaska))
            {
                AddPin("destination", Destinacija, Colors.Red);
                DrawRoute(MestoPolaska, Destinacija);
            }
        }

        private async void DrawRoute(string startLocation, string endLocation)
        {
            myMap.Children.Remove(currentRoute);
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
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red),
                    StrokeThickness = 5
                };
                currentRoute = routeLine;
                myMap.Children.Add(routeLine);
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = Keyboard.FocusedElement;
            Trace.WriteLine(focusedControl.GetType().ToString());
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                if (str == "Restoran")
                {
                    str = "Aranzman";
                }
                Trace.WriteLine(str);
                HelpProvider.ShowHelp(str, this);
            }
            else
            {
                string windowKey = HelpProvider.GetHelpKey(this);
                Trace.WriteLine("ne radi");
                HelpProvider.ShowHelp(windowKey, this);
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (HasNoErrors)
            {
                Button_Click(null, null);
            }
        }

        private void CanSaveExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Button_Click_1(null, null);
        }

        private void CanCloseExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void BrowseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BrowseButton_Click(null, null);
        }

        private void CanBrowseExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }
    }
}
