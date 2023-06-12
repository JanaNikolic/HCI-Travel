using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    /// Interaction logic for IzvestajMesecniView.xaml
    /// </summary>
    public partial class IzvestajMesecniView : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<MesecniIzvestaj> data { get; set; }


        private double suma { get; set; }
        public double Suma
            {
            get { return suma; }
            set
            {
                suma = value;
                OnPropertyChanged(nameof(Suma));

            }
        }

        private int izabranMesec { get; set; }
        public int IzabranMesec
        {
            get { return izabranMesec; }
            set
            {
                izabranMesec = value;
                OnPropertyChanged(nameof(IzabranMesec));

            }
        }

        private int brProdatih { get; set; }

        public int BrProdatih
        {
            get { return brProdatih; }
            set
            {
                brProdatih = value;
                OnPropertyChanged(nameof(BrProdatih));

            }
        }

        private string mesec { get; set; }
        public string Mesec
        {
            get { return mesec; }
            set
            {
                mesec = value;
                OnPropertyChanged(nameof(Mesec));

            }
        }

        public IzvestajMesecniView()
        {
            InitializeComponent();
            this.DataContext = this;

            var cmbMeseci = this.FindName("cmbMeseci") as ComboBox;
            cmbMeseci.ItemsSource = Enum.GetValues(typeof(Meseci)).Cast<Meseci>();


            data = new ObservableCollection<MesecniIzvestaj>();
            

            
            //Bind the DataGrid to the customer data
            //DG1.DataContext = data;
        }

        private void cbValueType_DropDownClosed(object sender, EventArgs e)
        {
            data.Clear();
            BrProdatih = 0;
            Suma = 0;
            Mesec = cmbMeseci.SelectedValue.ToString();
            if (cmbMeseci.SelectedIndex == 4) 
            {
                var gr = this.FindName("dataGrid") as Grid;
                if (gr.Visibility == Visibility.Collapsed)
                    gr.Visibility = Visibility.Visible;
                var noData = this.FindName("noData") as TextBlock;

                if (noData.Visibility == Visibility.Visible)
                    noData.Visibility = Visibility.Collapsed;
                MesecniIzvestaj izvestaj = new MesecniIzvestaj("Maj", "Manastiri Srbije", 50, 5000.0, 250000.0);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);
                
                izvestaj = new MesecniIzvestaj("Maj", "Soko Banja", 25, 8000.0, 8000.0 * 25);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("Maj", "Vrnjačka Banja", 25, 8000.0, 8000.0 * 25);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("Maj", "Đavolja Varoš", 15, 8000.0, 8000.0 * 15);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("Maj", "Krstarenje Beogradom", 25, 4000.0, 4000.0 * 25);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("Maj", "Lepenski Vir", 10, 3000.0, 3000.0 * 10);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("Maj", "Manastiri Fruške Gore", 10, 3000.0, 3000.0 * 10);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                var DG1 = this.FindName("DG1") as DataGrid;
                DG1.DataContext = data;
            } 
            else if (cmbMeseci.SelectedIndex == 3)
            {
                var gr = this.FindName("dataGrid") as Grid;
                if (gr.Visibility == Visibility.Collapsed)
                    gr.Visibility = Visibility.Visible;
                var noData = this.FindName("noData") as TextBlock;

                if (noData.Visibility == Visibility.Visible)
                    noData.Visibility = Visibility.Collapsed;

                MesecniIzvestaj izvestaj = new MesecniIzvestaj("April", "Soko Banja", 5, 8000.0, 8000.0 * 5);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("April", "Vrnjačka Banja", 10, 8000.0, 8000.0 * 10);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("April", "Đavolja Varoš", 15, 8000.0, 8000.0 * 15);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("April", "Krstarenje Beogradom", 15, 4000.0, 4000.0 * 15);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("April", "Lepenski Vir", 20, 3000.0, 3000.0 * 20);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                izvestaj = new MesecniIzvestaj("April", "Manastiri Fruške Gore", 10, 3000.0, 3000.0 * 10);
                BrProdatih += izvestaj.BrProdatih;
                Suma += izvestaj.UkupnaCena;
                data.Add(izvestaj);

                var DG1 = this.FindName("DG1") as DataGrid;
                DG1.DataContext = data;
            }
            else 
            {
                var gr = this.FindName("dataGrid") as Grid;
                gr.Visibility = Visibility.Collapsed;
                var noData = this.FindName("noData") as TextBlock;
                noData.Visibility = Visibility.Visible;
            }
            //if (cbValueType.SelectedIndex == ) //sel ind already updated
            //{
            //    // change sel Index of other Combo for example
            //    cbDataType.SelectedIndex = someotherIntValue;
            //}
        }
    }

    public enum Meseci
    {
        Januar, Februar, Mart, April, Maj, Jun, Jul, Avgust, Septembar, Oktobar, Novembar, Decembar
    }
}
