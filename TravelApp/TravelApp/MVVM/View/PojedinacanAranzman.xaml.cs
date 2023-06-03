using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public PojedinacanAranzman()
        {
            InitializeComponent();
            DataContext = this;
            List<Restoran> restorani = new List<Restoran>();
            List<Atrakcija> atrakcije = new List<Atrakcija>();
            List<Smestaj> smestaji = new List<Smestaj>();
            using (var dbContext = new MyDbContext())
            {
                Restoran rest =  dbContext.Restaurants.Find(1);
                restorani.Add(rest);
                Atrakcija attr = dbContext.Attractions.Find(1);
                Atrakcija attr2 = dbContext.Attractions.Find(2);
                atrakcije.Add(attr);
                atrakcije.Add(attr2);
                Smestaj smestaj = dbContext.Hotels.Find(1);
                smestaji.Add(smestaj);
            }
            Aranzman = new Aranzman("Putovanje Srbijom", "JEdno lepo putovanje glavnim gradovima Srbije pooput Novog Sada i Beogradda, s posetama manastirima Hopovo i jos neki cisto da popunim opis da ima dovoljno reci kako bi u prikazu lepse izgledalo tokom testiranja.",
                DateTime.Now, DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 4500, "null", restorani, atrakcije, smestaji);

            SelectedImage.Source = new BitmapImage(new Uri("C:\\Users\\davor\\OneDrive\\Desktop\\P5-APR-iStock-1359675618.jpg"));
        }
    }
}
