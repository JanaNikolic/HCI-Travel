
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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window, INotifyPropertyChanged
    {
        private string _userName { get; set; }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public User User { get; set; }
        private string _password { get; set; }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _userNameError { get; set; }
        public string UserNameError
        {
            get { return _userNameError; }
            set
            {
                _userNameError = value;
                OnPropertyChanged(nameof(UserNameError));
            }
        }

        private string _passwordError { get; set; }
        public string PasswordError
        {
            get { return _passwordError; }
            set
            {
                _passwordError = value;
                OnPropertyChanged(nameof(PasswordError));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginView()
        {
            //using (var db = new MyDbContext())
            //{
            //    User user = new User("j", "jana", Role.Agent);
            //    db.Users.Add(user);
            //    db.SaveChanges();
            //}

            //using (var dbContext = new MyDbContext())
            //{
            //    User user = new User("jana", "jana", Role.Client);
            //    dbContext.Users.Add(user);

            //    Restoran a = new Restoran("Loft", "Novosadskog Sajma 105 Novi Sad", FoodType.Italijanska);
            //    dbContext.Restaurants.Add(a);

            //    Smestaj s = new Smestaj("Smestaj", "Jevrejska 15", 3.2, "http://www.google.com");
            //    dbContext.Hotels.Add(s);

            //    Atrakcija at1 = new Atrakcija("Manastiri Fruske Gore", "Opis putovanja...", "Fruska Gora");
            //    dbContext.Attractions.Add(at1);

            //    Atrakcija at = new Atrakcija("Vrnjačka  Banja", "Opis putovanja...", "Vrnjačka  Banja");
            //    dbContext.Attractions.Add(at);

            //    Aranzman ar = new Aranzman("Manastiri Fruske Gore", "Opis putovanja...", DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 2000.00, "..\\..\\Images\\login.jpg");
            //    ar.Restorani.Add(a);
            //    ar.Atrakcije.Add(at1);
            //    ar.Smestaji.Add(s);
            //    dbContext.Arrangements.Add(ar);

            //    ar = new Aranzman("Vrnjačka  Banja", "Opis putovanja...", DateTime.Now.AddDays(10), DateTime.Now.AddDays(15), "Beograd", "Vrnjačka Banja", 12000.00, "..\\..\\Images\\login.jpg");
            //    ar.Restorani.Add(a);
            //    ar.Atrakcije.Add(at);
            //    ar.Smestaji.Add(s);
            //    dbContext.Arrangements.Add(ar);

            //    dbContext.SaveChanges();
            //}


            InitializeComponent();
            DataContext = this;
            Password = "";
            //inhabitDatabase();
            CommandManager.RegisterClassCommandBinding(typeof(LoginView), new CommandBinding(CustomCommands.Register, RegisterExecuted, CanRegisterExecute));
            CommandManager.RegisterClassCommandBinding(typeof(LoginView), new CommandBinding(CustomCommands.RegisterWindow, RegisterWindowExecuted, CanRegisterWindowExecute));
            CommandManager.RegisterClassCommandBinding(typeof(LoginView), new CommandBinding(CustomCommands.UnregisteredWindow, UnregisteredWindowExecuted, CanUnregisteredWindowExecute));
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new MyDbContext())
                {
                    var users = db.Users.Where(u => u.UserName == UserName && u.Password == Password).ToList();

                    if (users!= null)
                    {
                        if (users[0].Role == Role.Agent)
                        {
                            AgentAllArrangementsView newWindow = new AgentAllArrangementsView();
                            newWindow.Show();
                            Close();
                        } else
                        {
                            UnregisteredMainView view = new UnregisteredMainView(users[0]);
                            view.loggedIn = true;
                            view.Show();
                            Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = "Doslo je do greske prilikom prijave.";
                string caption = "Upozorenje!";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
            }

        }

        private void RegisterExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            btnLogin_Click(null, null);
        }

        private void CanRegisterExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void RegisterWindowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RegisterView view = new RegisterView();
            view.Show();
            this.Close();
        }

        private void CanRegisterWindowExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void UnregisteredWindowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UnregisteredMainView view = new UnregisteredMainView();
            view.Show();
            this.Close();
        }

        private void CanUnregisteredWindowExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void OnlineHelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            HelpProvider.ShowHelp("Travel", this);
        }

        private void CanOnlineHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { Password = ((PasswordBox)sender).Password; }
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
                    str = "Toolbar";
                }
                HelpProvider.ShowHelp(str, this);
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

                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", 2, "https://stackoverflow.com"));
                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", 3, "https://stackoverflow.com"));
                dbContext.Hotels.Add(new Smestaj("Naziv1", "adresa", 4, "https://stackoverflow.com"));

                dbContext.Users.Add(new User("agent", "password", Role.Agent));
                dbContext.SaveChanges();
            }
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            var passwordBox1 = this.FindName("txtPass") as PasswordBox;
            var textBox1 = this.FindName("passwordTxtBox") as TextBox;
            if (revealModeCheckBox.IsChecked == true)
            {
                textBox1.Text = passwordBox1.Password;
                passwordBox1.Visibility = Visibility.Collapsed;
                textBox1.Visibility = Visibility.Visible;
            }
            else
            {
                passwordBox1.Password = textBox1.Text;
                passwordBox1.Visibility = Visibility.Visible;
                textBox1.Visibility = Visibility.Collapsed;
            }
        }
    }
}
