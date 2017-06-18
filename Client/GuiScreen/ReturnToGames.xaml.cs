using System;
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
            cl = cli;
            //todo - add
            //cl.SetReturnToGameScreen(this);
            startList = new List<ClientGame>();
            listView.ItemsSource = startList;

            listView.AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(HandleDoubleClick));
            InitializeComponent();
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


    

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            Hide();
        }
        public void toStartlist()
        {
            startList = new List<ClientGame>();
        }

        public void SetStartList(List<ClientGame> newStartList)
        {
            startList = newStartList;
            listView.ItemsSource = startList;
        }

        private void HandleDoubleClick(object sender, RoutedEventArgs e)
        {
            ClientGame selectedGame = (ClientGame)listView.SelectedItem;
            if (selectedGame != null)
            {
                if (field == 0)
                {
                   //todo call return to active
                }
                else if (field == 1)
                {
                    //todo call return to spectetor
                }
                currRoomId = selectedGame.roomId;
                cl.JoinTheGame(currRoomId, selectedGame.startingChip);

            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            startList = new List<ClientGame>();

            listView.ItemsSource = startList;
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
          
           
        }
        private void GetAllSpectetorByUserName()
        {
           
            toSearch = cl.user.username;
            cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.SpectetorGameByUserName, toSearch, -1,
                GameMode.Limit);
        }

        private void GetAllUserByUserName()
        {
            
            toSearch = cl.user.username;

            cl.SearchGame(cl.user.id, SearchCommMessage.SearchType.ActiveGamesByUserName, toSearch, -1,
                GameMode.Limit);
        }

        //all active game bby username
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            field = 0;
          
        }

        //all spectetor game by user name
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            field = 1;
           
        }

        public void JoinOkay(GameDataCommMessage msgGameData)
        {
            if (msgGameData.IsSucceed)
            {

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You Return the game successfully! Enjoy");
                    GameScreen newGameWindow = new GameScreen(cl);
                    newGameWindow.UpdateGame(msgGameData);
                    cl.AddNewRoom(newGameWindow);
                    newGameWindow.Show();
                    Hide();
                });
            }
        }

        public void JoinOkayAsSpectate(GameDataCommMessage msgGameData)
             {
            if (msgGameData == null)
            {
                MessageBox.Show("You Can't Return to be a spectator in this game!");
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("You Return to watch game successfully! as Spectetor");
                    GameScreen newGameWindow = new GameScreen(cl);
                    newGameWindow.UpdateGame(msgGameData);

                    cl.AddNewRoom(newGameWindow);
                    newGameWindow.Show();
                    newGameWindow.isSpectrtor = true;
                    Hide();
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
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
