using System.Windows;
using System.Windows.Controls;

namespace Hargassner.Views
{
    /// <summary>
    /// Interaction logic for Heizung
    /// </summary>
    public partial class Heizung : UserControl
    {
        public Heizung()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Commands.StartupCommand.CanExecute(e))
            {
                Commands.StartupCommand.Execute(e);
            }
        }
    }
}
