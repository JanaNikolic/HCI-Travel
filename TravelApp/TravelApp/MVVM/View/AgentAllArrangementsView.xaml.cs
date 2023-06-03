﻿using MaterialDesignThemes.Wpf;
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

        private int brOdabranih { get; set; }

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

        public int BrOdabranih
        {
            get { return brOdabranih; }
            set
            {
                brOdabranih = value;
                OnPropertyChanged(nameof(BrOdabranih));
            }
        }

        public AgentAllArrangementsView()
        {
            InitializeComponent();
            this.DataContext = this;

            SviAranzmani = new List<Aranzman>();
            OdabraniAranzmani = new List<Aranzman>();
            BrOdabranih = 0;
            IsClicked = false;


            LoadAranzmani();

        }

        private void LoadAranzmani()
        {
            using (var dbContext = new MyDbContext())
            {
                //Aranzman a = new Aranzman("Manastiri Fruske Gore", "Opis putovanja...", DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 2000.00, "");
                //dbContext.Arrangements.Add(a);
                //a = new Aranzman("Vrnjačka  Banja", "Opis putovanja...", DateTime.Now.AddDays(10), DateTime.Now.AddDays(15), "Beograd", "Vrnjačka Banja", 12000.00, "");
                //dbContext.Arrangements.Add(a);
                //a = new Aranzman("Manastiri Fruske Gore", "Opis putovanja...", DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 2000.00, "");
                //dbContext.Arrangements.Add(a);
                //a = new Aranzman("Vrnjačka  Banja", "Opis putovanja...", DateTime.Now.AddDays(10), DateTime.Now.AddDays(15), "Beograd", "Vrnjačka Banja", 12000.00, "");
                //dbContext.Arrangements.Add(a);
                //a = new Aranzman("Manastiri Fruske Gore", "Opis putovanja...", DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 2000.00, "");
                //dbContext.Arrangements.Add(a);
                //dbContext.SaveChanges();

                SviAranzmani = dbContext.Arrangements.ToList();
                if (SviAranzmani.Count > 0)
                    ListViewAranzmans.ItemsSource = SviAranzmani;

                OdabraniAranzmani = new List<Aranzman>();
                BrOdabranih = OdabraniAranzmani.Count();
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

            BrOdabranih = OdabraniAranzmani.Count();
            Trace.WriteLine(BrOdabranih);

        }

        public void PretragaAranzmana(object sender, RoutedEventArgs e)
        {

        }

        public void BrisanjeAranzmana(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Da li želite da izbrišete izabrane aramžmane?";
            string caption = "Brisanje aranžmana";

            MessageBoxButton button = MessageBoxButton.YesNo;

            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
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
                    OdabraniAranzmani = new List<Aranzman>();
                    BrOdabranih = 0;
                }
            }
        }

        public void UkloniAranzman(object sender, RoutedEventArgs e)
        {
            Aranzman aranzman = ((CheckBox)sender).Tag as Aranzman;

            if (OdabraniAranzmani.Contains(aranzman))
            {
                OdabraniAranzmani.Remove(aranzman);
            }

            BrOdabranih = OdabraniAranzmani.Count();
        }


        public void IzmenaAranzmana(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < OdabraniAranzmani.Count(); i++)
            {
                var f = new FormaAranzman(OdabraniAranzmani[i], OdabraniAranzmani.Count(), i + 1);
                f.ShowDialog();
            }
            LoadAranzmani();
        }

        public void KreirajAranzman(object sender, RoutedEventArgs e)
        {
            FormaAranzman forma = new FormaAranzman();
            forma.Show();
            //this.Close();
        }
    }
}
