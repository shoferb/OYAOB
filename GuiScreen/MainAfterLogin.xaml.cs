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

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for MainAfterLogin.xaml
    /// </summary>
    public partial class MainAfterLogin : Window
    {
        private ClientLogic cl;
        private Window parent;
        private Lo
        private int currUserId;
        public MainAfterLogin(Window Parent,int id)
        {
            InitializeComponent();
            cl = new ClientLogic();      
            parent = Parent;
            currUserId = id;
            cl.SetUserId(currUserId);
        }

        private void Logoututton_Click(object sender, RoutedEventArgs e)
        {
           
            
        }
    }
}
