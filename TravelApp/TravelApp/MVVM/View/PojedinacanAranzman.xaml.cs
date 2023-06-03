using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PojedinacanAranzman.xaml
    /// </summary>
    public partial class PojedinacanAranzman : Window, INotifyPropertyChanged
    {

        private Aranzman _aranzman { get; set; }
        public Aranzman Aranzman
        {
            get { return _aranzman; }
            set
            {
                _aranzman = value;
                OnPropertyChanged(nameof(Aranzman));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PojedinacanAranzman(Aranzman aranzman)
        {
            InitializeComponent();
            DataContext = this;

            Aranzman = aranzman;
            if (Aranzman.PictureLocation == null || Aranzman.PictureLocation == "")
            {
                SelectedImage.Source = new BitmapImage(new Uri("C:\\Users\\davor\\OneDrive\\Desktop\\P5-APR-iStock-1359675618.jpg"));
            }
        }
    }
}
