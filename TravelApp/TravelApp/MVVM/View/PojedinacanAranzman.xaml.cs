using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
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
    /// Interaction logic for PojedinacanAranzman.xaml
    /// </summary>
    public partial class PojedinacanAranzman : Window, INotifyPropertyChanged
    {

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
        }
    }
}
