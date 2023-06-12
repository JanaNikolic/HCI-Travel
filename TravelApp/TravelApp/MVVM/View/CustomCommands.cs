using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TravelApp.MVVM.View
{
    public class CustomCommands
    {
        public static readonly RoutedCommand Register = new RoutedCommand();
        public static readonly RoutedCommand Logout = new RoutedCommand();
        public static readonly RoutedCommand LoginWindow = new RoutedCommand();
        public static readonly RoutedCommand RegisterWindow = new RoutedCommand();
        public static readonly RoutedCommand UnregisteredWindow = new RoutedCommand();
        public static readonly RoutedCommand OnlineHelp = new RoutedCommand();
    }
}
