using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TexasHoldem.Logic.Users;

namespace TexasHoldem.DatabaseProxy
{
    public class PlayerXML
    {
        public bool isPlayerActive { get; set; }
        public string name { get; set; }
        public int TotalChip { get; set; }
        public int RoundChipBet { get; set; } // the number of chips player use in this round
        public bool PlayedAnActionInTheRound { get; set; }
        public Card _firstCard { get; set; }
        public Card _secondCard { get; set; }
        public List<Card> _publicCards = new List<Card>();

        public int userId { get; set; }
        public int roomId { get; set; }
        public PlayerXML(Logic.Users.Player p)
        {
            isPlayerActive = p.isPlayerActive;
            name = p.name;
            TotalChip = p.TotalChip;
            RoundChipBet = p.RoundChipBet;
            PlayedAnActionInTheRound = p.PlayedAnActionInTheRound;
            _firstCard = p._firstCard;
            _secondCard = p._secondCard;
            _publicCards = p._publicCards;//new List<Card>();
            userId = p.user.Id();
            roomId = p.roomId;
    }
    }


}
