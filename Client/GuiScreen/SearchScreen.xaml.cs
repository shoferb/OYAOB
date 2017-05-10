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
using TexasHoldemShared.CommMessages.ClientToServer;

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for SearchScreen.xaml
    /// </summary>
    public partial class SearchScreen : Window
    {
        public SearchScreen(Window w,ClientLogic cli)
        {
            InitializeComponent();
            parent = w;
            cl = cli;
            startList = new List<ClientGame>();
            listView.ItemsSource = startList;

            listView.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(HandleDoubleClick));
        }

        private ClientLogic cl;
        private Window parent;
        private int currRoomId;
        private int field;
        private string toSearch;
        private List<ClientGame> result;
    
        bool isValid = false;
        private List<ClientGame> startList;
        private int idSearch;
        private Window perent;
        private DateTime dateToSearch;
        private int memberIdSearch;



        public void toStartlist()
        {
            startList = new List<ClientGame>();
        }

        public void SetStartList(List<ClientGame> newStartList)
        {
            this.startList = newStartList;
            listView.ItemsSource = startList;
        }

        private void HandleDoubleClick(object sender, RoutedEventArgs e)
        {
            ClientGame selectedGame = (ClientGame) listView.SelectedItem;
            if (selectedGame != null)
            {
                currRoomId = selectedGame.roomId;
               //todo - bar continue frome here join game
            }
        }

       

 

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            this.Hide();
        }



        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            startList = new List<ClientGame>();

            listView.ItemsSource = startList;
        }



        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    


        private void emptySearch()
        {
            MessageBox.Show("The search returned no results.");
        }

        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
           
            if (field == 0) //all active by user name
            {

                toSearch = searchBox.Text;
                
                List<ClientGame> temp;
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ActiveGamesByUserName, toSearch, -1,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 1) //Spectetor by user name
            {

                toSearch = searchBox.Text;
                List<ClientGame> temp;
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.SpectetorGameByUserName, toSearch, -1,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 2) //all active games user can join
            {

                
                List<ClientGame> temp = null;
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.GamesUserCanJoin, "", -1,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 3) //by room Id
            {
                toSearch = searchBox.Text;
                int toSearchRoomId;
               
               
                List<ClientGame> temp;
                isValid = int.TryParse(toSearch, out idSearch);
                if (isValid)
                {
                    temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByRoomId, "", idSearch,
                        GameMode.Limit);
                    result = temp;
                    if (result == null || !result.Any())
                    {
                        emptySearch();
                    }
                    else
                    {
                        listView.ItemsSource = result;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid room id should contains only numbers");
                }
            }
            else if (field == 4) //all spectetors
            {
                List<ClientGame> temp = null;
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.AllSepctetorGame, "", -1,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 5) //mmberid
            {
                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                isValid = int.TryParse(toSearch, out memberIdSearch);
                if (isValid)
                {
                    temp = _cusBl.GetByClubMemberID(memberIdSearch);
                    result = temp;
                    if (result == null || !result.Any())
                    {
                        emptySearch();
                    }
                    else
                    {
                        listView.ItemsSource = result;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Input-member Id contains only numbers");
                }
            }
            if (field == 6) //get all
            {
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                temp = _cusBl.GetAllClubMember();
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
        }

        //all active game bby username
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            field = 0;
            searchBox.IsEnabled = true;
        }

        //all spectetor game by user name
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            field = 1;
            searchBox.IsEnabled = true;
        }

        //all active game user can join
        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            field = 2;
            searchBox.IsEnabled = false;
        }

        //get room by room id
        private void ComboBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {
            field = 3;
            searchBox.IsEnabled = true;
        }

        //all spectetor game
        private void ComboBoxItem_Selected_4(object sender, RoutedEventArgs e)
        {
            field = 4;
            searchBox.IsEnabled = false;
        }

        //by min player
        private void ComboBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            field = 5;
            searchBox.IsEnabled = true;
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //by max player in room
        private void ComboBoxItem_Selected_6(object sender, RoutedEventArgs e)
        {
            field = 6;
            searchBox.IsEnabled = true;
        }

        //by min bet in room
        private void ComboBoxItem_Selected_7(object sender, RoutedEventArgs e)
        {
            field = 7;
            searchBox.IsEnabled = true;
        }

        //by potSize
        private void ComboBoxItem_Selected_8(object sender, RoutedEventArgs e)
        {
            field = 8;
            searchBox.IsEnabled = true;
        }


        //by buy in policy
        private void ComboBoxItem_Selected_9(object sender, RoutedEventArgs e)
        {
            field = 9;
            searchBox.IsEnabled = true;
        }

        //by starting chip
        private void ComboBoxItem_Selected_10(object sender, RoutedEventArgs e)
        {
            field = 10;
            searchBox.IsEnabled = true;
        }


        //by not-limit game
        private void ComboBoxItem_Selected_11(object sender, RoutedEventArgs e)
        {
            field = 11;
            searchBox.IsEnabled = false;
        }


        //by limit
        private void ComboBoxItem_Selected_12(object sender, RoutedEventArgs e)
        {
            field = 12;
            searchBox.IsEnabled = false; ;
        }

        //pot-limit
        private void ComboBoxItem_Selected_13(object sender, RoutedEventArgs e)
        {
            field = 13;
            searchBox.IsEnabled = false; ;
        }
    }
}
