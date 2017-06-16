using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Actions;

namespace TexasHoldem.Logic.Replay
{
    public class ReplayManager
    {
        public Dictionary<GameReplay, List<int>> _gamesActions { set; get; }
        private static readonly object padlock = new object();

        public ReplayManager()
        {
            _gamesActions = new Dictionary<GameReplay, List<int>>();
        }

        public bool AddGameReplay(GameReplay gr, List<int> ids)
        {
            lock (padlock)
            {

                if (gr == null || gr._gameNumber < 0 || gr._gameRoomID < 0 ||
                IsExist(gr._gameRoomID, gr._gameNumber))
                {
                    return false;
                }
                if (ids == null || ids.Count == 0)
                {
                    return false;
                }
                _gamesActions.Add(gr, ids);
            }
            return true;
        }
        public GameReplay GetGameReplayById(int roomId, int gameId)
        {
            foreach (var x in _gamesActions)
            {
                if(x.Key._gameRoomID == roomId & x.Key._gameNumber == gameId)
                {
                    return x.Key;
                }

            }
            return null;
        }

        public void UpdateGameReplayById(int roomId, int gameId, GameReplay newRep)
        {
            foreach (var x in _gamesActions)
            {
                if (x.Key._gameRoomID == roomId & x.Key._gameNumber == gameId)
                {
                    List<int> val = x.Value;
                    _gamesActions.Remove(x.Key);
                    _gamesActions.Add(newRep, val);
                }

            }
            
        }
        public bool IsExist(int gameRoomID, int gameNumber)
        {
            lock (padlock)
            {

                foreach (KeyValuePair<GameReplay, List<int>> entry in _gamesActions)
                {
                    if (entry.Key.RightGame(gameRoomID, gameNumber))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public GameReplay GetGameReplayForUser(int gameRoomID, int gameNumber, int userID) {
            if (!IsExist(gameRoomID, gameNumber))
            {
                return null;
            }
            lock (padlock)
            {
                foreach (KeyValuePair<GameReplay, List<int>> entry in _gamesActions)
                {
                    if (entry.Key.RightGame(gameRoomID, gameNumber))
                    {
                        if (entry.Value.Contains(userID))
                        {
                            return entry.Key;
                        }
                        break;
                    }
                }
            }
            return null;
        }

        public string ShowFirstGameReplayForUser(int userID, int gameRoomID)
        {
            string show = ShowGameReplay(gameRoomID, 0, userID);
            if (show != ""){
                return show;
            }
            return ShowGameReplay(gameRoomID, 1, userID);
        }

            public string ShowGameReplay(int gameRoomID, int gameNumber, int userID)
        {
            GameReplay gr = GetGameReplayForUser(gameRoomID, gameNumber, userID);
            if (gr!= null)
            {
                return gr.ToString();
            }
            return "";
        }

        public void DeleteGameReplay(int gameRoomID, int gameNumber)
        {
            lock (padlock)
            {
                if (_gamesActions != null && _gamesActions.Count > 0)
                {
                    var item = _gamesActions.First(e => e.Key._gameRoomID == gameRoomID &&
                    e.Key._gameNumber == gameNumber);
                    _gamesActions.Remove(item.Key);
                }
            }
         }
        //public GameMove GetActionFromGameReplay(int gameRoomID, int gameNumber, int actionNumber)
        //{
        //    GameReplay gr = GetGameReplayForUser(gameRoomID, gameNumber);
        //    if (gr == null)
        //    {
        //        return null;
        //    }
        //    return gr.GetActionAt(actionNumber);
        //}
    }
}
