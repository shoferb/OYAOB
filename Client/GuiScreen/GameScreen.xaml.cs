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
            ComboBoxItem chatMsgItem = new ComboBoxItem();
            chatMsgItem.Content = "Send A New Chat Message";
            ActionChosenComboBox.Items.Add(chatMsgItem);

            this.RoomId = msg.RoomId;
            RoomNum.Content = string.Concat(RoomNum.Content, RoomId);
            this.SbName = msg.SbName;
            this.SBNameLabel.Content = msg.SbName;
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
            this.BbName = msg.BbName;
            this.BBNameLabel.Content = msg.BbName;
            this.CurrPlayerTurn = msg.CurrPlayerTurn;
            this.CurrTurnNameLabel.Content = msg.CurrPlayerTurn;
            this.DealerName = msg.DealerName;
            this.DealerNameLabel.Content = msg.DealerName;
            this.PlayerCards = msg.PlayerCards;
            this.Card1Labek.Content = string.Concat(Card1Labek.Content, (msg.PlayerCards[0]).ToString());
            this.Card2Label.Content = string.Concat(Card2Label.Content, (msg.PlayerCards[1]).ToString());
            this.PotSize = msg.PotSize;
            this.PotAmountLabel.Content = msg.PotSize;
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
                    _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                }
                if (action.Equals("Check"))
                {
                    int amount = 0;
                    _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                }
                if (action.Equals("Fold"))
                {
                    int amount = -1;
                    _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Bet, amount, RoomId);
                }
                if (action.Equals("Send A New Chat Message"))
                {
                //TODO
                }
            }

        }

        private void LeaveBotton_Click(object sender, RoutedEventArgs e)
        {
            _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.Leave, -1, this.RoomId);

        }

        private void StartTheGameBTN_Click(object sender, RoutedEventArgs e)
        {
            _logic.NotifyChosenMove(TexasHoldemShared.CommMessages.CommunicationMessage.ActionType.StartGame, -1, this.RoomId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _logic.Logout(_logic.user.username, _logic.user.password);
        }
    }
}
