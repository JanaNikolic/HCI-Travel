using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public User user { get; set; }

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

            SviAranzmani = new List<Aranzman>();

            using (var dbContext = new MyDbContext())
            {
                SviAranzmani = dbContext.Arrangements.ToList();
                if (SviAranzmani.Count > 0)
                    ListViewAranzmans.ItemsSource = SviAranzmani;
            }
        }
    }

}
