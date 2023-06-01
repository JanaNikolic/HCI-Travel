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
    /// Interaction logic for FormaRestoran.xaml
    /// </summary>
    public partial class FormaRestoran : Window, INotifyPropertyChanged
    {

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

        private FoodType _vrstaHrane { get; set; }
        public FoodType VrstaHrane
        {
            get { return _vrstaHrane; }
            set
            {
                _vrstaHrane = value;
                OnPropertyChanged(nameof(VrstaHrane));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public FormaRestoran()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                Restoran restoran = new Restoran(Naziv, Adresa, VrstaHrane);
                using (var dbContext = new MyDbContext())
                {
                    dbContext.Restaurants.Add(restoran);
                    dbContext.SaveChanges();
                }

                string messageBoxText = "Nov restoran je uspešno sačuvan! Da li zelite da nastavite?";
                string caption = "Čuvanje";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                if (result == MessageBoxResult.Yes)
                {
                    Naziv = "";
                    Adresa = "";
                    VrstaHrane = FoodType.Domaca;
                }

                else if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = "Došlo je do greške prilikom sačuvanja restorana. Pokušajte ponovo.";
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
