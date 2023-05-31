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
    public partial class FormaAranzman : Window, INotifyPropertyChanged
    {
        Point startPoint = new Point();

        public ObservableCollection<Atrakcija> SveAtrakcije { get; set; }

        public ObservableCollection<Atrakcija> IzabraneAtrakcije { get; set; }

        public ObservableCollection<Restoran> SviRestorani { get; set; }

        public ObservableCollection<Restoran> IzabraniRestorani { get; set; }

        public ObservableCollection<Smestaj> SviSmestaji { get; set; }

        public ObservableCollection<Smestaj> IzabraniSmestaji { get; set; }

        public Double Cena { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Opis { get; set; }

        public string MestoPolaska { get; set; }

        public string Destinacija { get; set; }

        public DateTime DatumPolaska { get; set; } = DateTime.Now;

        public DateTime DatumPovratka { get; set; } = DateTime.Now;

        public string Slika { get; set; }

        public FormaAranzman()
        {
            InitializeComponent();
            DataContext = this;

            SveAtrakcije = new ObservableCollection<Atrakcija>();
            SviSmestaji = new ObservableCollection<Smestaj>();
            SviRestorani = new ObservableCollection<Restoran>();
            IzabraneAtrakcije = new ObservableCollection<Atrakcija>();
            IzabraniSmestaji = new ObservableCollection<Smestaj>();
            IzabraniRestorani = new ObservableCollection<Restoran>();

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

                string messageBoxText = "Do you want to save changes?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                
                if(result == MessageBoxResult.Yes)
                {
                    Naziv="";
                    this.Opis = "";
                    this.MestoPolaska = "";
                    this.Destinacija = "";
                    this.Cena = 0;
                    this.Slika = "";
                    IzabraniRestorani.Clear();
                    IzabraneAtrakcije.Clear();
                    IzabraniSmestaji.Clear();
                    this.DatumPolaska = DateTime.Now;
                    this.DatumPovratka = DateTime.Now;

                    loadLists();
                }
            } catch (Exception ex) { }
        }


        public void loadLists()
        {
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

                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", Stars.three));
                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", Stars.three));
                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", Stars.four));
                dbContext.SaveChanges();
            }
        }
    }

}
