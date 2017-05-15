using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;

namespace TexasHoldem.Logic.GameControl
{
    public interface IGameCenter
    {
        bool DoAction(IUser user, CommunicationMessage.ActionType action, int amount, int roomId);
        List<Player> getPlayersInRoom(int roomId);
        List<Spectetor> getSpectatorsInRoom(int roomId);
        IGame GetRoomById(int roomId);
        bool CreateNewRoomWithRoomId(int roomId, IUser user, int startingChip, bool canSpectate, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet);
        int GetNextIdRoom();

       IGameCenter Instance {get;}
        List<IGame> GetAllActiveGamesAUserCanJoin(IUser user);
        List<IGame> GetAllActiveGame();
        List<IGame> GetAllGames();
        List<IGame> GetAllSpectetorGame();
        List<IGame> GetAllGamesByPotSize(int potSize);
        List<IGame> GetGamesByGameMode(GameMode gm);
        List<IGame> GetGamesByBuyInPolicy(int buyIn);
        List<IGame> GetGamesByMinPlayer(int min);
        List<IGame> GetGamesByMinBet(int minBet);
        List<IGame> GetGamesByStartingChip(int startingChip);
        List<IGame> GetGamesByMaxPlayer(int max);
        bool CanSendPlayerBrodcast(IUser user, int roomId);
        bool CanSendSpectetorBrodcast(IUser user, int roomId);
        bool CanSendPlayerWhisper(IUser sender, IUser reciver, int roomId);
        bool CanSendSpectetorWhisper(IUser sender, IUser reciver, int roomId);

        }
}
