
using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Service;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges.Real
{
    class UserBridgeReal : IUserBridge
    {
        private UserServiceHandler _userService;
        private GameServiceHandler _gameService;

        public UserBridgeReal()
        {
            //TODO: init service here
        }

        public bool IsUserLoggedIn(int userId)
        {
            throw new System.NotImplementedException();
        }

        public string GetUserName(int id)
        {
            return _userService.GetUserFromId(id).Name;
        }

        public string GetUserPw(int id)
        {
            return _userService.GetUserFromId(id).Password;
        }

        public string GetUserEmail(int id)
        {
            return _userService.GetUserFromId(id).Email;
        }

        public int GetUserMoney(int id)
        {
            return _userService.GetUserFromId(id).Money;
        }

        public int GetUserChips(int userId)
        {
            throw new System.NotImplementedException();
        }

        public int GetUserChips(int userId, int roomId)
        {
            throw new System.NotImplementedException();
        }

        public List<int> GetUsersGameRooms(int userId)
        {
            List<GamePrefDecorator> allGames = _gameService.GetAllGames();
            List<int> gameIds = new List<int>();
            allGames.ForEach(game =>
            {
                if (game.player.Contains(userId))
                {
                    gameIds.Add(game.Id);
                }
            });
            return gameIds;
        }

        public List<string> GetUserNotificationMsgs(int userId)
        {
            var toReturn = new List<string>();
            var notifications = _userService.GetUserNotifications(userId);
            notifications.ForEach(noti =>
            {
                toReturn.Add(noti.Msg);
            });
            return toReturn;
        }

        public int GetNextFreeUserId()
        {
            return _userService.GetNextUserId();
        }

        public int GetUserWins(int userId)
        {
            throw new System.NotImplementedException();
        }

        public int GetUserLosses(int userId)
        {
            throw new System.NotImplementedException();
        }

        public int GetUserRank(int userId)
        {
            return _userService.GetUserFromId(userId).Points;
        }

        public void SetUserRank(int userId, int rank)
        {
            _userService.GetUserFromId(userId).Points = rank;
        }

        public bool SetUserRank(int userIdToChange, int rank, int changingUserId)
        {
            int changingUserRank = _userService.GetUserFromId(changingUserId).Points;
            if (changingUserRank == _userService.GetMaxUserPoints())
            {
                SetUserRank(userIdToChange, rank);
                return true;
            }
            return false;
        }

        public bool SetLeagueCriteria(int userId, int criteria)
        {
            throw new System.NotImplementedException();
        }

        public bool IsThereUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<int> GetAllUsers()
        {
            var allUsers = _userService.GetAllUsers();
            List<int> ids = new List<int>();
            allUsers.ForEach(user =>
            {
                ids.Add(user.Id);
            });
            return ids;
        }

        public bool LoginUser(string name, string password)
        {
            return _userService.LoginUser(name, password);
        }

        public bool LogoutUser(int userId)
        {
            return _userService.LogoutUser(userId);
        }

        public bool RegisterUser(string name, string pw1, string email)
        {
            int id = _userService.GetNextUserId();
            var user = _userService.CreateNewUser(id, name, name, pw1, email);
            return user != null;
        }

        public bool DeleteUser(string name, string pw)
        {
            return _userService.DeleteUser(name, pw);
        }

        public bool DeleteUser(int id)
        {
            var user = _userService.GetUserFromId(id);
            return DeleteUser(user.Name, user.Password);
        }

        public bool EditName(int id, string newName)
        {
            return _userService.EditUserName(id, newName);
        }

        public bool EditPw(int id, string oldPw, string newPw)
        {
            return _userService.EditUserPassword(id, oldPw, newPw);
        }

        public bool EditEmail(int id, string newEmail)
        {
            return _userService.EditUserEmail(id, newEmail);
        }

        public bool AddUserToGameRoomAsPlayer(int userId, int roomId, int chipAmount)
        {
            throw new System.NotImplementedException();
        }

        public bool AddUserToGameRoomAsSpectator(int userId, int roomId)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveUserFromRoom(int userId, int roomId)
        {
            throw new System.NotImplementedException();
        }

        public bool ReduceUserMoney(int userId, int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool AddUserMoney(int userId, int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool AddUserChips(int userId, int roomId, int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}
