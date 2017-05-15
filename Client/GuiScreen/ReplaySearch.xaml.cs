using Client.Logic;
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

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for ReplaySearch.xaml
    /// </summary>
    public partial class ReplaySearch : Window
    {

        private ClientLogic _logic;
        private Window _parent;

        public ReplaySearch(ClientLogic cl, Window p)
        {
            InitializeComponent();
            _logic = cl;
            _parent = p;
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _parent.Show();
        }

        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            
            if(SearchFilter_ComboBox.Text.Equals("View All Of My Game Replays"))
            {
                //TODO: Send 
            }
            else if(SearchFilter_ComboBox.Text.Equals("View Replay By Game ID"))
            {
                //TODO: Send
            }
        }
    }
}
