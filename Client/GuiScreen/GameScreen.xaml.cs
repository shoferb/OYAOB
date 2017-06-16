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


        public GameScreen(ClientLogic c)
        {
            InitializeComponent();
            _logic = c;
            UserID.Content = _logic.user.id;
            UserName.Content = _logic.user.username;
            string path = _logic.user.avatar;
            Avatar.Source = new BitmapImage(new Uri(@path, UriKind.Relative));

        }
       
        public void UpdateGame(GameDataCommMessage msg)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                UserID.Content = _logic.user.id;
                UserName.Content = _logic.user.username;
                string path = _logic.user.avatar;
                Avatar.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                if (_logic.user.username.Equals(msg.CurrPlayerTurn))
                {
                    //TODO BAR?!
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
                if ((msg.PlayerCards[0] != null) || (msg.PlayerCards[1] != null))
                {
                    this.PlayerCards = msg.PlayerCards;
                    string pre1 = "First Card is :    ";
                    string pre2 = "Second Card is : ";
                    this.Card1Labek.Content = string.Concat(pre1, (msg.PlayerCards[0]).ToString());
                    this.Card2Label.Content = string.Concat(pre2, (msg.PlayerCards[1]).ToString());
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

                this.TotalChips = msg.TotalChips;
                this.ChipAmountLabel.Content = msg.TotalChips;
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
                    else if (msg.Action.Equals(CommunicationMessage.ActionType.Leave))
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.ActionPlayerName, " left the game.");
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

                }

            }));
        }

        private void DoActiomBotton_Click(object sender, RoutedEventArgs e)
        {

            string action = ActionChosenComboBox.SelectedValue.ToString();
            if (action.Equals(""))
            {
                MessageBox.Show("please Choose an action");
            }
            else
            {
                if ((action.Equals("Call")) || (action.Equals("Raise")))
                {
                    int amount = 0;
                    string temp = InputForActionTextBox.Text;
                    bool isValid = int.TryParse(temp, out amount);
                    if (!isValid)
                    {
                        MessageBox.Show("Invalid Amount");
                    }
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                }
                if (action.Equals("Check"))
                {
                    int amount = 0;
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                }
                if (action.Equals("Fold"))
                {
                    int amount = -1;
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                }
                if (action.Equals("Send A New Broadcast Chat Message Player"))
                {
                    string msgToSend = InputForActionTextBox.Text; 
                     _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.PlayerBrodcast);                      
                }
                if (action.Equals("Send A New Whisper Chat Message Player"))
                {
                    string msgToSend = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend,
                        CommunicationMessage.ActionType.PlayerWhisper);
                }
                if (action.Equals("Send A New Broadcast Chat Message Spectator"))
                {
                    string msgToSend = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.SpectetorBrodcast);
                }
                if (action.Equals("Send A New Whisper Chat Message Spectator"))
                {
                    string msgToSend = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.SpectetorWhisper);
                }
            }

        }
   
        public void AddChatMsg(ChatResponceCommMessage msg)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
            new Action(delegate ()
            {
                
                if (msg.chatType != CommunicationMessage.ActionType.PlayerBrodcast)
                {
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = string.Concat("Whisper message from ", msg.senderngUsername, ": ", msg.msgToSend);
                    chatListView.Items.Add(toAdd);
                }
                else
                {
                    ListViewItem toAdd = new ListViewItem();
                    toAdd.Content = string.Concat("Broadcast message from ", msg.senderngUsername, ": ", msg.msgToSend);
                    this.chatListView.Items.Add(toAdd);
                }
            }
            ));


           
           
        }

        private void LeaveBotton_Click(object sender, RoutedEventArgs e)
        {
            bool res = _logic.LeaveTheGame(this.RoomId);
            if (res)
            {
                MessageBox.Show("game leave OK!!!");
                this.Close();
            }
            else
            {
                MessageBox.Show("game leave FAIL!!!");
            }
        }

        private void StartTheGameBTN_Click(object sender, RoutedEventArgs e)
        {
            bool res = _logic.StartTheGame(this.RoomId);
            if (res)
            {
                MessageBox.Show("game started :)");
            }
            else
            {
                MessageBox.Show("game start FAIL :(");
            }
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
                                
                                WellcomeScreen wellcomeScreen = new WellcomeScreen();

                                wellcomeScreen.Show();
                                this.Close();
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
            string Data = InputForActionTextBox.Text;

            switch (field)
            {
                case 1://call
                    int amount = 0;
                    string temp = InputForActionTextBox.Text;
                    bool isValid = int.TryParse(temp, out amount);
                    if (!isValid)
                    {
                        MessageBox.Show("Invalid Amount");
                    }
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                    break;
                case 2://raise
                    int amountw = 0;
                    string tempw = InputForActionTextBox.Text;
                    bool isValidw = int.TryParse(tempw, out amount);
                    if (!isValidw)
                    {
                        MessageBox.Show("Invalid Amount");
                    }
                    bool answ = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (answ)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                    break;
                case 3://fold
                    int amountd = -1;
                    bool ansd = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Fold, amountd, RoomId);
                    if (ansd)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                    break;
                case 4://cheack
                    int amounte = 0;
                    bool anse = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amounte, RoomId);
                    if (anse)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                    break;
                case 5://brodcast Player
                    string msgToSend2 = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend2, CommunicationMessage.ActionType.PlayerBrodcast);             
                    break;
                case 6://whisper Player
                    string msgToSend = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend,
                        CommunicationMessage.ActionType.PlayerWhisper);
                    break;
                case 7: //broadcast spec
                    string msgToSend3 = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend3,
                        CommunicationMessage.ActionType.PlayerWhisper);
                    break;
                case 8: //whisper spec
                    string msgToSend4 = InputForActionTextBox.Text;
                    _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend4,
                       CommunicationMessage.ActionType.PlayerWhisper);
                    break;
            }/*
            string action = ActionChosenComboBox.SelectedValue.ToString();
            if (action.Equals(""))
            {
                MessageBox.Show("please Choose an action");
            }
            else
            {
                if ((action.Equals("Call")) || (action.Equals("Raise")))
                {
                    int amount = 0;
                    string temp = InputForActionTextBox.Text;
                    bool isValid = int.TryParse(temp, out amount);
                    if (!isValid)
                    {
                        MessageBox.Show("Invalid Amount");
                    }
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                }
                if (action.Equals("Check"))
                {
                    int amount = 0;
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                }
                if (action.Equals("Fold"))
                {
                    int amount = -1;
                    bool ans = _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        MessageBox.Show("Action Succeeded");
                    }
                    else
                    {
                        MessageBox.Show("Action Failed");
                    }
                }
                if (action.Equals("Send A New Broadcast Chat Message"))
                {
                    string msgToSend = InputForActionTextBox.Text;
                    if (SpecOrPlay == true)
                    {

                        bool ans = _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.PlayerBrodcast);
                        if (!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                    else
                    {
                        bool ans = _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.SpectetorBrodcast);
                        if (!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                }
                if (action.Equals("Send A New Whisper Chat Message"))
                {
                    string msgToSend = InputForActionTextBox.Text;
                    string receiverName = WhisperReceiverTextBox_Copy.Text;
                    if (SpecOrPlay == true)
                    {

                        bool ans = _logic.SendChatMsg(RoomId, receiverName, msgToSend, CommunicationMessage.ActionType.PlayerWhisper);
                        if (!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                    else
                    {
                        bool ans = _logic.SendChatMsg(RoomId, receiverName, msgToSend, CommunicationMessage.ActionType.SpectetorWhisper);
                        if (!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                }
            }*/
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
            field = 5;//brodcast
        }

        private void ComboBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            field = 6;//whisper
        }

        private void ListViewPublicCards_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void publicCard_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
