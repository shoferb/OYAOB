using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game_Control
{
    public class  GameCenter
    {
        private List<League> leagueTable;
        private List<Log> logs;
        private User higherRank;
        private int leagueGap;
        public List<GameRoom> games { get;  }
        public List<ErrorLog> errorLog { get; set; }
        public List<SystemLog> systemLog { get; set; }
        private static int roomIdCounter = 0;
        private static GameCenter singlton;
        private ReplayManager _replayManager;

       
        private static GameCenter instance = null;
        private static readonly object padlock = new object();


        public static GameCenter Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameCenter();
                    }
                    return instance;
                }
            }
        }

       
        private GameCenter()
        {
            this.leagueTable = new List<League>();
            //add first league function
            this.logs = new List<Log>();
            this.games = new List<GameRoom>();
            _replayManager = new ReplayManager();
            errorLog = new List<ErrorLog>();
            systemLog = new List<SystemLog>();
        }

       
        private GameReplay GetGameReplay(int roomID, int gameID)
        {
            return _replayManager.GetGameReplay(roomID, gameID);
        }

        public string ShowGameReplay(int roomID, int gameID)
        {
            GameReplay gr = GetGameReplay(roomID, gameID);
            if (gr == null)
            {
                return null;
            }
            return gr.ToString();
        }

        //return thr next room Id
        public int GetNextIdRoom()
        {
            int toReturn = System.Threading.Interlocked.Increment(ref roomIdCounter);
            return toReturn;
        }
        
        //create new game room
        //game type  policy, limit, no-limit, pot-limit
        //אם הכסף של השחקן 0 אז מרוקנים את השדה של הכסף לגמרי
        public bool CreateNewRoom(int userId, int smallBlind, int playerMoney)
        {
            bool toReturn = false;
            if (playerMoney < smallBlind)
            {
                return toReturn;
            }   
            int nextId = GetNextIdRoom();
            List<Player> players = new List<Player>();
            SystemControl sc = new SystemControl();
            User user = sc.GetUserWithId(userId);

            Player player = new Player(smallBlind, 0, user.Id, user.Name, user.MemberName, user.Password, user.Points,
                user.Money, user.Email, nextId);
            ConcreteGameRoom room = new ConcreteGameRoom(players, smallBlind, nextId, _replayManager);
            toReturn = AddRoom(room);
            return toReturn;
        }

        public ConcreteGameRoom GetRoomById(int roomId)
        {
            ConcreteGameRoom toReturn = null;
            foreach (ConcreteGameRoom room in games)
            {
                if (room._id == roomId)
                {

                    toReturn = room;
                }
            }
            return toReturn;
        }

        public bool IsRoomExist(int roomId)
        {

            bool toReturn = false;
            foreach (ConcreteGameRoom room in games)
            {
                if (room._id == roomId)
                {
                    toReturn = true;
                }
            }
            return toReturn;
        }


        public bool RemoveRoom(int roomId)
        {
            bool toReturn = false;
            bool exist = IsRoomExist(roomId);
            if (!exist)
            {
                return toReturn;
            }
            ConcreteGameRoom toRemove = GetRoomById(roomId);
            try
            {
                int userId;
                SystemControl sc = new SystemControl();
                foreach (Player p in toRemove._players)
                {
                    userId = p.Id;
                    if (sc.HasThisActiveGame(roomId, userId))
                    {
                        sc.RemoveRoomFromActiveRoom(roomId, userId);
                    }
                    if (sc.HasThisSpectetorGame(roomId, userId))
                    {
                        sc.RemoveRoomFromSpectetRoom(roomId, userId);
                    }
                }
                games.Remove(toRemove);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool AddRoom(ConcreteGameRoom roomToAdd)
        {
            bool toReturn;
            try
            {
                this.games.Add(roomToAdd);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool AddPlayerToRoom(int roomId, int userId, int playerChipToEnterRoom)
        {
            bool toReturn = false;
            SystemControl sc = new SystemControl();
            User user = sc.GetUserWithId(userId);
            ConcreteGameRoom room = GetRoomById(roomId);
            int sb = room._sb;
            bool exist = IsRoomExist(roomId);
            if (!exist)
            {
                return toReturn;
            }
            if (playerChipToEnterRoom < sb)
            {
                return toReturn;
            }
            Player playerToAdd = new Player(playerChipToEnterRoom, 0, user.Id, user.Name, user.MemberName, user.Password, user.Points,
                user.Money, user.Email, roomId);
            try
            {
                ConcreteGameRoom toAdd = room;
                User newUser = user; // add room to user list
                newUser.ActiveGameList.Add(room);

                room._players.Add(playerToAdd);

                sc.ReplaceUser(user, newUser);
                games.Remove(room);
                games.Add(toAdd);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool AddSpectetorToRoom(int roomId, int userId)
        {
            bool toReturn = false;
            SystemControl sc = new SystemControl();
            User user = sc.GetUserWithId(userId);
            bool exist = IsRoomExist(roomId);
            if (!exist)
            {
                return toReturn;
            }
            ConcreteGameRoom room = GetRoomById(roomId);
            try
            {
                ConcreteGameRoom toAdd = room;
                User newUser = user; // add room to user list
                newUser.SpectateGameList.Add(room);
                Spectetor spectetor = new Spectetor(user.Id, user.Name, user.MemberName, user.Password, user.Points,
                    user.Money, user.Email, roomId);
                toAdd._spectatores.Add(spectetor);
                sc.ReplaceUser(user, newUser);
                games.Remove(room);
                games.Add(toAdd);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool RemovePlayerFromRoom(int roomId, int userId)
        {
            bool toReturn = false;
            bool exist = IsRoomExist(roomId);
            if (!exist)
            {
                return toReturn;
            }
            SystemControl sc = new SystemControl();
            ConcreteGameRoom room = GetRoomById(roomId);
            ConcreteGameRoom toAdd = room;
            List<Player> allPlayers = room._players;
            Player playerToRemove = null;
            User user = sc.GetUserWithId(userId);
            
            foreach (Player p in allPlayers)
            {
                if ((p.Id == userId) && (p.RoomId == roomId))
                {
                    playerToRemove = p;
                }
            }

            try
            {
                playerToRemove._isInRoom = false; //not in room - need to remove in end od round
                //allPlayers.Remove(playerToRemove);
               // toAdd._players = allPlayers;
                //games.Remove(room);
                //games.Add(toAdd);

                user.ActiveGameList.Remove(room);
                //sc.ReplaceUser(user, newUser);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }

            return toReturn;
        }


        public bool RemoveSpectetorFromRoom(int roomId, int userId)
        {
            bool toReturn = false;
            bool exist = IsRoomExist(roomId);
            if (!exist)
            {
                return toReturn;
            }
            SystemControl sc = new SystemControl();
            ConcreteGameRoom room = GetRoomById(roomId);
            ConcreteGameRoom toAdd = room;
            List<Spectetor> allSpectetors = room._spectatores;
            Spectetor sprctetorToRemove = null;
            User user = sc.GetUserWithId(userId);
            User newUser = user;
            foreach (Spectetor s in allSpectetors)
            {
                if ((s.Id == userId) && (s.RoomId == roomId))
                {
                    sprctetorToRemove = s;
                }
            }
            try
            {
                allSpectetors.Remove(sprctetorToRemove);
                toAdd._spectatores = allSpectetors;
                games.Remove(room);
                newUser.SpectateGameList.Remove(room);
                sc.ReplaceUser(user, newUser);
                games.Add(toAdd);
                toReturn = true;
            }
            catch (Exception e)
            {
                toReturn = false;
            }
            return toReturn;
        }


        public bool LeagueChange(int leagugap)
        {
            bool toReturn = false;
            int higherRank = HigherRank.Points;
            leagueTable = null;
            int currpoint = 0;
            int i = 1;
            int to = 0;
            String leaugeName;
            while (currpoint < higherRank)
            {
                leaugeName = "" + i;
                to = currpoint + leagueGap;
                League toAdd = new League(leaugeName, currpoint, to);
                i++;
                currpoint = to;
            }
            return toReturn;
        }

        public string UserLeageInfo(User user)
        {
            string toReturn = "";
            int userRank = user.Points;
            int i = 1;
            int min = 0;
            int max = leagueGap;
            bool flag = ((userRank > min) && (userRank < max));
            while (!flag)
            {
                if ((userRank > min) && (userRank < max))
                {
                    flag = true;
                    toReturn = "" + i;
                }
                else
                {
                    min = max + 1;
                    max = min + leagueGap;
                }
            }
            return toReturn;
        }

        public List<ConcreteGameRoom> GetAllActiveGame()
        {
            List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
            foreach (ConcreteGameRoom room in games)
            {
                if (room._isActiveGame)
                {
                    toReturn.Add(room);
                }
            }
            return toReturn;
        }

        public List<ConcreteGameRoom> GetAllSpectetorGame()
        {
            List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
            foreach (ConcreteGameRoom room in games)
            {
                if (room._isSpectetor)
                {
                    toReturn.Add(room);
                }
            }
            return toReturn;
        }

        public List<ConcreteGameRoom> GetAllGames()
        {
            List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
            foreach (ConcreteGameRoom room in games)
            {
                toReturn.Add(room);
            }
            return toReturn;
        }


        //todo ??? potCount =? postsize
        public List<ConcreteGameRoom> GetAllGamesByPotSize(int potSize)
        {
            List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
            foreach (ConcreteGameRoom room in games)
            {
                if (room._potCount == potSize)
                {
                    toReturn.Add(room);
                }
                
            }
            return toReturn;
        }


        public bool IsGameCanSpectete(int roomId)
        {
            bool toReturn = false;
            ConcreteGameRoom room = GetRoomById(roomId);
            if (room._isSpectetor)
            {
                toReturn = true;
            }
            return toReturn;
        }

        public bool IsGameActive(int roomId)
        {
            bool toReturn = false;
            ConcreteGameRoom room = GetRoomById(roomId);
            if (room._isActiveGame)
            {
                toReturn = true;
            }
            return toReturn;
        }
        public bool SendNotification(User reciver, Notification toSend)
        {
            bool toReturn = false;
            reciver.SendNotification(toSend);
            return toReturn;
        }

        //getters setters
        public List<League> LeagueTable
        {
            get
            {
                return leagueTable;
            }

            set
            {
                leagueTable = value;
            }
        }
        public List<Log> Logs
        {
            get
            {
                return logs;
            }

            set
            {
                logs = value;
            }
        }

        public int LeagueGap
        {
            get
            {
                return LeagueGap;
            }

            set
            {
                LeagueGap = value;
            }
        }



        public User HigherRank
        {
            get
            {
                return higherRank;
            }

            set
            {
                higherRank = value;
            }
        }
    }
}
