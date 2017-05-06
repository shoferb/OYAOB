using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game.Evaluator;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
     public interface IGame
    {
        int Id { get; set; }
        List<HandEvaluator> FindWinner(List<Card> table, List<Player> playersLeftInHand);
        bool AddPlayerToRoom(int userId);
        bool AddSpectetorToRoom(int userId);
        bool RemovePlayerFromRoom(int userId);
        bool RemoveSpectetorFromRoom(int userId);
    }
}
