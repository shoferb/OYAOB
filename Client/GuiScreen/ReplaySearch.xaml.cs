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
            _logic = cl;//init cient logic
            _parent = p;
            foreach (var game in _logic._games)
            {
                ListViewItem roomId = new ListViewItem();
                roomId.Content = game.RoomId;
                ListView_RoomId.Items.Add(roomId);
            }

        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            _parent.Show(); //window Parent
        }

        //private void SearchB_Click(object sender, RoutedEventArgs e)
        //{
        //   
        //}

        private void DoubleClickRoomIdToRep(object sender, MouseButtonEventArgs e)
        {
            this.results_ListView.Items.Clear();
           // int ;
            string ans="";
            int roomIdReq = (int)((ListViewItem)ListView_RoomId.SelectedItem).Content;
          //  bool isValid = int.TryParse(temp, out roomIdReq);
          //  if (!isValid)
          //  {
          ////      MessageBox.Show("Invalid Game ID input");

            //}
          //  else
            //{
                ans = _logic.AskForReplays(roomIdReq);
            //}

            if (ans == null)
            {
                MessageBox.Show("Search Failed!");
            }
            else
            { 
                string[] lines = ans.Split('\n');
                foreach (var line in lines)
                {
                    ListViewItem rep = new ListViewItem();
                    rep.Content = line;
                    results_ListView.Items.Add(rep);
                }
            }
        }
    }
}
