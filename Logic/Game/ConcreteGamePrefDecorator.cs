 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGamePrefDecorator : GamePrefDecorator
    {
        public ConcreteGamePrefDecorator(List<Player> players, int startingChip) : base(players, startingChip)
        {
        }

        public override List<Player> _players { get; set; }
        public override int _buttonPos { get; set; }
        public override int _maxCommitted { get; set; }
        public override int _actionPos { get; set; }
        public override int _potCount { get; set; }
        public override int _bb { get; set; }
        public override int _sb { get; set; }
        public override Deck _deck { get; set; }
        public override ConcreteGameRoom.HandStep _handStep { get; set; }
        public override List<Card> _publicCards { get; set; }
        public override bool _isGameOver { get; set; }
        public override List<Tuple<int, List<Player>>> _sidePots { get; set; }
        public override int _gameRoles { get; set; }
        public override void AddNewPublicCard()
        {
            throw new NotImplementedException();
        }

        public override Player NextToPlay()
        {
            throw new NotImplementedException();
        }

        
        public override int ToCall()
        {
            throw new NotImplementedException();
        }

        public override void UpdateGameState()
        {
            throw new NotImplementedException();
        }

        public override void ClearPublicCards()
        {
            throw new NotImplementedException();
        }

        public override void UpdateMaxCommitted()
        {
            throw new NotImplementedException();
        }

        public override void EndTurn()
        {
            throw new NotImplementedException();
        }

        public override void ResetActionPos()
        {
            throw new NotImplementedException();
        }

        public override void MoveChipsToPot()
        {
            throw new NotImplementedException();
        }

        public override int PlayersInHand()
        {
            throw new NotImplementedException();
        }

        public override int PlayersAllIn()
        {
            throw new NotImplementedException();
        }

        public override bool AllDoneWithTurn()
        {
            throw new NotImplementedException();
        }

        public override bool newSplitPot(Player allInPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
