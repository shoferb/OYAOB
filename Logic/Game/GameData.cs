using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldem.Logic.Game
{
    public class GameData
    {
        private List<Card> _publicCards;
        private int _chips;
        private int _pot;
        private List<Player> _players;
        private string _dealerName;
        private string _BbName;
        private string _SbName;

        public GameData(List<Card> cardses, int chips, int pot, List<Player> allPlayers, string dealerName,
            string bbName, string sbName)
        {
            this._publicCards = cardses;
            this._chips = chips;
            this._pot = pot;
            this._players = allPlayers;
            this._dealerName = dealerName;
            this._BbName = bbName;
            this._SbName = sbName;

        }

       public List<Card> getPublicCard()
        {
           return _publicCards;
        }

        public int getPotSize()
        {
            return _pot;
        }
        public int getChips()
        {
            return _chips;
        }
        public List<String> getPlayersNames()
        {
            List<string> playersNames = new List<string>();
            foreach (Player p in _players)
            {
               playersNames.Add(p.name); 
            }
            return playersNames;
        }

        public string getDealer()
        {
            return _dealerName;
        }

        public string GetBbPlayer()
        {
            return _BbName;
        }

        public string GetSbPlayer()
        {
            return _SbName;
        }

    }
}
