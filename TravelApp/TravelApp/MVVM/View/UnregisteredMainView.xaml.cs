using System;
using System.Collections.Generic;
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

namespace TravelApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for UnregisteredMainView.xaml
    /// </summary>
    public partial class UnregisteredMainView : Window
    {
        public UnregisteredMainView()
        {
            InitializeComponent();
            var Aranzmans = GetAranzmans();
            if (Aranzmans.Count > 0)
                ListViewAranzmans.ItemsSource = Aranzmans;

        }
        private List<Aranzman> GetAranzmans()
        {
            return new List<Aranzman>()
      {
                new Aranzman("Manastiri Fruske Gore", "25.06.2023.", "20.06.2023", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ", "2000 RSD", "Beograd", "../../Images/login.png"),
                new Aranzman("Manastiri Fruske Gore", "25.06.2023.", "20.06.2023", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ", "2000 RSD", "Beograd", "../../Images/login.png"),
                new Aranzman("Manastiri Fruske Gore", "25.06.2023.", "20.06.2023", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ", "2000 RSD", "Beograd", "../../Images/login.png"),
                new Aranzman("Manastiri Fruske Gore", "25.06.2023.", "20.06.2023", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ", "2000 RSD", "Beograd", "../../Images/login.png")};
        }
    
    }

    public class Aranzman 
    {

        public string Name { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string Description { get; set; }
        public string Price { get; set; }
        public string StartAdderss { get; set; }
        public string Image { get; set; }



        public Aranzman(string name, string toDate, string fromDate, string description, string price, string startAddress, string image)
        {
            Name = name;
            FromDate = fromDate;
            ToDate = toDate;
            Description = description;
            Price = price;
            StartAdderss = startAddress;
            Image = image;
        }

    }
}
