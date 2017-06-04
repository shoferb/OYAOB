﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
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

        public void SendMessageToClient(GameDataCommMessage gmData, List<int> idsToSend, bool useCommunication)
        {
            if (useCommunication)
            {
                foreach (int id in idsToSend)
                {
                    if (id == gmData.UserId && gmData.action == CommunicationMessage.ActionType.Join)
                    {
                        continue;
                    }
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
