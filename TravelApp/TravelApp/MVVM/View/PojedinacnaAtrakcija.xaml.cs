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
    /// Interaction logic for PojedinacnaAtrakcija.xaml
    /// </summary>
    public partial class PojedinacnaAtrakcija : Window, INotifyPropertyChanged
    {
        private Model.Atrakcija _atrakcija { get; set; }

        public Model.Atrakcija Atrakcija
        {
            get { return _atrakcija; }
            set
            {
                _atrakcija = value;
                OnPropertyChanged(nameof(Atrakcija));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PojedinacnaAtrakcija(Atrakcija atrakcija)
        {
            InitializeComponent();
            DataContext = this;
            this.Atrakcija = atrakcija;
        }
    }
}
