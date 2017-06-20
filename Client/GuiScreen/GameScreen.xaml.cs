using Client.Logic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TexasHoldem;
using TexasHoldem.GuiScreen;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ServerToClient;
using Application = System.Windows.Application;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MessageBox = System.Windows.MessageBox;

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        public int RoomId;
        public Card[] PlayerCards = new Card[2];
        public List<Card> TableCards;
        public int TotalChips;
        public int PotSize;
        public List<string> AllPlayerNames;
        public List<string> AllSpecNames;
        public string DealerName;
        public string BbName;
        public string SbName;
        public string CurrPlayerTurn;
        bool SpecOrPlay;//spec=false, play=true;
        private ClientLogic _logic;
        public GameDataCommMessage update;
        public bool isSpectrtor { get; set; }
        public GameScreen(ClientLogic c)
        {
            InitializeComponent();
            _logic = c;
          /*  UserID.Content = _logic.user.id;
            UserName.Content = _logic.user.username;
            string path = _logic.user.avatar;
            Avatar.Source = new BitmapImage(new Uri(@path, UriKind.Relative));*/
            update = null;
            ActionLabel.Visibility = Visibility.Hidden;
            ChooseActionLabel.Visibility = Visibility.Hidden;
            ActionAmountLabel.Visibility = Visibility.Hidden;
            ActionChosenComboBox.Visibility = Visibility.Hidden;
            InputForActionTextBox.Visibility = Visibility.Hidden;
            DoActiomBotton.Visibility = Visibility.Hidden;
            isSpectrtor = false;
        }
       
        public void UpdateGame(GameDataCommMessage msg)
        {

            PopupUser(msg.ActionPlayerName, msg.Action, msg.IsSucceed);

            update = msg;
            Dispatcher.Invoke(() =>
            {
                UserID.Content = _logic.user.id;
                UserName.Content = _logic.user.username;
                string path = _logic.user.avatar;
                Avatar.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                if (_logic.user.username.Equals(msg.CurrPlayerTurn))
                {
                    ActionLabel.Visibility = Visibility.Visible;
                    ChooseActionLabel.Visibility = Visibility.Visible;
                    ActionAmountLabel.Visibility = Visibility.Visible;
                    ActionChosenComboBox.Visibility = Visibility.Visible;
                    InputForActionTextBox.Visibility = Visibility.Visible;
                    DoActiomBotton.Visibility = Visibility.Visible;

                }
                else
                {
                    ActionLabel.Visibility = Visibility.Hidden;
                    ChooseActionLabel.Visibility = Visibility.Hidden;
                    ActionAmountLabel.Visibility = Visibility.Hidden;
                    ActionChosenComboBox.Visibility = Visibility.Hidden;
                    InputForActionTextBox.Visibility = Visibility.Hidden;
                    DoActiomBotton.Visibility = Visibility.Hidden;
                }
                if (!String.IsNullOrEmpty(msg.Winner))
                {
                    if (msg.Winner.Contains(","))
                    {
                        winnerNameLabel.Content = "Winners: ";
                    }
                    else
                    {
                        winnerNameLabel.Content = "Winner: ";

                    }
                    winnerName.Content = msg.Winner;
                    string msgToChat = string.Concat("*GAME MESSAGE* ","The " + winnerNameLabel.Content + winnerName.Content, " Won the game.");
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = msgToChat;
                    this.chatListView.Items.Add(toAdd);
                     msgToChat = string.Concat("*GAME MESSAGE* ", " Game is Over ");
                    ListViewItem toAdd1 = new ListViewItem();
                    toAdd1.Content = msgToChat;
                    this.chatListView.Items.Add(toAdd1);
                    this.PotAmountLabel.Content = "";
                    this.BB.Content = "";
                    this.CurrRound.Content = "";
                    this.CurrTurnNameLabel.Content = "";
                    this.DealerNameLabel.Content = "";
                    this.SB.Content = "";
                    this.PublicCardView.Items.Clear();
                    this.PlayerCards = msg.PlayerCards;
                    string pre1 = "First Card is :    ";
                    string pre2 = "Second Card is : ";
                    this.Card1Labek.Content = string.Concat(pre1);
                    this.Card2Label.Content = string.Concat(pre2);
                    return;
                }
                this.RoomId = msg.RoomId;
                string pre = "Room Number: ";
                
                RoomNum.Content = string.Concat(pre, RoomId);
                if (msg.SbName != null)
                {
                    this.SbName = msg.SbName;
                    this.SB.Content = msg.SbName;
                }
                if (msg.AllPlayerNames != null)
                {
                    this.AllPlayerNames = msg.AllPlayerNames;
                    List<ViewObj> players = new List<ViewObj>();
                    this.ListViewPlayers.Items.Clear();
                    foreach (string aString in AllPlayerNames)
                    {
                        ListViewItem toAdd = new ListViewItem();
                        toAdd.Content = aString;
                        this.ListViewPlayers.Items.Add(aString);
                    }
                    // ListViewPlayers.ItemsSource = players;
                    foreach (string playerName in AllPlayerNames)
                    {
                        if (_logic.user.name.Equals(playerName))
                        {
                            this.SpecOrPlay = true;
                        }
                    }
                }

                if (msg.AllSpectatorNames != null)
                {
                    this.AllSpecNames = msg.AllSpectatorNames;
                    this.ListViewSpectetors.Items.Clear();
                    foreach (string aString in AllSpecNames)
                    {
                        if (_logic.user.username.Equals(aString))
                        {
                            isSpectrtor = true;
                        }
                        ListViewItem toAdd = new ListViewItem();
                        toAdd.Content = aString;
                        this.ListViewSpectetors.Items.Add(aString);
                    }
                  
                }

                if (msg.BbName != null)
                {
                    this.BbName = msg.BbName;
                    this.BB.Content = msg.BbName;
                }

                if (msg.CurrRound != null)
                {
                    this.CurrRound.Content = msg.CurrRound;
                }

                if (msg.CurrPlayerTurn != null)
                {
                    this.CurrPlayerTurn = msg.CurrPlayerTurn;
                    this.CurrTurnNameLabel.Content = msg.CurrPlayerTurn;
                }
                if (msg.DealerName != null)
                {
                    this.DealerName = msg.DealerName;
                    this.DealerNameLabel.Content = msg.DealerName;
                }
                if (!isSpectrtor)
                {
                    if ((msg.PlayerCards[0] != null) || (msg.PlayerCards[1] != null))
                    {
                        this.PlayerCards = msg.PlayerCards;
                        string pre1 = "First Card is :    ";
                        string pre2 = "Second Card is : ";
                        this.Card1Labek.Content = string.Concat(pre1, (msg.PlayerCards[0]).ToString());
                        this.Card2Label.Content = string.Concat(pre2, (msg.PlayerCards[1]).ToString());
                    }
                }
                else
                {
                    myCardsLabel.Content = "";
                    Card1Labek.Visibility = Visibility.Hidden;
                    Card2Label.Visibility = Visibility.Hidden;
                }

                
                this.PotSize = msg.PotSize;
                this.PotAmountLabel.Content = msg.PotSize;

                if (msg.TableCards != null)
                {
                    this.PublicCardView.Items.Clear();
                    this.TableCards = msg.TableCards;                    
                    foreach (Card aCard in TableCards)
                    {                  
                        if (aCard != null)
                        {         
                            // PUBLIC cards 
                            ListViewItem publicCardItem = new ListViewItem();                         
                            publicCardItem.Content = string.Concat(aCard.ToString());
                            this.PublicCardView.Items.Add(publicCardItem);
                        }
                    }
                }
                if (!isSpectrtor)
                {
                    this.TotalChips = msg.TotalChips;
                    this.ChipAmountLabel.Content = msg.TotalChips;
                }
                else
                {
                    this.TotalChips = 0;
                    this.ChipAmountLabel.Content = "";


                }
                
                if (msg.IsSucceed)
                {
                    string msgToChat = "";
                    if (msg.Action.Equals(CommunicationMessage.ActionType.Bet))
                    {
                        if (msg.BetAmount == 0)
                        {
                            msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " Checked");
                        }
                        else
                        {
                            msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " Bet with amount of ",
                                msg.BetAmount);
                        }
                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.Fold))
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " Folded.");
                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.Join))
                    {
                        
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " Joined the game.");
                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.Spectate))
                    {

                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " Joined as a spectetor to game.");
                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.Leave))
                    {
                         msgToChat = string.Concat("*GAME MESSAGE* ", " player: " + msg.ActionPlayerName,
                                " left the game.");
  
                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.SpectatorLeave))
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", " spectetor: " + msg.ActionPlayerName,
                            " left the game.");

                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.StartGame))
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " started the game.");
                    }
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.CreateRoom))
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " game  was created.");
                    }


                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = msgToChat;
                    this.chatListView.Items.Add(toAdd);
                    if (msg.ActionPlayerName.Equals(_logic.user.username) && msg.Action.Equals(CommunicationMessage.ActionType.Leave))
                    {
                        this.Close();
                    }

                }

            });
        }

        private void PopupUser(string userName, CommunicationMessage.ActionType msgAction, bool msgIsSucceed)
        {
            if (userName.Equals(_logic.user.username))
            {
                string msg = "";
                switch (msgAction)
                {
                    case CommunicationMessage.ActionType.Bet:
                        msg = "Bet ";
                        break;
                    case CommunicationMessage.ActionType.Fold:
                        msg = "Fold ";
                        break;
                    case CommunicationMessage.ActionType.Leave:
                        msg = "Leave ";
                        break;
                    case CommunicationMessage.ActionType.StartGame:
                        msg = "Start Game ";
                        break;
                } 
                if (msgIsSucceed)
                {
                    MessageBox.Show(msg + "succeeded!");
                    return;
                }
                MessageBox.Show(msg + "failed!");
            }


        }

        public void AddChatMsg(ChatResponceCommMessage msg)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
            new Action(delegate ()
            {
                if (msg.chatType == CommunicationMessage.ActionType.PlayerBrodcast)
                {
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = string.Concat("Player Broadcast message from  ", msg.senderngUsername, ": ", msg.msgToSend);
                    chatListView.Items.Add(toAdd);
                }
                else if (msg.chatType == CommunicationMessage.ActionType.PlayerWhisper)
                {
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = string.Concat("Player Whisper message from  ", msg.senderngUsername, ": ", msg.msgToSend);
                    chatListView.Items.Add(toAdd);
                }
                else if (msg.chatType == CommunicationMessage.ActionType.SpectetorBrodcast)
                {
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = string.Concat("Spectetor Broadcast message from  ", msg.senderngUsername, ": ", msg.msgToSend);
                    chatListView.Items.Add(toAdd);
                }
                else if (msg.chatType == CommunicationMessage.ActionType.SpectetorWhisper)
                {
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = string.Concat("Spectetor Whisper message from  ", msg.senderngUsername, ": ", msg.msgToSend);
                    chatListView.Items.Add(toAdd);
                }
              
            }
            ));


           
           
        }

        private void LeaveBotton_Click(object sender, RoutedEventArgs e)
        {
            if (!isSpectrtor)
            {
                _logic.LeaveTheGame(this.RoomId);
            }
            else
            {
                _logic.SpectetorLeaveTheGame(this.RoomId);
            }
        }

        private void StartTheGameBTN_Click(object sender, RoutedEventArgs e)
        {
             _logic.StartTheGame(this.RoomId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you Sure you want To logout?", "LogoutFromSystem", MessageBoxButton.YesNo);
            bool done = false;
            while (!done)
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            string username = _logic.user.username;
                            string password = _logic.user.password;
                            bool logoutOk = _logic.Logout(username, password);
                            if (logoutOk)
                            {
                                MessageBox.Show("Logout OK!");

                                //WellcomeScreen wellcomeScreen = new WellcomeScreen();

                                // wellcomeScreen.Show();
                                _logic.CloseSystem();
                                Application.Current.Shutdown();
                                //this.Hide();
                                done = true;
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Logout Fail! - please try again");
                                break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Logout Fail! Exeption - please try again");
                            done = true;
                            break;
                        }


                    case MessageBoxResult.No:
                        done = true;
                        break;
                }
            }

        }

        private void DoActiomBotton_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO Bar
        }

        private void DoActiomBotton_Click_2(object sender, RoutedEventArgs e)
        {
           
            switch (field)
            {
                case 1://call
                    int amount = 0;
                    string temp = InputForActionTextBox.Text;
                    bool isValid = int.TryParse(temp, out amount);
                    if (!isValid)
                    {
                        Dispatcher.BeginInvoke((Action)(() => MessageBox.Show("Invalid Amount")));
                    }
                     _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                   break;
                case 2://raise
                    int amountw = 0;
                    string tempw = InputForActionTextBox.Text;
                    bool isValidw = int.TryParse(tempw, out amount);
                    if (!isValidw)
                    {
                        Dispatcher.BeginInvoke((Action)(() => MessageBox.Show("Invalid Amount")));
                     }
                     _logic.NotifyChosenMove(CommunicationMessage.ActionType.Bet, amount, RoomId);
                   
                    break;
                case 3://fold
                    int amountd = -1;
                     _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Fold, amountd, RoomId);
                    
                    break;
                case 4://check
                    int amounte = 0;
                    _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amounte, RoomId);
                    break;
            }
        }

        private int field;
       
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            field = 1; //call
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            field = 2;//raise
        }

        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            field = 3;//fold
        }

        private void ComboBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {
            field = 4;//cheack
        }

        private void ComboBoxItem_Selected_4(object sender, RoutedEventArgs e)
        {
            field = 5;//brodcast - as player
        }

        private void ComboBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            field = 6;//whisper as player
        }

        private void ListViewPublicCards_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void publicCard_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected_6(object sender, RoutedEventArgs e)
        {
            this.field = 7;//borcast spectetor
        }

        private void ComboBoxItem_Selected_7(object sender, RoutedEventArgs e)
        {
            this.field = 8;//whisper spectetor
        }

        private void SendChatButton_Click(object sender, RoutedEventArgs e)
        {
            switch (field)
            {
                case 5://brodcast Player
                    string msgToSend2 = InputForChat.Text;
                    if (!update.AllPlayerNames.Contains(_logic.user.name))
                    {
                        MessageBox.Show("You're NOT a player... SORRY");
                        break;
                    }
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend2, CommunicationMessage.ActionType.PlayerBrodcast);
                    break;
                case 6://whisper Player
                    string msgToSend = InputForChat.Text;
                    string reciverName = WhisperReceiverTextBox.Text;
                    if (!update.AllPlayerNames.Contains(_logic.user.name))
                    {
                        MessageBox.Show("You're NOT a player... SORRY");
                        break;
                    }
                    if (update.AllPlayerNames.Contains(reciverName) || update.AllSpectatorNames.Contains(reciverName))
                    {
                        if (update.AllPlayerNames.Contains(reciverName))
                        {
                            _logic.SendChatMsg(RoomId, reciverName, msgToSend,
                                CommunicationMessage.ActionType.PlayerWhisper);
                        }
                        else
                        {
                            _logic.SendChatMsg(RoomId, reciverName, msgToSend,
                                CommunicationMessage.ActionType.SpectetorWhisper);
                        }
                       
                    }
                    else
                    {
                        MessageBox.Show("There is no such user... Sorry");
                    }
                    break;
                case 7: //broadcast spec
                    string msgToSend3 = InputForChat.Text;
                    if (!update.AllSpectatorNames.Contains(_logic.user.name))
                    {
                        MessageBox.Show("You're NOT a spectatore... SORRY");
                        break;
                    }
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend3,
                        CommunicationMessage.ActionType.SpectetorBrodcast);
                    break;
                case 8: //whisper spec
                    string msgToSend4 = InputForChat.Text;
                    string reciverNameSpec = WhisperReceiverTextBox.Text;
                    if (!update.AllSpectatorNames.Contains(_logic.user.name))
                    {
                        MessageBox.Show("You're NOT a spectatore... SORRY");
                        break;
                    }
                    if (update.AllSpectatorNames.Contains(reciverNameSpec))
                    {
                        _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend4,
                            CommunicationMessage.ActionType.SpectetorWhisper);
                    }
                    else
                    {
                        MessageBox.Show("There is no such spectator... Sorry");
                    }

                    break;
            }
        }

        public void LeaveOkay(GameDataCommMessage gd)
        {
            if (gd.IsSucceed)
            {
                MessageBox.Show("Leave Game Okay. BYE BYE");
                Dispatcher.Invoke(this.Close);
            }
            else
            {
                MessageBox.Show("Something went wrong. Leave Game Fail");
            }
        }

        public void LeaveAsPlayerOk(GameDataCommMessage msgGameData)
        {
            if (msgGameData.IsSucceed)
            {

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("BYE BYE " + _logic.user.username + " see you soon!" );
                    this.Close();
                });
            }
            else
            {
                MessageBox.Show("Leave the game faild, please try again");
            }
        }

        public void LeaveAsSpectetorOk(GameDataCommMessage msgGameData)
        {
            if (msgGameData.IsSucceed)
            {

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("BYE BYE " + _logic.user.username + " see you soon!");
                    this.Close();
                });
            }
            else
            {
                MessageBox.Show("Leave the game faild, please try again");
            }
        }
    }
}
