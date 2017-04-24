using System;
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
            if (user != null)
            {
                user.ActiveGameList.ForEach(game =>
                    {
                        chips += _userService.GetPlayer(userId, game._id)._totalChip;
                    }); 
            }
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
            List<int> gameIds = new List<int>();
            List<GameRoom> allGames = _gameService.GetAllGames();
            if (allGames != null)
            {
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
            }
            return gameIds;
        }

        public int GetNextFreeUserId()
        {
            return _userService.GetNextUserId();
        }

        public int GetUserPoints(int userId)
        {
            var user = _userService.GetUserFromId(userId);
            if (user != null)
            {
                return user.Points;
            }
            return -1;
        }

        public void SetUserPoints(int userId, int points)
        {
            _userService.EditUserPoints(userId, points);
        }

        public bool SetUserPoints(int userIdToChange, int points, int changingUserId)
        {
            var user = _userService.GetUserFromId(changingUserId);
            if (user != null && user.Points == _userService.GetMaxUserPoints())
            {
                SetUserPoints(userIdToChange, points);
                return true;
            }
            return false;
        }

        public bool SetLeagueCriteria(int userId, int criteria)
        {
            var user = _userService.GetUserFromId(userId);
            if (user != null && user.IsHigherRank)
            {
                return GameCenter.Instance.LeagueChangeAfterGapChange(criteria);
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
            int id = new Random().Next();
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
            if (user != null)
            {
                return DeleteUser(user.Name, user.Password); 
            }
            return false;
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
                return SystemControl.SystemControlInstance.EditUserMoney(userId, user.Money + amount);
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

    }
}
