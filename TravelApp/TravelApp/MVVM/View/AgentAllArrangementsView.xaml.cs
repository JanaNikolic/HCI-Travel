﻿using MaterialDesignThemes.Wpf;
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
    }
}
