using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TravelApp.Model;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for UnregisteredMainView.xaml
    /// </summary>
    public partial class UnregisteredMainView : Window
    {
        public List<Aranzman> SviAranzmani { get; set; }

        public bool loggedIn { get; set; }
        public UnregisteredMainView()
        {
            InitializeComponent();

            SviAranzmani = new List<Aranzman>();

            using (var dbContext = new MyDbContext())
            {
                SviAranzmani = dbContext.Arrangements.ToList();
                if (SviAranzmani.Count > 0)
                    ListViewAranzmans.ItemsSource = SviAranzmani;
            }

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

        private void OnlineHelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            HelpProvider.ShowHelp("Travel", this);
        }

        private void CanOnlineHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            LoginView view = new LoginView();
            view.Show();
            this.Close();
        }

        private void CanCloseExecute(object sender, CanExecuteRoutedEventArgs e)
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
