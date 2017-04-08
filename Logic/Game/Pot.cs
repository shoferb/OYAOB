using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class Pot
    {
        private int _totalPot;
        private int _smallBlind;
        private int _bigBlind;
        private int _maxRaisePutsIn;
        private PlayersList _playersInPot = new PlayersList();


        public Pot()
        {
            _totalPot = 0;
            _smallBlind = 0;
            _bigBlind = 0;
            _maxRaisePutsIn = 0;           
        }

        public Pot(int amount, PlayersList playersInPot)
        {
            this.Amount = amount;
            this._playersInPot = playersInPot;
            _smallBlind = 0;
            _bigBlind = 0;
            _maxRaisePutsIn = 0;
        }

        public int SmallBlind
        {
            get { return _smallBlind; }
            set { _smallBlind = value; }
        }
        public int BigBlind
        {
            get { return _bigBlind; }
            set { _bigBlind = value; }
        }
        public int Amount
        {
            get { return _totalPot; }
            set
            {
                if (value < 0)
                    value = 0;
                _totalPot = value;
            }
        }
        public PlayersList getPlayersInPot()
        {
            return _playersInPot;
        }
        //add player to pot
        public void AddPlayer(Player player)
        {
            if (!_playersInPot.Contains(player))
                _playersInPot.Add(player);
        }
        //add money to pot
        public void Add(int amount)
        {
            if (amount < 0)
                return;
            _totalPot += amount;
        }
        //get maximum amount in pot
        public int getMaximumAmountPutIn()
        {
            return _maxRaisePutsIn;
        }
        //set maximum amount in pot
        public void setMaximumAmount(int amount)
        {
            _maxRaisePutsIn = amount;
        }
    }
}
