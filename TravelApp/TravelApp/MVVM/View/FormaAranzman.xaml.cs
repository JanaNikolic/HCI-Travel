using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using TravelApp.Model;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FormaAranzman : Window
    {
        Point startPoint = new Point();

        public ObservableCollection<Atrakcija> SveAtrakcije { get; set; }

        public ObservableCollection<Atrakcija> IzabraneAtrakcije { get; set; }

        public ObservableCollection<Restoran> SviRestorani { get; set; }

        public ObservableCollection<Restoran> IzabraniRestorani { get; set; }

        public ObservableCollection<Smestaj> SviSmestaji { get; set; }

        public ObservableCollection<Smestaj> IzabraniSmestaji { get; set; }

        public Double Cena { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }

        public string MestoPolaska { get; set; }

        public string Destinacija { get; set; }

        public DateTime DatumPolaska { get; set; } = DateTime.Now;

        public DateTime DatumPovratka { get; set; } = DateTime.Now;

        public FormaAranzman()
        {
            InitializeComponent();
            this.DataContext = this;

            SveAtrakcije = new ObservableCollection<Atrakcija>();
            SviSmestaji = new ObservableCollection<Smestaj>();
            SviRestorani = new ObservableCollection<Restoran>();
            IzabraneAtrakcije = new ObservableCollection<Atrakcija>();
            IzabraniSmestaji = new ObservableCollection<Smestaj>();
            IzabraniRestorani = new ObservableCollection<Restoran>();

            using (var dbContext = new MyDbContext())
            {
                var listaAtrakcija = dbContext.Attractions.ToList();
                foreach(var atrakcija in listaAtrakcija)
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

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }



        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Perform further processing with the selected picture
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

    }

}
