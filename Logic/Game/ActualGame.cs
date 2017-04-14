using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    class ActualGame
    { 
    List<Player> _players;
    public HandOfPoker _handsOfPoker;
    public ConcreteGameRoom _state;
    public ActualGame(int numPlayers, int startingChips)
    {
        //TODO: when will be log class - ResetLog();
        int bb = startingChips / 100; //I'll change it later
        _players = new List<Player>(numPlayers);

        Random random = new Random();
        int buttonPos = random.Next(1, _players.Count);

        _state = new ConcreteGameRoom(_players, buttonPos);
        _state._bb = bb;

        _handsOfPoker = new HandOfPoker(_state);
            _handsOfPoker.NewHand(_state);
            _handsOfPoker.Play(_state);
        if (_players.Count == 1)
            GameEnds();
    }

    public void GameEnds()
    {
            //TODO: maybe ?
    }
}
}
