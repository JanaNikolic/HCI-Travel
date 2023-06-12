using Syncfusion.Windows.Shared.Resources;
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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        private string _userName {  get; set; }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

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
                if (columnName == "UserName" && string.IsNullOrEmpty(UserName))
                {
                    HasNoErrors = false;
                    UserNameError = "Mora da postoji korisnicko ime.";
                    return "Mora da postoji korisnicko ime.";
                }
                if (columnName == "UserName" && !string.IsNullOrEmpty(UserName))
                {
                    UserNameError = "";
                }
                if (columnName == "Password" && (string.IsNullOrEmpty(Password) || Password.Length < 8))
                {
                    HasNoErrors = false;
                    PasswordError = "Lozinka mora imati barem 8 znakova.";
                    return "Lozinka mora imati barem 8 znakova.";
                }
                if (columnName == "Password" && !string.IsNullOrEmpty(Password) && Password.Length >= 8)
                {
                    PasswordError = "";
                }
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && Password.Length >= 8)
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

        public RegisterView()
        {
            InitializeComponent();
            DataContext = this;
            HasNoErrors = false;
            Password = "";
            CommandManager.RegisterClassCommandBinding(typeof(RegisterView), new CommandBinding(CustomCommands.Register, RegisterExecuted, CanRegisterExecute));
            CommandManager.RegisterClassCommandBinding(typeof(RegisterView), new CommandBinding(CustomCommands.LoginWindow, LoginWindowExecuted, CanLoginWindowExecute));
            CommandManager.RegisterClassCommandBinding(typeof(RegisterView), new CommandBinding(CustomCommands.UnregisteredWindow, UnregisteredWindowExecuted, CanUnregisteredWindowExecute));
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { Password = ((PasswordBox)sender).Password; }
            ValidatePassword();

        }

        private void ValidatePassword()
        {
            // Perform the validation for the Password property
            string passwordValidationResult = this["Password"];
            PasswordError = passwordValidationResult; // Update the PasswordError property with the validation result

            // Check if there are no errors
            if (string.IsNullOrEmpty(Password) || Password.Length < 8)
            {
                HasNoErrors = false;
            }
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && Password.Length >= 8)
            {
                HasNoErrors = true;
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new MyDbContext())
                {
                    User user = new User(UserName, Password, Role.Client);
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                string messageBoxText = "Uspesno ste se registrovali.";
                string caption = "Dobrodosli!";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                if (result == MessageBoxResult.OK)
                {
                    // Open a new window
                    LoginView newWindow = new LoginView();
                    newWindow.Show();
                    Close();

                }
            } catch (Exception ex)
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

        private void LoginWindowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            LoginView view = new LoginView();
            view.Show();
            this.Close();
        }

        private void CanLoginWindowExecute(object sender, CanExecuteRoutedEventArgs e)
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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = Keyboard.FocusedElement;
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
    }
}
