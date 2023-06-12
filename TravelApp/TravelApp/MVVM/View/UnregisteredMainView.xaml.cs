using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        
    
    }

}
