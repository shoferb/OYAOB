using TexasHoldem.Logic.Users;
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
            
            bool success = _userService.LoginUser(msg.UserName, msg.Password);
            if (success)
            {
                IUser user = _userService.GetUserById(msg.UserId);
                ResponeCommMessage response = new LoginResponeCommMessage(user.Id(), user.Name(), user.MemberName(),
                    user.Password(), user.Avatar(), user.Money()
                    , user.Email(), success, msg);
                _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            }
            else
            {
                ResponeCommMessage response = new LoginResponeCommMessage(-1, "", "",
                    "", "", -1 , "", success, msg);
                _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            }
           
        }

        public void HandleEvent(RegisterCommMessage msg)
        {
            bool success = _userService.RegisterToSystem(msg.UserId, msg.Name, msg.MemberName, msg.Password, msg.Money,
                msg.Email);
            
            ResponeCommMessage response = new RegisterResponeCommMessage(msg.UserId,msg.Name,msg.MemberName,msg.Password,
                "/GuiScreen/Photos/Avatar/devil.png",msg.Money,msg.Email,success,msg);

            _commHandler.AddMsgToSend(_parser.SerializeMsg(response), msg.UserId);
            
        }

        public void HandleEvent(SearchCommMessage msg)
        {
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
