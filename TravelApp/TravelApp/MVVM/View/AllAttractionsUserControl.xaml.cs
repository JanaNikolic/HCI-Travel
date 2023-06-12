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
    /// Interaction logic for AllAttractionsUserControl.xaml
    /// </summary>
    public partial class AllAttractionsUserControl : UserControl, INotifyPropertyChanged
    {

        public List<Atrakcija> SveAtrakcije { get; set; }
        public List<Atrakcija> OdabraneAtrakcije { get; set; }

        private int brOdabranih { get; set; }

        public static readonly DependencyProperty IsClickedProperty =
        DependencyProperty.RegisterAttached("IsClicked", typeof(bool), typeof(AllAttractionsUserControl), new PropertyMetadata(false));

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

        public AllAttractionsUserControl()
        {
            InitializeComponent();

            this.DataContext = this;

            SveAtrakcije = new List<Atrakcija>();
            OdabraneAtrakcije = new List<Atrakcija>();
            BrOdabranih = 0;
            IsClicked = false;


            LoadAtrakcije();
        }

        private void LoadAtrakcije()
        {
            using (var dbContext = new MyDbContext())
            {
                //Atrakcija a = new Atrakcija("Manastiri Fruske Gore", "Opis putovanja...", "Fruska Gora");
                //dbContext.Attractions.Add(a);
                //a = new Atrakcija("Vrnjačka  Banja", "Opis putovanja...", "Vrnjačka  Banja");
                //dbContext.Attractions.Add(a);
                //a = new Atrakcija("Manastiri Fruske Gore", "Opis putovanja...", "Fruska Gora");
                //dbContext.Attractions.Add(a);
                //a = new Atrakcija("Vrnjačka  Banja", "Opis putovanja...", "Vrnjačka  Banja");
                //dbContext.Attractions.Add(a);
                //a = new Atrakcija("Manastiri Fruske Gore", "Opis putovanja...", "Fruska Gora");
                //dbContext.Attractions.Add(a);
                //dbContext.SaveChanges();

                SveAtrakcije = dbContext.Attractions.ToList();
                if (SveAtrakcije.Count > 0)
                    ListViewAtrakcijas.ItemsSource = SveAtrakcije;

                OdabraneAtrakcije = new List<Atrakcija>();
                BrOdabranih = OdabraneAtrakcije.Count();
            }
        }

        public void DodajAtrakciju(object sender, RoutedEventArgs e)
        {
            Atrakcija atrakcija = ((CheckBox)sender).Tag as Atrakcija;

            Trace.WriteLine(atrakcija.ToString());

            if (!OdabraneAtrakcije.Contains(atrakcija))
            {
                OdabraneAtrakcije.Add(atrakcija);
            }

            BrOdabranih = OdabraneAtrakcije.Count();
            Trace.WriteLine(BrOdabranih);

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

        public void PretragaAtrakcija(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                Trace.WriteLine(PretragaTB);
                SveAtrakcije = dbContext.Attractions.Where(a => a.Name.Contains(PretragaTB)).ToList();

                if (SveAtrakcije.Count > 0)
                    ListViewAtrakcijas.ItemsSource = SveAtrakcije;

                OdabraneAtrakcije.Clear();
                BrOdabranih = 0;

            }
        }

        public void BrisanjeAtrakcija(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Da li želite da izbrišete izabrane atrakcije?";
            string caption = "Brisanje atrakcija";

            MessageBoxButton button = MessageBoxButton.YesNo;

            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                using (var dbContext = new MyDbContext())
                {
                    foreach (var atrakcija in OdabraneAtrakcije)
                    {
                        dbContext.Entry(atrakcija).State = System.Data.Entity.EntityState.Deleted;
                    }
                    dbContext.SaveChanges();
                    SveAtrakcije = dbContext.Attractions.ToList();
                    ListViewAtrakcijas.ItemsSource = SveAtrakcije;
                    OdabraneAtrakcije = new List<Atrakcija>();
                    BrOdabranih = 0;
                }
            }
        }

        public void UkloniAtrakciju(object sender, RoutedEventArgs e)
        {
            Atrakcija atrakcija = ((CheckBox)sender).Tag as Atrakcija;

            if (OdabraneAtrakcije.Contains(atrakcija))
            {
                OdabraneAtrakcije.Remove(atrakcija);
            }

            BrOdabranih = OdabraneAtrakcije.Count();
        }


        public void IzmenaAtrakcija(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < OdabraneAtrakcije.Count(); i++)
            {
                var f = new FormaAtrakcija(OdabraneAtrakcije[i], OdabraneAtrakcije.Count(), i + 1);
                f.ShowDialog();
            }
            LoadAtrakcije();
        }

        public void KreirajAtrakciju(object sender, RoutedEventArgs e)
        {
            FormaAtrakcija forma = new FormaAtrakcija();
            forma.Show();
            //this.Close();
            LoadAtrakcije();
        }

        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Atrakcija atrakcija = ((StackPanel)sender).Tag as Atrakcija;
            Trace.WriteLine(atrakcija);
            PojedinacnaAtrakcija w = new PojedinacnaAtrakcija(atrakcija);
            w.Show();
        }
    }
}
