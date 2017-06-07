using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldem.Logic
{
    public class ServerToClientSender
    {
        private readonly MessageEventHandler _eventHandler;

        public ServerToClientSender(MessageEventHandler messageEventHandler, 
            GameCenter gc, SystemControl sys, LogControl log, ReplayManager replay)
        {
            _eventHandler = messageEventHandler;
        }

        public void SendMessageToClient(IGame room, GameDataCommMessage gmData, List<int> idsToSend, bool useCommunication)
        {
            if (useCommunication)
            {
                gmData.TableCards = room.GetPublicCards();
                foreach (int id in idsToSend)
                {
                    if (id == gmData.UserId && gmData.action == CommunicationMessage.ActionType.Join)
                    {
                        continue;
                    }
                    
                    Player player = room.GetPlayersInRoom().Find(p => p.user.Id() == id);
                    gmData.PlayerCards[0] = player._firstCard;
                    gmData.PlayerCards[1] = player._secondCard;
                    gmData.TotalChips = player.TotalChip;

                    gmData.UserId = id; //id of the user to send 
                    _eventHandler.SendGameDataToClient(gmData);
                }
            }
        }

        public long GetSessionIdByUserId(int userId)
        {
            return _eventHandler.GetSessionIdByUserId(userId);
        }
    }
}
