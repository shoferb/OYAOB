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
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

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
            cl.SetSearchScreen(this);
            startList = new List<ClientGame>();
            listView.ItemsSource = startList;

            listView.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(HandleDoubleClick));
        }

        private ClientLogic cl;
        private Window parent;
        private int currRoomId;
        private int field = -1;
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
                cl.JoinTheGame(currRoomId, selectedGame.startingChip);
                
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

        public void EmptySearch()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("The search returned no results.");
            });
        }

        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            if (field == -1)
            {
                MessageBox.Show("Please Select a filter ");
                return;
            }
            if (field == 0) //all active by user name
            {
                GetAllUserByUserName();
            }
            else if (field == 1) //Spectetor by user name
            {
                GetAllSpectetorByUserName();
            }
            else if (field == 2) //all active games user can join
            {
                AllActiveGamesUserCanJoin();
            }
            else if (field == 3) //by room Id
            {
                GetRoomById();
            }
            else if (field == 4) //all spectetors
            {
                GetAllSpectetor();
            }
            else if (field == 5) //min player
            {
                GetGamesByMinPlayer();
            }
            if (field == 6) //max player num
            {
                GetGamesByMaxPlayer();
            }
            if (field == 7)
            {
                GetGamesByMinBet();
            }
            if (field == 8)
            {
                GetGamesByPotSize();
            }

            if (field == 9)
            {
                GetGamesByBuyInPolicy();
            }
            if (field == 10)//by starting chip
            {
                GetGameByStartingChip();
            }
            if (field == 11)//by not limit
            {
                GetNotLimitGames();
            }
            if (field == 12)//by limit
            {
                GetLinitGames();
            }
            if (field == 13)//by limit
            {
                GetPotLimitGames();
            }

        }

        private void GetPotLimitGames()
        {
            List<ClientGame> temp;
           
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByGameMode, "", -1,
                GameMode.PotLimit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
            }
        }

        private void GetLinitGames()
        {
            List<ClientGame> temp;
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByGameMode, "", -1,
                GameMode.Limit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
            }
        }

        private void GetNotLimitGames()
        {
            List<ClientGame> temp;
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByGameMode, "", -1,
                GameMode.NoLimit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
            }
        }

        private void GetGameByStartingChip()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter amount of starting chip");
                return;
                
            }
            toSearch = searchBox.Text;
            int toSearchSartingChip;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchSartingChip);
            if (isValid)
            {
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByStartingChip, "", toSearchSartingChip,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    EmptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else
            {
                MessageBox.Show("Invalid chip should contains only numbers");
            }
        }

        private void GetGamesByBuyInPolicy()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter amount of buy in policy");
                return;

            }
            toSearch = searchBox.Text;
            int toSearchBuyIn;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchBuyIn);
            if (isValid)
            {
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByBuyInPolicy, "", toSearchBuyIn,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    EmptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else
            {
                MessageBox.Show("Invalid buy in policy should contains only numbers");
            }
        }

        private void GetGamesByPotSize()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter amount of pot size");
                return;

            }
            toSearch = searchBox.Text;
            int toSearchPotSize;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchPotSize);
            if (isValid)
            {
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByPotSize, "", toSearchPotSize,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    EmptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else
            {
                MessageBox.Show("Invalid pot size should contains only numbers");
            }
        }

        private void GetGamesByMinBet()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter amount of min bet in room");
                return;

            }
            toSearch = searchBox.Text;
            int toSearchMinBet;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchMinBet);
            if (isValid)
            {
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByMinBet, "", toSearchMinBet,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    EmptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else
            {
                MessageBox.Show("Invalid min bet should contains only numbers");
            }
        }

        private void GetGamesByMaxPlayer()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter amount of max player in room");
                return;

            }
            toSearch = searchBox.Text;
            int toSearchmaxPlayer;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchmaxPlayer);
            if (isValid)
            {
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByMaxPlayer, "", toSearchmaxPlayer,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    EmptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else
            {
                MessageBox.Show("Invalid player number should contains only numbers");
            }
        }

        private void GetGamesByMinPlayer()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter amount min player in room");
                return;

            }
            toSearch = searchBox.Text;
            int toSearchminPlayer;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchminPlayer);
            if (isValid)
            {
                temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByMinPlayer, "", toSearchminPlayer,
                    GameMode.Limit);
                result = temp;
                if (result == null || !result.Any())
                {
                    EmptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else
            {
                MessageBox.Show("Invalid player number should contains only numbers");
            }
        }

        private void GetAllSpectetor()
        {
            List<ClientGame> temp = null;
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.AllSepctetorGame, "", -1,
                GameMode.Limit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
            }
        }

        private void GetRoomById()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter room id to search");
                return;

            }
            toSearch = searchBox.Text;
            int toSearchRoomId;


            List<ClientGame> temp;
            isValid = int.TryParse(toSearch, out toSearchRoomId);
            if (isValid)
            {
                //temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByRoomId, "", toSearchRoomId,
                //    GameMode.Limit);
                //result = temp;
                //if (result == null || !result.Any())
                //{
                EmptySearch();
                //}
                //else
                //{
                //    listView.ItemsSource = result;
                //}

                cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ByRoomId, "", toSearchRoomId,
                    GameMode.Limit);
            }
            else
            {
                MessageBox.Show("Invalid room id should contains only numbers");
            }
        }

        private void AllActiveGamesUserCanJoin()
        {
            List<ClientGame> temp = null;
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.GamesUserCanJoin, "", -1,
                GameMode.Limit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
            }
        }

        private void GetAllSpectetorByUserName()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter username");
                return;

            }
            toSearch = searchBox.Text;
            List<ClientGame> temp;
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.SpectetorGameByUserName, toSearch, -1,
                GameMode.Limit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
            }
        }

        private void GetAllUserByUserName()
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                MessageBox.Show("please enter username");
                return;

            }
            toSearch = searchBox.Text;

            List<ClientGame> temp;
            temp = cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ActiveGamesByUserName, toSearch, -1,
                GameMode.Limit);
            result = temp;
            if (result == null || !result.Any())
            {
                EmptySearch();
            }
            else
            {
                listView.ItemsSource = result;
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

        private void WatchGame_Btn_Click(object sender, RoutedEventArgs e)
        {
            int roomIdToSpectate;
            string temp = IdToSpectate_TextBox.Text;
            bool isValid = int.TryParse(temp, out roomIdToSpectate);
            if (!isValid)
            {
                MessageBox.Show("Invalid Game ID input");
            }
            else
            {
               cl.SpectateRoom(roomIdToSpectate);
               
            }
        }

        public void JoinOkay(GameDataCommMessage msgGameData)
        {
            if (msgGameData.IsSucceed)
            {

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You joined the game successfully!");
                    GameScreen newGameWindow = new GameScreen(cl);
                    newGameWindow.UpdateGame(msgGameData);
                    cl.AddNewRoom(newGameWindow);
                    newGameWindow.Show();
                    this.Hide();
                });
            }

            else
            {
                MessageBox.Show("Joined the game failed!");
            }
        }

        public void JoinOkayAsSpectate(GameDataCommMessage msgGameData)
        {
            if (msgGameData == null)
            {
                MessageBox.Show("You Can't be a spectator in this game!");
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You joined the game successfully! as Spectetor");
                    GameScreen newGameWindow = new GameScreen(cl);
                    newGameWindow.UpdateGame(msgGameData);
                    cl.AddNewRoom(newGameWindow);
                    newGameWindow.Show();
                    this.Hide();
                });
            }
        }

        public void ResultRecived(List<ClientGame> games)
        {
            result = games;
            Dispatcher.Invoke(() =>
            {
                listView.ItemsSource = result;
            });
        }
    }
}
