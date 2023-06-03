using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for FormaSmestaj.xaml
    /// </summary>
    public partial class FormaSmestaj : Window, INotifyPropertyChanged, IDataErrorInfo
    {

        private string _adresa { get; set; }
        public string AdresaSmestaja
        {
            get { return _adresa; }
            set
            {
                _adresa = value;
                OnPropertyChanged(nameof(AdresaSmestaja));
            }
        }

        private string _naziv;
        public string NazivSmestaja
        {
            get { return _naziv; }
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(NazivSmestaja));
            }
        }

        private double _rating;

        public double Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(Rating));
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
                if (columnName == "NazivSmestaja" && string.IsNullOrEmpty(NazivSmestaja))
                {
                    HasNoErrors = false;
                    NazivError = "Mora da postoji naziv.";
                    return "Mora da postoji naziv.";
                }
                if (columnName == "NazivSmestaja" && !string.IsNullOrEmpty(NazivSmestaja))
                {
                    NazivError = "";
                    return null;
                }
                if (columnName == "AdresaSmestaja" && string.IsNullOrEmpty(AdresaSmestaja))
                {
                    HasNoErrors = false;
                    AdresaError = "Mora da postoji opis";
                    return "Mora da postoji opis";
                }
                if (columnName == "AdresaSmestaja" && !string.IsNullOrEmpty(AdresaSmestaja))
                {
                    AdresaError = "";
                    return null;
                }
                if (!string.IsNullOrEmpty(NazivSmestaja) && !string.IsNullOrEmpty(AdresaSmestaja))
                {
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
        public FormaSmestaj()
        {
            InitializeComponent();
            DataContext = this;
            HasNoErrors = false;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                Smestaj smestaj = new Smestaj(NazivSmestaja, AdresaSmestaja, Rating);
                using (var dbContext = new MyDbContext())
                {
                    dbContext.Hotels.Add(smestaj);
                    dbContext.SaveChanges();
                }

                string messageBoxText = "Nov smestaj je uspešno sačuvan! Da li zelite da nastavite?";
                string caption = "Čuvanje";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                if (result == MessageBoxResult.Yes)
                {
                    NazivSmestaja = "";
                    AdresaSmestaja = "";
                    Rating = 0;
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

        private void RatingBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            Rating = e.NewValue;
        }
    }
}
