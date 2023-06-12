using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.Entity;
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
    /// Interaction logic for PregledKupovinaView.xaml
    /// </summary>
    public partial class PregledKupovinaView : Window
    {
        public List<Aranzman> KupljeniAranzmani { get; set; } = new List<Aranzman>();
        public List<Aranzman> RezervisaniAranzmani { get; set; } = new List<Aranzman>();
        public User user { get; set; }
        public PregledKupovinaView()
        {
            InitializeComponent();
            this.DataContext = this;

            var noBookings = this.FindName("noBookings") as TextBlock;
            var noReservations = this.FindName("noReservations") as TextBlock;

            using (var dbContext = new MyDbContext())
            {
                this.user = dbContext.Users.SingleOrDefault(u => u.UserName.Equals("jana")) as User;
                var bookings = dbContext.Bookings.Include(d => d.Aranzman).Where(b => b.User.Id == this.user.Id).ToList();

                foreach (var booking in bookings)
                {
                    //Trace.WriteLine(booking.Aranzman);
                    KupljeniAranzmani.Add(booking.Aranzman);
                }

                if (KupljeniAranzmani.Count > 0)
                    ListViewKupovina.ItemsSource = KupljeniAranzmani;
                else
                    noBookings.Visibility = Visibility.Visible;

                var reservations = dbContext.Reservations.Include(d => d.Aranzman).Where(b => b.User.Id == this.user.Id).ToList();
                //var rs = dbContext.Bookings.Include(s => s.);
                foreach (var reservation in reservations)
                {
                    //var aranzman = dbContext.Arrangements.SingleOrDefault(a => a.Id == reservation.Aranzman.Id);
                    Trace.WriteLine(reservation.Aranzman.Name);
                    RezervisaniAranzmani.Add(reservation.Aranzman);
                }

                if (RezervisaniAranzmani.Count > 0)
                    ListViewRezervacija.ItemsSource = RezervisaniAranzmani;
                else
                    noReservations.Visibility = Visibility.Visible;
            }
        }

        public PregledKupovinaView(User user)
        {
            InitializeComponent();
            this.DataContext = this;

            var noBookings = this.FindName("noBookings") as TextBlock;
            var noReservations = this.FindName("noReservations") as TextBlock;

            using (var dbContext = new MyDbContext())
            {
                this.user = dbContext.Users.SingleOrDefault(u => u.UserName.Equals(user.UserName)) as User;
                var bookings = dbContext.Bookings.Include(d => d.Aranzman).Where(b => b.User.Id == this.user.Id).ToList();

                foreach (var booking in bookings)
                {
                    KupljeniAranzmani.Add(booking.Aranzman);
                }

                if (KupljeniAranzmani.Count > 0)
                    ListViewKupovina.ItemsSource = KupljeniAranzmani;
                else
                    noBookings.Visibility = Visibility.Visible;

                var reservations = dbContext.Reservations.Include(d => d.Aranzman).Where(b => b.User.Id == this.user.Id).ToList();
                foreach (var reservation in reservations)
                {
                    RezervisaniAranzmani.Add(reservation.Aranzman);
                }

                if (RezervisaniAranzmani.Count > 0)
                    ListViewRezervacija.ItemsSource = RezervisaniAranzmani;
                else
                    noReservations.Visibility = Visibility.Visible;
            }
        }

        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Aranzman aranzman = ((StackPanel)sender).Tag as Aranzman;
            Trace.WriteLine($"Slika>> {aranzman.PictureLocation}");
            PojedinacanAranzman w = new PojedinacanAranzman(aranzman);
            w.Show();
        }
    }
}
