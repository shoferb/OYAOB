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
using TexasHoldem;
using TexasHoldem.GuiScreen;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ServerToClient;

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
        //todo refrash name
        /*
         *  UserID.Content = _logic.user.id;
            UserName.Content = _logic.user.username;
            string path = _logic.user.avatar;
            Avatar.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
            */
        public void UpdateGame(GameDataCommMessage msg)
        {
           
            ActionChosenComboBox.Items.Clear();
            if(_logic.user.name.Equals(CurrPlayerTurn))
            {
                ComboBoxItem callItem = new ComboBoxItem();
                callItem.Content = "Call";
                ActionChosenComboBox.Items.Add(callItem);
                ComboBoxItem raiseItem = new ComboBoxItem();
                raiseItem.Content = "Raise";
                ActionChosenComboBox.Items.Add(raiseItem);
                ComboBoxItem checkItem = new ComboBoxItem();
                checkItem.Content = "Check";
                ActionChosenComboBox.Items.Add(checkItem);
                ComboBoxItem foldItem = new ComboBoxItem();
                foldItem.Content = "Fold";
                ActionChosenComboBox.Items.Add(foldItem);
            }
            ComboBoxItem broadcastChatMsgItem = new ComboBoxItem();
            broadcastChatMsgItem.Content = "Send A New Broadcast Chat Message";
            ActionChosenComboBox.Items.Add(broadcastChatMsgItem);
            ComboBoxItem whisperchatMsgItem = new ComboBoxItem();
            whisperchatMsgItem.Content = "Send A New Whisper Chat Message";
            ActionChosenComboBox.Items.Add(whisperchatMsgItem);

            this.RoomId = msg.RoomId;
            RoomNum.Content = string.Concat(RoomNum.Content, RoomId);
            if (msg.SbName != null)
            {
                this.SbName = msg.SbName;
                this.SBNameLabel.Content = msg.SbName;
            }
            if (msg.AllPlayerNames != null)
            {
                this.AllPlayerNames = msg.AllPlayerNames;
                List<ViewObj> players = new List<ViewObj>();
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
            if (msg.BbName != null)
            {
                this.BbName = msg.BbName;
                this.BBNameLabel.Content = msg.BbName;
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
            if ((msg.PlayerCards[0] != null)||(msg.PlayerCards[1]!=null))
            {
                this.PlayerCards = msg.PlayerCards;
                this.Card1Labek.Content = string.Concat(Card1Labek.Content, (msg.PlayerCards[0]).ToString());
                this.Card2Label.Content = string.Concat(Card2Label.Content, (msg.PlayerCards[1]).ToString());
            }
            
            this.PotSize = msg.PotSize;
            this.PotAmountLabel.Content = msg.PotSize;
            
            if (msg.TableCards != null)
            {
                this.TableCards = msg.TableCards;
                List<ViewObj> pCards = new List<ViewObj>();
                foreach (Card aCard in TableCards)
                {
                    if (aCard != null)
                    {
                        ViewObj toAdd = new ViewObj(aCard.ToString());
                        pCards.Add(toAdd);
                    }
                }
                ListViewPublicCards.ItemsSource = pCards;
            }
            
            this.TotalChips = msg.TotalChips;
            this.ChipAmountLabel.Content = msg.TotalChips;
            if (msg.isSucceed)
            {
                string msgToChat = "";
                if (msg.action.Equals(CommunicationMessage.ActionType.Bet))
                {
                    if (msg.betAmount == 0)
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " Checked");
                    }
                    else
                    {
                        msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " Bet with amount of ",
                            msg.betAmount);
                    }
                }
                else if (msg.action.Equals(CommunicationMessage.ActionType.Fold))
                {
                    msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " Folded.");
                }
                else if (msg.action.Equals(CommunicationMessage.ActionType.Join))
                {
                    msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " Joined the game.");
                }
                else if (msg.action.Equals(CommunicationMessage.ActionType.Leave))
                {
                    msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " left the game.");
                }
                else if (msg.action.Equals(CommunicationMessage.ActionType.StartGame))
                {
                    msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " started the game.");
                }
                else if (msg.action.Equals(CommunicationMessage.ActionType.CreateRoom))
                {
                    msgToChat = string.Concat("*GAME MESSAGE* ", msg.actionPlayerName, " created the room game.");
                }


                ListViewItem toAdd = new ListViewItem();
                toAdd.Content = msgToChat;
                this.chatListView.Items.Add(toAdd);

            }


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
                if ((action.Equals("Call"))|| (action.Equals("Raise")))
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
                    bool ans =_logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
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
                    bool ans =_logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
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
                       
                        bool ans=  _logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.PlayerBrodcast);
                        if (!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                    else
                    {
                        bool ans =_logic.SendChatMsg(RoomId, _logic.user.name, msgToSend, CommunicationMessage.ActionType.SpectetorBrodcast);
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

                       bool ans = _logic.SendChatMsg(RoomId,receiverName, msgToSend, CommunicationMessage.ActionType.PlayerWhisper);
                        if(!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                    else
                    {
                        bool ans =_logic.SendChatMsg(RoomId, receiverName, msgToSend, CommunicationMessage.ActionType.SpectetorWhisper);
                        if (!ans)
                        {
                            MessageBox.Show("Cant send this message!");
                        }
                    }
                }
                }

        }
        public void AddChatMsg(ChatResponceCommMessage msg)
        {
            if(msg.idReciver==this._logic.user.id)
            {
                ListViewItem toAdd = new ListViewItem();
                toAdd.Content = string.Concat("Whisper message from ", msg.senderngUsername, ": ", msg.msgToSend);
                this.chatListView.Items.Add(toAdd);
            }
            else
            {
                ListViewItem toAdd = new ListViewItem();
                toAdd.Content = string.Concat("Broadcast message from ", msg.senderngUsername, ": ", msg.msgToSend);
                this.chatListView.Items.Add(toAdd);
            }
        }

        private void LeaveBotton_Click(object sender, RoutedEventArgs e)
        {
            _logic.StartTheGame(this.RoomId);

        }

        private void StartTheGameBTN_Click(object sender, RoutedEventArgs e)
        {
            _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.StartGame, -1, this.RoomId);
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
                                done = true;
                                WellcomeScreen wellcomeScreen = new WellcomeScreen();

                                wellcomeScreen.Show();
                                this.Close();
                                this.Hide();
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
    }
}
