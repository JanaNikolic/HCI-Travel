using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelApp.Model;

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for UnregisteredMainView.xaml
    /// </summary>
    public partial class UnregisteredMainView : Window, INotifyPropertyChanged
    {
        public List<Aranzman> SviAranzmani { get; set; }

        private bool _loggedIn {  get; set; }
        public bool loggedIn {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                OnPropertyChanged(nameof(loggedIn));
            }
        }
        public User user { get; set; }

        public static readonly DependencyProperty IsClickedProperty =
        DependencyProperty.RegisterAttached("IsClicked", typeof(bool), typeof(UnregisteredMainView), new PropertyMetadata(false));

        //public bool IsClicked
        //{
        //    get { return (bool)GetValue(IsClickedProperty); }
        //    set { SetValue(IsClickedProperty, value); }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _pretragaTB { get; set; }
        public string PretragaTB
        {
            get { return _pretragaTB; }
            set
            {
                _pretragaTB = value;
                OnPropertyChanged(nameof(PretragaTB));
            }
        }

        public UnregisteredMainView()
        {
            InitializeComponent();

            this.DataContext = this;
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

        private void ReportExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PregledKupovinaView view = new PregledKupovinaView(user);
            view.Show();
        }

        private void CanReportExecute(object sender, CanExecuteRoutedEventArgs e)
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

        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Aranzman aranzman = ((StackPanel)sender).Tag as Aranzman;
            Trace.WriteLine($"Slika>> {aranzman.PictureLocation}");
            if (user == null)
            {
                PojedinacanAranzman w = new PojedinacanAranzman(aranzman);
                w.Show();
            }
            else
            {
                PojedinacanAranzman w = new PojedinacanAranzman(aranzman, user);
                w.Show();
            }
        }

        public UnregisteredMainView(User user)
        {
            this.user = user;
            
            InitializeComponent();
            this.DataContext = this;

            SviAranzmani = new List<Aranzman>();

            using (var dbContext = new MyDbContext())
            {
                SviAranzmani = dbContext.Arrangements.ToList();
                if (SviAranzmani.Count > 0)
                    ListViewAranzmans.ItemsSource = SviAranzmani;
            }
        }

        public void PretragaAranzmana(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Cliknuto dugme");
            Trace.WriteLine(PretragaTB);
            using (var dbContext = new MyDbContext())
            {
                Trace.WriteLine(PretragaTB);
                SviAranzmani = dbContext.Arrangements.Where(a => a.Name.Contains(PretragaTB)).ToList();

                if (SviAranzmani.Count > 0)
                    ListViewAranzmans.ItemsSource = SviAranzmani;

            }
        }
    }

}
