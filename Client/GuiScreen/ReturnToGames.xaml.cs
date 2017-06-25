using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Client.Logic;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for ReturnToGames.xaml
    /// </summary>
    public partial class ReturnToGames : Window
    {
        public ReturnToGames(Window w, ClientLogic cli)
        {
            InitializeComponent();
            parent = w;
            _cl = cli;
            _cl.SetReturnToGameScreen(this);
            _startList = new List<ClientGame>();
            listView.ItemsSource = _startList;

            listView.AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(HandleDoubleClick));
            InitializeComponent();
        }
        private readonly ClientLogic _cl;
        private readonly Window parent;
        private int _currRoomId;
        private int _field = -1;
        private string toSearch;
        private List<ClientGame> _result;

        private List<ClientGame> _startList;

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            Hide();
        }
        public void ToStartlist()
        {
            _startList = new List<ClientGame>();
        }

        public void SetStartList(List<ClientGame> newStartList)
        {
            _startList = newStartList;
            listView.ItemsSource = _startList;
        }

        private void HandleDoubleClick(object sender, RoutedEventArgs e)
        {
            ClientGame selectedGame = (ClientGame)listView.SelectedItem;
            if (selectedGame != null)
            {
                if (_field == 0)
                {
                   _cl.ReturnGamePlayer(selectedGame.roomId);
                }
                else if (_field == 1)
                {
                    _cl.ReturnGameSpec(selectedGame.roomId);
                }
                _currRoomId = selectedGame.roomId;
                _cl.JoinTheGame(_currRoomId, selectedGame.startingChip);

            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _startList = new List<ClientGame>();

            listView.ItemsSource = _startList;
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
            if (_field == -1)
            {
                MessageBox.Show("Please Select a filter ");
                return;
            }
            if (_field == 0) //all active by user name
            {
                GetAllUserByUserName();
            }
            else if (_field == 1) //Spectetor by user name
            {
                GetAllSpectetorByUserName();
            }
          
           
        }
        private void GetAllSpectetorByUserName()
        {
           
            toSearch = _cl.user.username;
            _cl.SearchGame(_cl.user.id, SearchCommMessage.SearchType.SpectetorGameByUserName, toSearch, -1,
                GameMode.Limit, true);
        }

        private void GetAllUserByUserName()
        {
            
            toSearch = _cl.user.username;

            _cl.SearchGame(_cl.user.id, SearchCommMessage.SearchType.ActiveGamesByUserName, toSearch, -1,
                GameMode.Limit);
        }

        //all active game bby username
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            _field = 0;
          
        }

        //all spectetor game by user name
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            _field = 1;
           
        }

        public void PlayerReturnResponseReceived(GameDataCommMessage msgGameData)
        {
            if (msgGameData.IsSucceed)
            {

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You Return the game successfully! Enjoy");
                    GameScreen newGameWindow = new GameScreen(_cl);
                    newGameWindow.UpdateGame(msgGameData);
                    _cl.AddNewRoom(newGameWindow);
                    newGameWindow.Show();
                    Hide();
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You Can't Return to be a player in this game!");
                });
            }
        }

        public void SpecReturnResponseReceived(GameDataCommMessage msgGameData)
        {
            if (msgGameData == null)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You Can't Return to be a spectator in this game!");
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You Return to watch game successfully! as Spectetor");
                    GameScreen newGameWindow = new GameScreen(_cl);
                    newGameWindow.UpdateGame(msgGameData);

                    _cl.AddNewRoom(newGameWindow);
                    newGameWindow.Show();
                    newGameWindow.isSpectrtor = true;
                    Hide();
                });
            }
        }
        public void ResultRecived(List<ClientGame> games)
        {
            _result = games;
            Dispatcher.Invoke(() =>
            {
                listView.ItemsSource = _result;
            });
        }
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
