using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public abstract class GamePrefDecorator : GameRoom 
    {
        public GamePrefDecorator(List<Player> players, int startingChip, int ID, ReplayManager rm, bool isSpectetor, GameMode gameModeChosen) : base(players, startingChip, ID, rm, isSpectetor, gameModeChosen)
        {
        }
    }
}
