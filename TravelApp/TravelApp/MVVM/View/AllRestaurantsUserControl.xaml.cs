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
    /// Interaction logic for AllRestaurantsUserControl.xaml
    /// </summary>
    public partial class AllRestaurantsUserControl : UserControl, INotifyPropertyChanged
    {
        public List<Restoran> SviRestorani { get; set; }
        public List<Restoran> OdabraniRestorani { get; set; }

        private int brOdabranih { get; set; }

        public static readonly DependencyProperty IsClickedProperty =
        DependencyProperty.RegisterAttached("IsClicked", typeof(bool), typeof(AllRestaurantsUserControl), new PropertyMetadata(false));

        public bool IsClicked
        {
            get { return (bool)GetValue(IsClickedProperty); }
            set { SetValue(IsClickedProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int BrOdabranih
        {
            get { return brOdabranih; }
            set
            {
                brOdabranih = value;
                OnPropertyChanged(nameof(BrOdabranih));
            }
        }
        public AllRestaurantsUserControl()
        {
            InitializeComponent();

            this.DataContext = this;

            SviRestorani = new List<Restoran>();
            OdabraniRestorani = new List<Restoran>();
            BrOdabranih = 0;
            IsClicked = false;


            LoadRestorani();
        }

        private void LoadRestorani()
        {
            using (var dbContext = new MyDbContext())
            {
                //Restoran a = new Restoran("Loft", "Adresa", FoodType.Italijanska);
                //dbContext.Restaurants.Add(a);
                //dbContext.SaveChanges();

                SviRestorani = dbContext.Restaurants.ToList();
                if (SviRestorani.Count > 0)
                    ListViewRestorans.ItemsSource = SviRestorani;

                OdabraniRestorani = new List<Restoran>();
                BrOdabranih = OdabraniRestorani.Count();
            }
        }

        public void DodajRestoran(object sender, RoutedEventArgs e)
        {
            Restoran restoran = ((CheckBox)sender).Tag as Restoran;

            Trace.WriteLine(restoran.ToString());

            if (!OdabraniRestorani.Contains(restoran))
            {
                OdabraniRestorani.Add(restoran);
            }

            BrOdabranih = OdabraniRestorani.Count();
            Trace.WriteLine(BrOdabranih);

        }

        public void PretragaRestorana(object sender, RoutedEventArgs e)
        {

        }

        public void BrisanjeRestorana(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Da li želite da izbrišete izabrane restorane?";
            string caption = "Brisanje restorana";

            MessageBoxButton button = MessageBoxButton.YesNo;

            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                using (var dbContext = new MyDbContext())
                {
                    foreach (var restoran in OdabraniRestorani)
                    {
                        dbContext.Entry(restoran).State = System.Data.Entity.EntityState.Deleted;
                    }
                    dbContext.SaveChanges();
                    SviRestorani = dbContext.Restaurants.ToList();
                    ListViewRestorans.ItemsSource = SviRestorani;
                    OdabraniRestorani = new List<Restoran>();
                    BrOdabranih = 0;
                }
            }
        }

        public void UkloniRestoran(object sender, RoutedEventArgs e)
        {
            Restoran restoran = ((CheckBox)sender).Tag as Restoran;

            if (OdabraniRestorani.Contains(restoran))
            {
                OdabraniRestorani.Remove(restoran);
            }

            BrOdabranih = OdabraniRestorani.Count();
        }


        public void IzmenaRestorana(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < OdabraniRestorani.Count(); i++)
            {
                var f = new FormaRestoran(OdabraniRestorani[i], OdabraniRestorani.Count(), i + 1);
                f.ShowDialog();
            }
            LoadRestorani();
        }

        public void KreirajRestoran(object sender, RoutedEventArgs e)
        {
            FormaRestoran forma = new FormaRestoran();
            forma.Show();
            //this.Close();
        }
    }
}
