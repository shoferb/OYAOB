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
           
        }

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
                foreach (string aString in AllPlayerNames)
                {
                    // Construct the ListViewItem object
                    ListViewItem item = new ListViewItem();

                    // Set the Text property to the cursor name.
                    item.Content = aString;

                    // Set the Tag property to the cursor.
                    item.Tag = aString;

                    // Add the ListViewItem to the ListView.
                    ListViewPlayers.Items.Add(item);
                }
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
            if (msg.PlayerCards != null)
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
                foreach (Card aCard in TableCards)
                {
                    // Construct the ListViewItem object
                    ListViewItem item = new ListViewItem();

                    // Set the Text property to the cursor name.
                    item.Content = aCard.ToString();

                    // Set the Tag property to the cursor.
                    item.Tag = aCard;

                    // Add the ListViewItem to the ListView.
                    ListViewPublicCards.Items.Add(item);
                }
            }
            
            this.TotalChips = msg.TotalChips;
            this.ChipAmountLabel.Content = msg.TotalChips;
           
        }

        private void DoActiomBotton_Click(object sender, RoutedEventArgs e)
        {

            string action = ActionChosenComboBox.Text;
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
                        string msg =string.Concat("*GAME MESSAGE* ",_logic.user.username,": ",action,"with amount of ",amount);
                        _logic.SendChatMsg(this.RoomId, _logic.user.username, msg, CommunicationMessage.ActionType.PlayerBrodcast);
                    }
                }
                if (action.Equals("Check"))
                {
                    int amount = 0;
                    bool ans =_logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        string msg = string.Concat("*GAME MESSAGE* ", _logic.user.username, ": ", action);
                        _logic.SendChatMsg(this.RoomId, _logic.user.username, msg, CommunicationMessage.ActionType.PlayerBrodcast);
                    }
                }
                if (action.Equals("Fold"))
                {
                    int amount = -1;
                    bool ans =_logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                    if (ans)
                    {
                        string msg = string.Concat("*GAME MESSAGE* ", _logic.user.username, ": ", action);
                        _logic.SendChatMsg(this.RoomId, _logic.user.username, msg, CommunicationMessage.ActionType.PlayerBrodcast);
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
            _logic.Logout(_logic.user.username, _logic.user.password);
        }

        private void DoActiomBotton_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO Bar
        }
    }
}
