using System;
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

        public bool ObserverNotify(ResponeCommMessage msg)
        {
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toEdit =
                _messagesSentObserver.Find(x => x.Item1.Equals(msg.OriginalMsg));
            _messagesSentObserver.Remove(toEdit);
            var toAdd = new Tuple<CommunicationMessage, bool, bool,
                ResponeCommMessage>(toEdit.Item1, true, msg.Success, msg);
            _messagesSentObserver.Add(toAdd);
            return true;
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
                _logic.SearchResultRecived(((SearchResponseCommMessage)msg).Games);
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
                        ReceivedJoin(msg as JoinResponseCommMessage);
                        break;
                    case CommunicationMessage.ActionType.Spectate:
                        ReceivedSpectate(msg as JoinResponseCommMessage);
                        break;
                        case CommunicationMessage.ActionType.Leave:
                            ReceivedLeave(msg as JoinResponseCommMessage);
                            break;
                    case CommunicationMessage.ActionType.SpectatorLeave:
                            ReceivedSpectetorLeave(msg as JoinResponseCommMessage);
                            break;
                    default:
                        GeneralCase(msg.GameData);
                        break;
                }
                return true;
            }
            return false;
        }

        private bool ReceivedLeave(JoinResponseCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.LeaveAsPlayer((JoinResponseCommMessage)msg);
                return true;
            }
            return false;
        }

        private bool ReceivedSpectetorLeave(JoinResponseCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.LeaveAsSpectetor((JoinResponseCommMessage)msg);
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
