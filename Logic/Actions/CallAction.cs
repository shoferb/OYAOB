using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.User;

namespace TexasHoldem.Logic.Actions
{
    public class CallAction : UserAction
    {
        public CallAction(Card card1, Card card2, int playerPosition, Role playerRole, int amount,
            Player player, int roomID, int gameNumber) :
            base(card1, card2, playerPosition, playerRole, amount, player, roomID, gameNumber)
        {
        }

        public override String DoAction()
        {
            return ToString();
        }

        public override String ToString()
        {
            return String.Format("RoomID: {0}, GameNumber: {1}, PlayerName: {2}",
                        _roomID , _gameNumber, _player.ToString());  //need to add various fields of Player here
        }

    }
}
