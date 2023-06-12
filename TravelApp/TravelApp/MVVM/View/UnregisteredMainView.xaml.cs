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
