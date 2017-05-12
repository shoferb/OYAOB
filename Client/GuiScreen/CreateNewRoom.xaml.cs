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

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for CreateNewRoom.xaml
    /// </summary>
    public partial class CreateNewRoom : Window
    {
        private GameMode _mode;
        private int _minBet;
        private int _chipPolicy;
        private int _buyInPolicy;
        private bool _canSpectate;
        private int _minPlayer;
        private int _maxPlayers;

        public CreateNewRoom()
        {
            InitializeComponent();
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

        }
    }
}
