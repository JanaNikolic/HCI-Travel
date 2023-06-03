using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.Xml.Linq;
using TravelApp.Model;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FormaAranzman : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        Point startPoint = new Point();

        public ObservableCollection<Atrakcija> SveAtrakcije { get; set; }

        public ObservableCollection<Atrakcija> IzabraneAtrakcije { get; set; }

        public ObservableCollection<Restoran> SviRestorani { get; set; }

        public ObservableCollection<Restoran> IzabraniRestorani { get; set; }

        public ObservableCollection<Smestaj> SviSmestaji { get; set; }

        public ObservableCollection<Smestaj> IzabraniSmestaji { get; set; }

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
                    NazivError = "Mora da postoji naziv.";
                    return "Mora da postoji naziv.";
                } else
                {
                    NazivError = "";
                }
                if (columnName == "Opis" && string.IsNullOrEmpty(Opis))
                {
                    HasNoErrors = false;
                    OpisError = "Mora da postoji opis.";
                    return "Mora da postoji opis";
                } else
                {
                    OpisError = "";
                }
                if (columnName == "MestoPolaska" && string.IsNullOrEmpty(MestoPolaska))
                {
                    HasNoErrors = false;
                    return "Mora da postoji Mesto Polaska.";
                } 
                if (columnName == "Destinacija" && string.IsNullOrEmpty(Destinacija))
                {
                    HasNoErrors = false;
                    return "Mora da postoji destinacija.";
                }
                if (columnName == "Cena" && double.IsNegative(Cena) || Cena == default(double))
                {
                    HasNoErrors = false;
                    CenaError = "Cena mora da bude pozitivan broj.";
                    return "Cena mora da bude pozitivan broj.";
                } else
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
                if (columnName == "DatumPolaska" && DatumPovratka < DatumPolaska)
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

                // Find the data behind the ListViewItem
                Atrakcija atrakcija = (Atrakcija)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", atrakcija);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
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

                // Find the data behind the ListViewItem
                Restoran restoran = (Restoran)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", restoran);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
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

                // Find the data behind the ListViewItem
                Smestaj smestaj = (Smestaj)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", smestaj);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
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
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Aranzman aranzman = new Aranzman(Naziv, Opis, DatumPolaska, DatumPovratka, MestoPolaska, Destinacija, Cena, Slika, IzabraniRestorani.ToList(), IzabraneAtrakcije.ToList(), IzabraniSmestaji.ToList());
                using (var dbContext = new MyDbContext())
                {
                    dbContext.Arrangements.Add(aranzman);
                    dbContext.SaveChanges();
                }

                string messageBoxText = "Nov aranžman je uspešno sačuvan. Da li zelite da nastavite?";
                string caption = "Čuvanje";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                
                if(result == MessageBoxResult.Yes)
                {
                    Naziv="";
                    this.Opis = "";
                    this.MestoPolaska = "";
                    this.Destinacija = "";
                    this.Cena = 0;
                    Slika = "";
                    SelectedImage.Source = new BitmapImage(new Uri("C:\\fax\\hci\\HCI-Travel\\TravelApp\\TravelApp\\Images\\placeholder-image.png"));
                    IzabraniRestorani.Clear();
                    IzabraneAtrakcije.Clear();
                    IzabraniSmestaji.Clear();
                    this.DatumPolaska = DateTime.Now;
                    this.DatumPovratka = DateTime.Now;

                    loadLists();
                }
            } catch (Exception ex) {
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

        public void inhabitDatabase()
        {
            using (var dbContext = new MyDbContext())
            {
                dbContext.Attractions.Add(new Atrakcija("Naziv1", "Opis", "adresa"));
                dbContext.Attractions.Add(new Atrakcija("Naziv2", "Opis", "adresa"));
                dbContext.Attractions.Add(new Atrakcija("Naziv3", "Opis", "adresa"));
                dbContext.Attractions.Add(new Atrakcija("Naziv4", "Opis", "adresa"));
                dbContext.Attractions.Add(new Atrakcija("Naziv5", "Opis", "adresa"));
                dbContext.Attractions.Add(new Atrakcija("Naziv6", "Opis", "adresa"));

                dbContext.Restaurants.Add(new Restoran("Naziv1", "adresa", FoodType.Meksicka));
                dbContext.Restaurants.Add(new Restoran("Naziv2", "adresa", FoodType.Italijanska));
                dbContext.Restaurants.Add(new Restoran("Naziv3", "adresa", FoodType.Domaca));
                dbContext.Restaurants.Add(new Restoran("Naziv4", "adresa", FoodType.Meksicka));

                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", 2));
                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", 3));
                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", 4));
                dbContext.SaveChanges();
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
            string caption = "Odustajanje od novog aranžmana";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }

}
