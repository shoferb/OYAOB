using System.Collections.Generic;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges
{
    class UserBridge : IUserBridge
    {
        private readonly UserServiceHandler _userService;
        private readonly GameServiceHandler _gameService;

        public UserBridge()
        {
            _gameService = new GameServiceHandler();
            _userService = new UserServiceHandler();
        }

        public bool IsUserLoggedIn(int userId)
        {
            var user = _userService.GetUserFromId(userId);
            return user != null && user.IsActive;
        }

        public string GetUserName(int id)
        {
            var user = _userService.GetUserFromId(id);
            if (user != null)
            {
                return user.Name;
            }
            return "";
        }

        public string GetUserPw(int id)
        {
            var user = _userService.GetUserFromId(id);
            if (user != null)
            {
                return user.Password;
            }
            return "";
        }

        public string GetUserEmail(int id)
        {
            var user = _userService.GetUserFromId(id);
            if (user != null)
            {
                return user.Email;
            }
            return "";
        }
        
        public string GetUserAvatar(int id)
        {
            var user = _userService.GetUserFromId(id);
            if (user != null)
            {
                return user.Avatar;
            }
            return "";
        }

        public int GetUserMoney(int id)
        {
            var user = _userService.GetUserFromId(id);
            if (user != null)
            {
                return user.Money;
            }
            return 0;
        }

        public int GetUserChips(int userId)
        {
            int chips = 0;
            User user = _userService.GetUserFromId(userId);
            user.ActiveGameList.ForEach(game =>
            {
                chips += _userService.GetPlayer(userId, game._id)._totalChip;
            });
            return chips;
        }

        public int GetUserChips(int userId, int roomId)
        {
            var player = _userService.GetPlayer(userId, roomId);
            if (player != null)
            {
                return player._totalChip;
            }
            return 0;
        }

        public List<int> GetUsersGameRooms(int userId)
        {
            List<GameRoom> allGames = _gameService.GetAllGames();
            List<int> gameIds = new List<int>();
            allGames.ForEach(game =>
            {
                game._players.ForEach(p =>
                {
                    if (p.Id == userId)
                    {
                        gameIds.Add(game._id);
                    }
                });
                game._spectatores.ForEach(s =>
                {
                    if (s.Id == userId)
                    {
                        gameIds.Add(game._id);
                    }
                });

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
            if (_userService.GetUserFromId(userId).IsHigherRank)
            {
                GameCenter center = GameCenter.Instance;

                return center.LeagueChangeAfterGapChange(criteria);
            }
            return false;
        }

        public bool IsThereUser(int id)
        {
            return _userService.GetUserFromId(id) != null;
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

        public bool EditPw(int id, string newPw)
        {
            return _userService.EditUserPassword(id, newPw);
        }

        public bool EditEmail(int id, string newEmail)
        {
            return _userService.EditUserEmail(id, newEmail);
        }
        
        public bool EditAvatar(int id, string newAvatarPath)
        {
            return _userService.EditUserAvatar(id, newAvatarPath);
        }

        public bool AddUserToGameRoomAsPlayer(int userId, int roomId, int chipAmount)
        {
            User user = _userService.GetUserFromId(userId);
            if (user == null)
            {
                return _gameService.AddPlayerToRoom(userId, roomId, chipAmount);
            }
            return false;
        }

        public bool AddUserToGameRoomAsSpectator(int userId, int roomId)
        {
            User user = _userService.GetUserFromId(userId);
            if (user == null)
            {
                return _gameService.AddSpectatorToRoom(userId, roomId);
            }
            return false;
        }

        public bool RemoveUserFromRoom(int userId, int roomId)
        {
            return _gameService.RemoveUserFromRoom(userId, roomId);
        }

        private bool ChangeUserMoney(int userId, int amount)
        {
            var user = _userService.GetUserFromId(userId);
            if (user != null)
            {
                user.Money += amount;
                return true;
            }
            return false;
        }

        public bool ReduceUserMoney(int userId, int amount)
        {
            return ChangeUserMoney(userId, -1 * amount);
        }

        public bool AddUserMoney(int userId, int amount)
        {
            return ChangeUserMoney(userId, amount);
        }

        public bool AddUserChips(int userId, int roomId, int amount)
        {
            var player = _userService.GetPlayer(userId, roomId);
            if (player != null)
            {
                player._totalChip += amount;
                return true;
            }
            return false;
        }
    }
}
