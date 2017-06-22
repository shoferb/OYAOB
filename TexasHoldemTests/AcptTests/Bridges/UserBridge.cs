using System;
using System.Collections.Generic;
using TexasHoldem.communication.Impl;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Game_Control;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemShared;
using TexasHoldemShared.CommMessages;
using TexasHoldemTests.AcptTests.Bridges.Interface;

namespace TexasHoldemTests.AcptTests.Bridges
{
    public class UserBridge : IUserBridge
    {
        private readonly UserServiceHandler _userService;
        private readonly GameServiceHandler _gameService;
        private const int RegisterMoney = 1000;

        public UserBridge(GameCenter gc, SystemControl sys, LogControl log, ReplayManager replay)
        {
            var ses = new SessionIdHandler();
            _gameService = new GameServiceHandler(gc, sys, log, replay, ses);
            _userService = new UserServiceHandler(gc, sys);
        }

        public bool IsUserLoggedIn(int userId)
        {
            var user = _userService.GetUserById(userId);
            return user != null && user.IsLogin();
        }

        public string GetUserName(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return user.MemberName();
            }
            return "";
        }

        public string GetUserPw(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return user.Password();
            }
            return "";
        }

        public string GetUserEmail(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return user.Email();
            }
            return "";
        }

        public string GetUserAvatar(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return user.Avatar();
            }
            return "";
        }

        public int GetUserMoney(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return user.Money();
            }
            return 0;
        }

        public int GetUserChips(int userId)
        {
            int chips = 0;
            IUser user = _userService.GetUserById(userId);
            if (user != null)
            {
                user.ActiveGameList().ForEach(game =>
                    {
                        if (game != null)
                        {
                            var players = game.GetPlayersInRoom().FindAll(p => p.user.Id() == userId);
                            players.ForEach(p =>
                            {
                                chips += p.TotalChip;
                            });
                        }
                    });
            }
            return chips;
        }

        public int GetUserChips(int userId, int roomId)
        {/*
            var games = _userService.GetActiveGamesByUserName(userId);
            if (player != null)
            {
                return player._totalChip;
            }*/
            return 0;
        }

        public List<int> GetUsersGameRooms(int userId)
        {
            List<int> gameIds = new List<int>();
            List<IGame> allGames = _gameService.GetAllGames();
            if (allGames != null)
            {
                allGames.ForEach(game =>
                {
                    game.GetPlayersInRoom().ForEach(p =>
                    {
                        if (p.user.Id() == userId)
                        {
                            gameIds.Add(game.Id);
                        }
                    });
                    game.GetSpectetorInRoom().ForEach(s =>
                    {
                        if (s.user.Id() == userId)
                        {
                            gameIds.Add(game.Id);
                        }
                    });

                });

            }
            return gameIds;
        }

        public int GetNextFreeUserId()
        {
            return new Random().Next();
        }

        public int GetUserPoints(int userId)
        {
            var user = _userService.GetUserById(userId);
            if (user != null)
            {
                return user.Points();
            }
            return -1;
        }

        public void SetUserPoints(int userId, int points)
        {
            /*  _userService.EditUserPoints(userId, points);*/
        }

        public bool IsThereUser(int id)
        {
            return _userService.GetUserById(id) != null;
        }

        public List<int> GetAllUsers()
        {
            var allUsers = _userService.GetAllUser();
            List<int> ids = new List<int>();
            allUsers.ForEach(user =>
            {
                ids.Add(user.Id());
            });
            return ids;
        }

        public bool LoginUser(string name, string password)
        {
            var t = _userService.LoginUser(name, password);
          
            return t!=null && t.IsLogin();
        }

        public bool LogoutUser(int userId)
        {
            var t = _userService.LogoutUser(userId);
            return t!=null && !t.IsLogin();
        }

        public int RegisterUser(string name, string pw1, string email)
        {
            int id = new Random().Next();
            var success = _userService.RegisterToSystem(id, name, name, pw1, RegisterMoney, email);
            if (success)
            {
                return id;
            }
            return -1;
        }

        public bool RegisterUser(int id, string name, string pw1, string email)
        {
            return _userService.RegisterToSystem(id, name, name, pw1, RegisterMoney, email);
        }

        public bool DeleteUser(string name, string pw)
        {
            return _userService.DeleteUser(name, pw);
        }

        public bool DeleteUser(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return DeleteUser(user.Name(), user.Password());
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

            IUser user = _userService.GetUserById(userId);
            if (user != null)
            {
                return ActionSuccedded(_gameService.DoAction(userId, CommunicationMessage.ActionType.Join, chipAmount, roomId));
            }
            return false;
        }

        public bool AddUserToGameRoomAsSpectator(int userId, int roomId)
        {
            IUser user = _userService.GetUserById(userId);
            if (user != null)
            {
                //TODO
                //return _gameService.AddSpectatorToRoom(userId, roomId);
            }
            return false;
        }

        public IUser getUserById(int userId)
        {
            return _userService.GetUserById(userId);
        }

        public bool RemoveUserFromRoom(int userId, int roomId)
        {
            return ActionSuccedded(_gameService.DoAction(userId, CommunicationMessage.ActionType.Leave, 0, roomId));
        }

        public bool RemoveSpectatorFromRoom(int userId, int roomId)
        {
            return ActionSuccedded(_gameService.RemoveSpectatorFromRoom(userId, roomId));
        }

        public bool DividLeage()
        {
            return _userService.DevideLeague();
        }

        private bool ChangeUserMoney(int userId, int amount)
        {
            var user = _userService.GetUserById(userId);
            if (user != null)
            {
                return _userService.EditMoney(userId, user.Money() + amount);
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

        public List<IUser> GetUsersByNumOfGames()
        {
            return _userService.GetUsersByNumOfGames();
        }

        public List<IUser> GetUsersByHighestCash()
        {
            return _userService.GetUsersByHighestCash();
        }

        public List<IUser> GetUsersByTotalProfit()
        {
            return _userService.GetUsersByTotalProfit();
        }

       
        private bool ActionSuccedded(IEnumerator<ActionResultInfo> results)
        {
            results.MoveNext();
            ActionResultInfo result = results.Current;
            return result.GameData.IsSucceed;
        }
        public List<IUser> GetUsersByHighestCashn()
        {
            return new List<IUser>();
        }
    }
}
