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
            this.results_ListView.Items.Clear();
            bool isAllReq;
            int gameIdReq;
            Tuple<bool, List<string>> ans = new Tuple<bool, List<string>>(false,new List<string>());
            if (SearchFilter_ComboBox.Text.Equals("View All Of My Game Replays"))
            {
                isAllReq = true;
                gameIdReq = -1;
                ans = _logic.AskForReplays(isAllReq, gameIdReq);
            }
            else if(SearchFilter_ComboBox.Text.Equals("View Replay By Game ID"))
            {
                isAllReq = false;
                string temp = GameId_textBox.Text;
                bool isValid = int.TryParse(temp, out gameIdReq);
                if (!isValid)
                {
                    MessageBox.Show("Invalid Game ID input");
                    
                }
                else
                {
                    ans = _logic.AskForReplays(isAllReq, gameIdReq);
                }
            }
            
            if (ans.Item1)
            {
                List<string> replays = ans.Item2;
                foreach (string rep in replays)
                {
                    ListViewItem toAdd = new ListViewItem()
                    {
                        Content = rep
                    };
                    this.results_ListView.Items.Add(toAdd);
                }
            }
            else
            {
                MessageBox.Show("Search Failed!");
            }
        }
    }
}
