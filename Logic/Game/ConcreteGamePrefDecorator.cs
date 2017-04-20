 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class ConcreteGamePrefDecorator : GamePrefDecorator
    {
        public ConcreteGamePrefDecorator(List<Player> players, int startingChip, int ID, bool isSpectetor, GameMode gameModeChosen, int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBetInRoom) : base(players, startingChip, ID, isSpectetor, gameModeChosen, minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBetInRoom)
        {
        }

      

    }

}
