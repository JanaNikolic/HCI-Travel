﻿using System;
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
    /// Interaction logic for FormaRestoran.xaml
    /// </summary>
    public partial class FormaRestoran : Window, INotifyPropertyChanged, IDataErrorInfo
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


        private bool edit { get; set; }

        private Restoran editRestoran { get; set; }

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
                if (columnName == "Adresa" && string.IsNullOrEmpty(Adresa))
                {
                    HasNoErrors = false;
                    AdresaError = "Polje za adresu ne sme biti prazno";
                    return "Polje za adresu ne sme biti prazno";
                }
                if (columnName == "Adresa" && !string.IsNullOrEmpty(Adresa))
                {
                    AdresaError = "";
                }
                if (!string.IsNullOrEmpty(Naziv) && !string.IsNullOrEmpty(Adresa))
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
        public FormaRestoran()
        {
            InitializeComponent();
            DataContext = this;
            HasNoErrors = false; 
            CommandManager.RegisterClassCommandBinding(typeof(FormaRestoran), new CommandBinding(CustomCommands.Save, SaveExecuted, CanSaveExecute));
            CommandManager.RegisterClassCommandBinding(typeof(FormaRestoran), new CommandBinding(CustomCommands.Close, CloseExecuted, CanCloseExecute));
        }

        public FormaRestoran(Restoran restoran, int brOdabranih, int indeks)
        {
            edit = true;
            editRestoran = restoran;
            this.indeks = indeks;
            this.brOdabranih = brOdabranih;


            InitializeComponent();
            this.Title = "Izmeni restoran";

            var elem = this.FindName("ListForEdit") as StackPanel;
            elem.Visibility = Visibility.Visible;

            DataContext = this;
            HasNoErrors = false;

            Naziv = restoran.Name;
            Adresa = restoran.Address;
            VrstaHrane = restoran.FoodType;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (edit)
                {
                    using (var dbContext = new MyDbContext())
                    {
                        var restoran = dbContext.Restaurants.SingleOrDefault(a => a.Id == editRestoran.Id);

                        if (restoran != null)
                        {
                            restoran.Name = Naziv;
                            restoran.Address = Adresa;
                            restoran.FoodType = VrstaHrane;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            string messageBoxText = "Greška prilikom izmene restorana";
                            string caption = "Izmena restorana";
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
                    Restoran restoran = new Restoran(Naziv, Adresa, VrstaHrane);
                    using (var dbContext = new MyDbContext())
                    {
                        dbContext.Restaurants.Add(restoran);
                        dbContext.SaveChanges();
                    }

                    string messageBoxText = "Nov restoran je uspešno sačuvan! Da li zelite da kreirate još?";
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
            string caption = "Odustajanje od novog restorana";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = Keyboard.FocusedElement;
            Trace.WriteLine(focusedControl.GetType().ToString());
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                Trace.WriteLine("radi");
                HelpProvider.ShowHelp(str, this);
            } else
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
                Button_Click_Submit(null, null);
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
    }
}
