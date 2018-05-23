﻿using System.Windows;

namespace Hargassner.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Commands.ShutdownCommand.CanExecute(e))
            {
                Commands.ShutdownCommand.Execute(e);
            }
        }
    }
}
