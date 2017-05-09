using TexasHoldem.Service;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;
using TexasHoldemShared.Parser;

namespace TexasHoldem.communication.Impl
{
    //TODO: this class
    class ServerEventHandler : IEventHandler
    {
        private readonly UserServiceHandler _userService; //TODO init // = new UserServiceHandler();
        private readonly CommunicationHandler _commHandler = CommunicationHandler.GetInstance();
        private readonly ICommMsgXmlParser _parser; //TODO: init
        //private readonly LogServiceHandler _logService = new LogServiceHandler(); //TODO: change to log control

        public void HandleEvent(ActionCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(EditCommMessage msg)
        {
            bool success;
            switch (msg.FieldToEdit)
            {
                case EditCommMessage.EditField.UserName:
                    success = _userService.EditName(msg.UserId, msg.NewValue);
                    break;
                case EditCommMessage.EditField.Password:
                    success = _userService.EditUserPassword(msg.UserId, msg.NewValue);
                    break;
                case EditCommMessage.EditField.Avatar:
                    success = _userService.EditUserAvatar(msg.UserId, msg.NewValue);
                    break;
                case EditCommMessage.EditField.Email:
                    success = _userService.EditUserEmail(msg.UserId, msg.NewValue);
                    break;
                default:
                   // _logService.CreateNewErrorLog(
                    //    "an unidentified EditCommMessage was received by ServerEventHandler.");
                    return;
            }
            ResponeCommMessage response = new ResponeCommMessage(msg.UserId, success, msg);
            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
        }

        public void HandleEvent(LoginCommMessage msg)
        {
            throw new System.NotImplementedException();
            //todo call login in service and send responce with user info
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
            //todo call register in service and send responce with user info
            throw new System.NotImplementedException();
        }

        public void HandleEvent(GameDataCommMessage msg)
        {
            _commHandler.AddMsgToSend(_parser.SerializeMsg(msg), msg.UserId);
        }

        public void HandleEvent(MoveOptionsCommMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleEvent(ResponeCommMessage msg)
        {
            _commHandler.AddMsgToSend(_parser.SerializeMsg(msg), msg.UserId);
        }
    }
}
