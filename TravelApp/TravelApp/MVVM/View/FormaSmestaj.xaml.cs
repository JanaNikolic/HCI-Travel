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

        private string _link;
        public string Link
        {
            get { return _link;  }
            set
            {
                _link = value;
                OnPropertyChanged(nameof(Link));
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

        private string _linkError { get; set; }
        public string LinkError
        {
            get { return _linkError; }
            set
            {
                _linkError = value;
                OnPropertyChanged(nameof(LinkError));
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

        private bool edit { get; set; }

        private Smestaj editSmestaj { get; set; }

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


        public string this[string columnName]
        {
            get
            {
                if (columnName == "NazivSmestaja" && string.IsNullOrEmpty(NazivSmestaja))
                {
                    HasNoErrors = false;
                    NazivError = "Polje za naziv ne sme biti prazno";
                    return "Polje za naziv ne sme biti prazno";
                }
                if (columnName == "NazivSmestaja" && !string.IsNullOrEmpty(NazivSmestaja))
                {
                    NazivError = "";
                }
                if (columnName == "AdresaSmestaja" && string.IsNullOrEmpty(AdresaSmestaja))
                {
                    HasNoErrors = false;
                    AdresaError = "Polje za opis ne sme biti prazno";
                    return "Polje za opis ne sme biti prazno";
                }
                if (columnName == "AdresaSmestaja" && !string.IsNullOrEmpty(AdresaSmestaja))
                {
                    AdresaError = "";
                }
                if(columnName == "Link" && string.IsNullOrEmpty(Link))
                {
                    HasNoErrors = false;
                    LinkError = "Polje za link ne sme biti prazno";
                    return "Polje za link ne sme biti prazno";
                }
                if (columnName == "Link" && !string.IsNullOrEmpty(Link))
                {
                    LinkError = "";
                }
                if (!string.IsNullOrEmpty(NazivSmestaja) && !string.IsNullOrEmpty(AdresaSmestaja) && !string.IsNullOrEmpty(Link))
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

        public FormaSmestaj(Smestaj smestaj, int brOdabranih, int indeks)
        {
            edit = true;
            editSmestaj = smestaj;
            this.indeks = indeks;
            this.brOdabranih = brOdabranih;


            InitializeComponent();
            this.Title = "Izmeni smeštaj";

            var elem = this.FindName("ListForEdit") as StackPanel;
            elem.Visibility = Visibility.Visible;

            DataContext = this;
            HasNoErrors = false;

            NazivSmestaja = smestaj.Name;

            AdresaSmestaja = smestaj.Address;
            Rating = smestaj.Stars;
            Link = smestaj.Link;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (edit)
                {
                    using (var dbContext = new MyDbContext())
                    {
                        var smestaj = dbContext.Hotels.SingleOrDefault(a => a.Id == editSmestaj.Id);

                        if (smestaj != null)
                        {
                            smestaj.Name = NazivSmestaja;
                            smestaj.Address = AdresaSmestaja;
                            smestaj.Stars = Rating;
                            smestaj.Link = Link;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            string messageBoxText = "Greška prilikom izmene smeštaja";
                            string caption = "Izmena smeštaja";
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
                    Smestaj smestaj = new Smestaj(NazivSmestaja, AdresaSmestaja, Rating, Link);
                    using (var dbContext = new MyDbContext())
                    {
                        dbContext.Hotels.Add(smestaj);
                        dbContext.SaveChanges();
                    }

                    string messageBoxText = "Nov smeštaj je uspešno sačuvan! Da li želite da nastavite?";
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

            }
            catch (Exception ex)
            {
                string messageBoxText = "Došlo je do greške prilikom sačuvanja smeštaja. Pokušajte ponovo.";
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
            string caption = "Odustajanje od čuvanja smeštaja";
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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = Keyboard.FocusedElement;
            Trace.WriteLine(focusedControl.GetType().ToString());
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                if (str == "Restoran")
                {
                    str = "Smestaj";
                }
                HelpProvider.ShowHelp(str, this);
            }
        }
    }
}
