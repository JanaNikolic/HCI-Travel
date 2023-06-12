using MaterialDesignThemes.Wpf;
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
    public partial class AgentAllArrangementsView : Window
    {
        
        public AgentAllArrangementsView()
        {
            //using (var dbContext = new MyDbContext())
            //{
            //    Restoran a = new Restoran("Loft", "Novosadskog Sajma 105 Novi Sad", FoodType.Italijanska);
            //    dbContext.Restaurants.Add(a);

            //    Smestaj s = new Smestaj("Smestaj", "Jevrejska 15", 3.2, "http://www.google.com");
            //    dbContext.Hotels.Add(s);

            //    Atrakcija at1 = new Atrakcija("Manastiri Fruske Gore", "Opis putovanja...", "Fruska Gora");
            //    dbContext.Attractions.Add(at1);

            //    Atrakcija at = new Atrakcija("Vrnjačka  Banja", "Opis putovanja...", "Vrnjačka  Banja");
            //    dbContext.Attractions.Add(at);

            //    Aranzman ar = new Aranzman("Manastiri Fruske Gore", "Opis putovanja...", DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), "Beograd", "Novi Sad", 2000.00, "");
            //    ar.Restorani.Add(a);
            //    ar.Atrakcije.Add(at1);
            //    ar.Smestaji.Add(s);
            //    dbContext.Arrangements.Add(ar);

            //    ar = new Aranzman("Vrnjačka  Banja", "Opis putovanja...", DateTime.Now.AddDays(10), DateTime.Now.AddDays(15), "Beograd", "Vrnjačka Banja", 12000.00, "");
            //    ar.Restorani.Add(a);
            //    ar.Atrakcije.Add(at);
            //    ar.Smestaji.Add(s);
            //    dbContext.Arrangements.Add(ar);

            //    dbContext.SaveChanges();
            //}

            InitializeComponent();
            CommandManager.RegisterClassCommandBinding(typeof(PojedinacnaAtrakcija), new CommandBinding(CustomCommands.Logout, CloseExecuted, CanCloseExecute));
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
            Trace.WriteLine(focusedControl.GetType().ToString());
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                if (str == "Restoran")
                {
                    str = "Agent";
                }
                HelpProvider.ShowHelp(str, this);
            }
        }
        private void OnlineHelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            HelpProvider.ShowHelp("Travel", this);
        }

        private void CanOnlineHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }
        private void ReportExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IzvestajMesecniView view = new IzvestajMesecniView();
            view.Show();
        }

        private void CanReportExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Enable the command by default
        }
    }
}
