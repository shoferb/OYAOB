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
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for CreateNewRoom.xaml
    /// </summary>
    public partial class CreateNewRoom : Window
    {
        private GameMode _mode;
        private int _minBet=-1;
        private int _chipPolicy=-1;
        private int _buyInPolicy=-1;
        private bool _canSpectate;
        private int _minPlayer=-1;
        private int _maxPlayers=-1;
        private ClientLogic _logic;
        private Window parentScreen;


        public CreateNewRoom(Window p,ClientLogic cl)
        {
            InitializeComponent();
            _logic = cl;
            parentScreen = p;
        }

        private void GameModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(GameModeComboBox.Text.Equals("Pot Limit"))
            {
                this._mode = GameMode.PotLimit;
            }
            else if (GameModeComboBox.Text.Equals("No Limit"))
            {
                this._mode = GameMode.NoLimit;
            }
            else if (GameModeComboBox.Text.Equals("Limit"))
            {
                this._mode = GameMode.Limit;
            }
        }

       

        private void SpectatorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameModeComboBox.Text.Equals("No"))
            {
                this._canSpectate=false;
            }
            else if (GameModeComboBox.Text.Equals("Yes"))
            {
                this._canSpectate = true;
            }
        }

        private void MinBettextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = MinBettextBox.Text;
            bool isValid = int.TryParse(temp, out _minBet);
            if (!isValid)
            {
                MessageBox.Show("Invalid Minimum Bet input");
            }
        }

        private void ChipPoltextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = ChipPoltextBox.Text;
            bool isValid = int.TryParse(temp, out _chipPolicy);
            if (!isValid)
            {
                MessageBox.Show("Invalid Chip Policy input");
            }

        }

        private void BuyInPoltextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = BuyInPoltextBox.Text;
            bool isValid = int.TryParse(temp, out _buyInPolicy);
            if (!isValid)
            {
                MessageBox.Show("Invalid Buy-In Policy input");
            }
        }

        private void MinPlayerstextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = MinPlayerstextBox.Text;
            bool isValid = int.TryParse(temp, out _minPlayer);
            if (!isValid)
            {
                MessageBox.Show("Invalid Minimum Players input");
            }
        }

        private void MaxPlayerstextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = MaxPlayersTextBox.Text;
            bool isValid = int.TryParse(temp, out _maxPlayers);
            if (!isValid)
            {
                MessageBox.Show("Invalid Maximum Players input");
            }
        }

        private void CreateBotton_Click(object sender, RoutedEventArgs e)
        {
            if (GameModeComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Please Choose Game Mode");
                return;
            }
            if (_minPlayer == -1)
            {
                MessageBox.Show("Please enter Minimum Playes");
                return;
            }
            if (_maxPlayers == -1)
            {
                MessageBox.Show("Please enter Maximum Playes");
                return;
            }
            if (_chipPolicy == -1)
            {
                MessageBox.Show("Please enter Chip Policy");
                return;
            }
            if (_buyInPolicy== -1)
            {
                MessageBox.Show("Please enter Buy-In Policy");
                return;
            }
            if (SpectatorsComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose Spectetors Allowed");
                return;
            }
            if (_minBet == -1)
            {
                MessageBox.Show("Please enter Chip Policy");
                return;
            }
            GameDataCommMessage newRoom = _logic.CreateNewRoom(_mode, _minBet, _chipPolicy, _buyInPolicy, _canSpectate, _minPlayer, _maxPlayers);
            if (newRoom!=null)
            {
                MessageBox.Show("New Room sucssesfully Created, Enjoy!");
                GameScreen newGameWindow = new GameScreen(_logic);
                
                newGameWindow.UpdateGame(newRoom);
                _logic.AddNewRoom(newGameWindow);
                newGameWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("New room creation failed!");
            }
        }
        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            parentScreen.Show();
            this.Hide();
        }
    }
}
