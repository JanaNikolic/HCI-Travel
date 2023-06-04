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
            InitializeComponent();
            DataContext = this;
            Password = "";
            CommandManager.RegisterClassCommandBinding(typeof(RegisterView), new CommandBinding(CustomCommands.Register, RegisterExecuted, CanRegisterExecute));
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
                        string messageBoxText = users[0].UserName;
                        string caption = "Dobrodosli!";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Information;
                        MessageBoxResult result;

                        result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = "Doslo je do greske prilikom registracije.";
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { Password = ((PasswordBox)sender).Password; }
        }
    }
}
