﻿using System;
using System.Collections.Generic;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace Client.Logic
{
    public class ResponseNotifier : IResponseNotifier
    {
        private readonly List<Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>> _messagesSentObserver;
        private readonly ClientLogic _logic;

        public ResponseNotifier(List<Tuple<CommunicationMessage, bool, bool, ResponeCommMessage>> messagesSentObserver, 
            ClientLogic logic)
        {
            _messagesSentObserver = messagesSentObserver;
            _logic = logic;
        }

        public bool GeneralCase(GameDataCommMessage msgGameData)
        {
            _logic.GameUpdateReceived(msgGameData);
            return true;
        }

        public bool NotifyLogin(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                LoginResponeCommMessage loginResp = (LoginResponeCommMessage) msg;
                if (((LoginCommMessage) loginResp.OriginalMsg).IsLogin)
                {
                    return _logic.LoginRespReceived(loginResp);
                }
                return _logic.LogoutRespReceived(loginResp);
            }
            return false;
        }

        public bool ObserverNotify(ResponeCommMessage msg)
        {
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toEdit =
                _messagesSentObserver.Find(x => x.Item1.Equals(msg.OriginalMsg));
            if (toEdit != null)
            {
                _messagesSentObserver.Remove(toEdit);
                var toAdd = new Tuple<CommunicationMessage, bool, bool,
                    ResponeCommMessage>(toEdit.Item1, true, msg.Success, msg);
                _messagesSentObserver.Add(toAdd);
                return true;
            }
            return false;
        }

        public bool NotifyChat(ResponeCommMessage msg)
        {
            if (((ChatCommMessage)msg.OriginalMsg).ChatType == CommunicationMessage.ActionType.PlayerWhisper ||
                ((ChatCommMessage)msg.OriginalMsg).ChatType == CommunicationMessage.ActionType.SpectetorWhisper)
            {
                return true;
            }
            return GeneralCase(msg.GameData);
        }

        public bool NotifyReturnAsPlayer(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.PlayerReturnsToGame(msg.GameData);
                return true;
            }
            return false;
        }

        public bool NotifyReturnAsSpec(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.SpecReturnsToGame(msg.GameData);
                return true;
            }
            return false;
        }

        public bool NotifySearch(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                SearchCommMessage original = (SearchCommMessage) msg.OriginalMsg;
                _logic.SearchResultRecived(((SearchResponseCommMessage)msg).Games, original.IsReturnToGame);
                return true;
            }
            return false;
        }

        public bool ReceivedJoin(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.JoinAsPlayerReceived((JoinResponseCommMessage) msg);
                return true;
            }
            return false;
        }

        public bool ReceivedSpectate(ResponeCommMessage msg)
        {
            _logic.JoinAsSpectatorReceived((JoinResponseCommMessage) msg);
            return true;
        }

        public bool NotifyAction(ResponeCommMessage msg)
        {
            ActionCommMessage original = msg.OriginalMsg as ActionCommMessage;
            if (original != null)
            {
                switch (original.MoveType)
                {
                    case CommunicationMessage.ActionType.Join:
                        ReceivedJoin((JoinResponseCommMessage)msg);
                        break;
                    case CommunicationMessage.ActionType.Spectate:
                        ReceivedSpectate((JoinResponseCommMessage)msg);
                        break;
                        case CommunicationMessage.ActionType.Leave:
                            ReceivedLeave(msg);
                            break;
                    case CommunicationMessage.ActionType.SpectatorLeave:
                            ReceivedSpectetorLeave(msg);
                            break;
                    default:
                        GeneralCase(msg.GameData);
                        break;
                }
                return true;
            }
            return false;
        }

        private bool ReceivedLeave(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.LeaveAsPlayer(msg);
                return true;
            }
            return false;
        }

        private bool ReceivedSpectetorLeave(ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.LeaveAsSpectetor(msg);
                return true;
            }
            return false;
        }
        public bool Default(ResponeCommMessage msg)
        {
            if (msg != null)
            {
                GeneralCase(msg.GameData);
                return true;
            }
            return false;
        }
    }
}
