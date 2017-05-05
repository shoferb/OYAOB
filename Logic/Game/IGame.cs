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
         List<Player> Players { get; set; }
         int Id { get; set; }
         List<Spectetor> Spectatores { get; set; }
         int DealerPos { get; set; }
         int maxBetInRound { get; set; }
         int ActionPos { get; set; }
         int PotCount { get; set; }
         int Bb { get; set; }
         int Sb { get; set; }
         Deck Deck { get; set; }
         GameRoom.HandStep Hand_Step { get; set; }
         List<Card> Cards { get; set; }
         bool IsActiveGame { get; set; }
         List<Tuple<int, List<Player>>> SidePots { get; set; }
         GameReplay GameReplay { get; set; }     
         int VerifyAction { get; set; }       
         bool IsTestMode { get; set; }     
         int MaxRaiseInThisRound { get; set; } //מה המקסימום raise / bet שיכול לבצע בסיבוב הנוכחי 
         int MinRaiseInThisRound { get; set; } //המינימום שחייב לבצע בסיבוב הנוכחי
         int LastRaise { get; set; }  //change to maxCommit
         Thread RoomThread { get; set; }    
        List<HandEvaluator> FindWinner(List<Card> table, List<Player> playersLeftInHand);
        bool AddPlayerToRoom(int userId);
        bool AddSpectetorToRoom(int userId);
        bool RemovePlayerFromRoom(int userId);
        bool RemoveSpectetorFromRoom(int userId);

    }
}
