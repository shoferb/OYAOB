using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public abstract class GamePrefDecorator : GameRoom 
    {
        public GamePrefDecorator(List<Player> players, int buttonPos) : base(players, buttonPos)
        {
        }
    }
}
