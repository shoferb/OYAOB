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
using Client.Logic;

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for SearchMenuScreen.xaml
    /// </summary>
    public partial class SearchMenuScreen : Window
    {

        private ClientLogic cl;
        private Window parentScreen; 

        public SearchMenuScreen(Window w,ClientLogic cli)
        {
            InitializeComponent();
            parentScreen = w;
            cl = cli;
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            this.parentScreen.Show();
            this.Hide();
        }
    }
}
