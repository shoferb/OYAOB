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
    List<Player> players;
    public HandOfPoker handsOfPoker;
    public ConcreteGameRoom state;
    public ActualGame(int numPlayers, int startingChips)
    {
        //TODO: when will be log class - ResetLog();
        int bb = startingChips / 100; //I'll change it later
        players = new List<Player>(numPlayers);

        Random random = new Random();
        int buttonPos = random.Next(1, players.Count);

        state = new ConcreteGameRoom(players, buttonPos);
        state.bb = bb;

        handsOfPoker = new HandOfPoker(state);
        if (players.Count == 1)
            GameEnds();
    }

    public void GameEnds()
    {

        Console.WriteLine("Player " + players[0].name + " wins!");
    }
}
}
