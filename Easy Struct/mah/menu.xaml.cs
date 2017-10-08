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
using MahApps.Metro.Controls;
using System.Threading;

namespace mah
{
    /// <summary>
    /// Interaction logic for menu.xaml
    /// </summary>
    public partial class menu : MetroWindow
    {
        public menu()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            simulation simulation = new simulation();
            this.Visibility = Visibility.Hidden;
            simulation.ShowDialog();
            this.Visibility = Visibility.Visible;
        }
    }
}
