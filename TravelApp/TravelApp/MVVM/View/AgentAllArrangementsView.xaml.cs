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
    /// Interaction logic for AgentAllArrangementsView.xaml
    /// </summary>
    public partial class AgentAllArrangementsView : Window, INotifyPropertyChanged
    {
        public List<Aranzman> SviAranzmani { get; set; }
        public List<Aranzman> OdabraniAranzmani { get; set; }

        public int brOdabranih { get; set; }
        
        public static readonly DependencyProperty IsClickedProperty =
        DependencyProperty.RegisterAttached("IsClicked", typeof(bool), typeof(AgentAllArrangementsView), new PropertyMetadata(false));

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


        public AgentAllArrangementsView()
        {
            InitializeComponent();
            this.DataContext = this;

            SviAranzmani = new List<Aranzman>();
            OdabraniAranzmani = new List<Aranzman>();
            brOdabranih = 0;
            IsClicked = false;


            using (var dbContext = new MyDbContext())
            {
                Aranzman a = new Aranzman("Manastiri Fruske Gore", "Opis putovanja...", DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 2000.00, "");
                dbContext.Arrangements.Add(a);
                ////dbContext.Arrangements.Add(a);
                ////dbContext.Arrangements.Add(a);
                ////dbContext.Arrangements.Add(a);
                ////dbContext.Arrangements.Add(a);
                dbContext.SaveChanges();

                SviAranzmani = dbContext.Arrangements.ToList();
                if (SviAranzmani.Count > 0)
                    ListViewAranzmans.ItemsSource = SviAranzmani;
            }

        }

        public void DodajAranzman(object sender, RoutedEventArgs e) 
        {
            Aranzman aranzman = ((CheckBox)sender).Tag as Aranzman;

            Trace.WriteLine(aranzman.ToString());
            
            if (!OdabraniAranzmani.Contains(aranzman))
            { 
                OdabraniAranzmani.Add(aranzman);
            }

            brOdabranih = OdabraniAranzmani.Count();
            Trace.WriteLine(brOdabranih);

        }

        public void PretragaAranzmana(object sender, RoutedEventArgs e)
        { 
            
        }

        public void BrisanjeAranzmana(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                foreach (var aranzman in OdabraniAranzmani)
                {
                    dbContext.Entry(aranzman).State = System.Data.Entity.EntityState.Deleted;
                }
                dbContext.SaveChanges();
                SviAranzmani = dbContext.Arrangements.ToList();
                ListViewAranzmans.ItemsSource = SviAranzmani;

            }

        }

        public void UkloniAranzman(object sender, RoutedEventArgs e)
        {
            Aranzman aranzman = ((CheckBox)sender).Tag as Aranzman;

            if (OdabraniAranzmani.Contains(aranzman))
            {
                OdabraniAranzmani.Remove(aranzman);
            }

            brOdabranih = OdabraniAranzmani.Count();
        }


    }
}
