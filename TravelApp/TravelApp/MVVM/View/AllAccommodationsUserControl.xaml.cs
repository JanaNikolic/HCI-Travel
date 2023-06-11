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
    /// Interaction logic for AllAccommodationsUserControl.xaml
    /// </summary>
    public partial class AllAccommodationsUserControl : UserControl, INotifyPropertyChanged
    {
        public List<Smestaj> SviSmestaji { get; set; }
        public List<Smestaj> OdabraniSmestaji { get; set; }

        private int brOdabranih { get; set; }

        public static readonly DependencyProperty IsClickedProperty =
        DependencyProperty.RegisterAttached("IsClicked", typeof(bool), typeof(AllAccommodationsUserControl), new PropertyMetadata(false));

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

        public AllAccommodationsUserControl()
        {
            InitializeComponent();
            this.DataContext = this;

            SviSmestaji = new List<Smestaj>();
            OdabraniSmestaji = new List<Smestaj>();
            BrOdabranih = 0;
            IsClicked = false;
            LoadSmestaje();
        }

        private void LoadSmestaje()
        {
            using (var dbContext = new MyDbContext())
            {
                //Smestaj a = new Smestaj("Smestaj", "Adresa", 3.2, "http://www.google.com");
                //dbContext.Hotels.Add(a);
                //a = new Smestaj("Smesaj 1", "Adresa", 2.5, "https://www.google.com");
                //dbContext.Hotels.Add(a);
                //a = new Smestaj("Smesaj 2", "Adresa", 4, "link");
                //dbContext.Hotels.Add(a);
                //a = new Smestaj("Smesaj 3", "Adresa", 4.5, "link");
                //dbContext.Hotels.Add(a);
                //a = new Smestaj("Smesaj 4", "Adresa", 5, "link");
                //dbContext.Hotels.Add(a);
                dbContext.SaveChanges();

                SviSmestaji = dbContext.Hotels.ToList();
                if (SviSmestaji.Count > 0)
                    ListViewSmestajs.ItemsSource = SviSmestaji;

                OdabraniSmestaji = new List<Smestaj>();
                BrOdabranih = OdabraniSmestaji.Count();
            }
        }

        public void DodajSmestaj(object sender, RoutedEventArgs e)
        {
            Smestaj smestaj = ((CheckBox)sender).Tag as Smestaj;

            Trace.WriteLine(smestaj.ToString());

            if (!OdabraniSmestaji.Contains(smestaj))
            {
                OdabraniSmestaji.Add(smestaj);
            }

            BrOdabranih = OdabraniSmestaji.Count();
            Trace.WriteLine(BrOdabranih);

        }

        public void PretragaSmestaja(object sender, RoutedEventArgs e)
        {

        }

        public void BrisanjeSmestaja(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Da li želite da izbrišete izabrane smeštaje?";
            string caption = "Brisanje smeštaja";

            MessageBoxButton button = MessageBoxButton.YesNo;

            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                using (var dbContext = new MyDbContext())
                {
                    foreach (var smestaj in OdabraniSmestaji)
                    {
                        dbContext.Entry(smestaj).State = System.Data.Entity.EntityState.Deleted;
                    }
                    dbContext.SaveChanges();
                    SviSmestaji = dbContext.Hotels.ToList();
                    ListViewSmestajs.ItemsSource = SviSmestaji;
                    OdabraniSmestaji = new List<Smestaj>();
                    BrOdabranih = 0;
                }
            }
        }

        public void UkloniSmestaj(object sender, RoutedEventArgs e)
        {
            Smestaj aranzman = ((CheckBox)sender).Tag as Smestaj;

            if (OdabraniSmestaji.Contains(aranzman))
            {
                OdabraniSmestaji.Remove(aranzman);
            }

            BrOdabranih = OdabraniSmestaji.Count();
        }


        public void IzmenaSmestaja(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < OdabraniSmestaji.Count(); i++)
            {
                var f = new FormaSmestaj(OdabraniSmestaji[i], OdabraniSmestaji.Count(), i + 1);
                f.ShowDialog();
            }
            LoadSmestaje();
        }

        public void KreirajSmestaj(object sender, RoutedEventArgs e)
        {
            FormaSmestaj forma = new FormaSmestaj();
            forma.Show();
            //this.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
