using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public abstract class GameRoom
    {
        public abstract List<Player> _players { get; set; }
        public abstract int _buttonPos { get; set; }
        public abstract int _maxCommitted { get; set; }
        public abstract int _actionPos { get; set; }
        public abstract int _potCount { get; set; }
        public abstract int _bb { get; set; }
        public abstract int _sb { get; set; }
        public abstract Deck _deck { get; set; }
        public abstract ConcreteGameRoom.HandStep _handStep { get; set; }
        public abstract List<Card> _publicCards { get; set; }
        public abstract bool _isGameOver { get; set; }
        public abstract List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public abstract int _gameRoles { get; set; }
        public GameRoom(List<Player> players, int buttonPos)
        {
            //throw new NotImplementedException();
        }

        public abstract void AddNewPublicCard();
        public abstract Player NextToPlay();
        public abstract int ToCall();
        public abstract void UpdateGameState();
        public abstract void ClearPublicCards();
        public abstract void UpdateMaxCommitted();
        public abstract void EndTurn();
        public abstract void ResetActionPos();
        public abstract void MoveChipsToPot();
        public abstract int PlayersInHand();
        public abstract int PlayersAllIn();
        public abstract bool AllDoneWithTurn();
        public abstract void newSplitPot(Player allInPlayer);
    }
}
