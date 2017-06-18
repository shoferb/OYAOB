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

        private void GeneralCase(GameDataCommMessage msgGameData)
        {
            _logic.GameUpdateReceived(msgGameData);
        }

        private void ObserverNotify(ResponeCommMessage msg)
        {
            Tuple<CommunicationMessage, bool, bool, ResponeCommMessage> toEdit =
                _messagesSentObserver.Find(x => x.Item1.Equals(msg.OriginalMsg));
            _messagesSentObserver.Remove(toEdit);
            var toAdd = new Tuple<CommunicationMessage, bool, bool,
                ResponeCommMessage>(toEdit.Item1, true, msg.Success, msg);
            _messagesSentObserver.Add(toAdd);
        }


        public void Notify(ChatCommMessage originalMsg, ResponeCommMessage msg)
        {
            if (originalMsg.ChatType == CommunicationMessage.ActionType.PlayerWhisper ||
                originalMsg.ChatType == CommunicationMessage.ActionType.SpectetorWhisper)
            {
                return;
            }
            GeneralCase(msg.GameData);
        }

        public void Notify(LoginCommMessage originalMsg, ResponeCommMessage msg)
        {
            ObserverNotify(msg);
        }

        public void Notify(RegisterCommMessage originalMsg, RegisterResponeCommMessage msg)
        {
            ObserverNotify(msg);
        }

        public void Notify(CreateNewRoomMessage originalMsg, ResponeCommMessage msg)
        {
            ObserverNotify(msg);
        }

        public void Notify(ReturnToGameAsPlayerCommMsg originalMsg, ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.PlayerReturnsToGame(msg.GameData);
            }
        }

        public void Notify(ReturnToGameAsSpecCommMsg originalMsg, ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.SpecReturnsToGame(msg.GameData);
            }
        }

        public void Notify(SearchCommMessage originalMsg, ResponeCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.SearchResultRecived(((SearchResponseCommMessage)msg).Games);
            }
        }

        private void ReceivedJoin(JoinResponseCommMessage msg)
        {
            if (_logic != null)
            {
                _logic.JoinAsPlayerReceived(msg);
            }
        }

        private void ReceivedSpectate(JoinResponseCommMessage msg)
        {
            _logic.JoinAsSpectatorReceived(msg);
        }

        public void Notify(ActionCommMessage originalMsg, ResponeCommMessage msg)
        {
            if (originalMsg != null && msg != null)
            {
                switch (originalMsg.MoveType)
                {
                    case CommunicationMessage.ActionType.Join:
                        ReceivedJoin(msg as JoinResponseCommMessage);
                        break;
                    case CommunicationMessage.ActionType.Spectate:
                        ReceivedSpectate(msg as JoinResponseCommMessage);
                        break;
                    default:
                        GeneralCase(msg.GameData);
                        break;
                }
            }
        }

        public void Notify(CommunicationMessage originalMsg, ResponeCommMessage msg)
        {
            if (msg != null)
            {
                GeneralCase(msg.GameData);
            }
        }
    }
}
