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
        private List<GameRoom> games;
        public List<ErrorLog> errorLog { get; set; }
        public List<SystemLog> systemLog { get; set; }
        private static int roomIdCounter = 0;
        private static GameCenter singlton;
        private ReplayManager _replayManager;

       
        private static GameCenter instance = null;


        private static readonly object padlock = new object();

        private GameCenter()
        {
            this.leagueTable = new List<League>();
            //todo - add first league function 
            this.logs = new List<Log>();// - why need this ToString()?
            this.games = new List<GameRoom>();
            _replayManager = new ReplayManager();
            errorLog = new List<ErrorLog>();
            systemLog = new List<SystemLog>();
        }

        public static GameCenter Instance
        {
            get
            {
                lock (padlock)
                {
                if(instance == null)
                    {
                        instance = new GameCenter();
                    }
                    return instance;
                }
            }
        }


        public ReplayManager GetReplayManager()
        {
            return _replayManager;
        }


        //return all games in the system 0 active and non active
        public List<GameRoom> GetGames()
        {
            return games;
        }
       
        //edit the gap field - syncronized 
        public bool EditLeagueGap(int newGap)
        {
            bool toReturn;
            lock (padlock)
            {
                try
                {
                    LeagueGap = newGap;
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
            }
            return toReturn;
        }
       
      

       
        public GameReplay GetGameReplay(int roomID, int gameID, int userID)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(roomID, gameID);
            List<Tuple<int, int>> userGames = GetGamesAvailableForReplayByUser(userID);
            if (!userGames.Contains(tuple))
            {
                return null;
            }
            return _replayManager.GetGameReplay(roomID, gameID);
        }


        public string ShowGameReplay(int roomID, int gameID, int userID)
        {
            GameReplay gr = GetGameReplay(roomID, gameID, userID);
            if (gr == null)
            {
                return null;
            }
            return gr.ToString();
        }


        //need to syncronzed?? 
        public string getActionFromGameReplay(int roomID, int gameID, int userID, int actionNum)
        {
            GameReplay gr = GetGameReplay(roomID, gameID, userID);
            if (gr == null)
            {
                return null;
            }
            TexasHoldem.Logic.Actions.Action action = gr.GetActionAt(actionNum);
            if (action == null)
            {
                return null;
            }
            return action.ToString();
        }

        //return thr next room Id
        public int GetNextIdRoom()
        {
            lock (padlock)
            {
                int toReturn = System.Threading.Interlocked.Increment(ref roomIdCounter);
                return toReturn;
            }
        }

        //create new game room
        //game type  policy, limit, no-limit, pot-limit
        //אם הכסף של השחקן 0 אז מרוקנים את השדה של הכסף לגמרי
        public bool CreateNewRoom(int userId, int startingChip, int playerMoney, bool isSpectetor, GameMode gameModeChosen, int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney)
        {
            lock (padlock)
            {
            bool toReturn = false;
                if (playerMoney < startingChip)
                {
                    return toReturn;
                }
                int nextId = GetNextIdRoom();
                List<Player> players = new List<Player>();
               
                User user = SystemControl.SystemControlInstance.GetUserWithId(userId);   //imposible like that

                Player player = new Player(startingChip, 0, user.Id, user.Name, user.MemberName, user.Password, user.Points,
                    user.Money, user.Email, nextId);
                ConcreteGameRoom room = new ConcreteGameRoom(players, startingChip, nextId, isSpectetor, gameModeChosen, minPlayersInRoom, maxPlayersInRoom, enterPayingMoney);
                toReturn = AddRoom(room);
                return toReturn;
            }
            
        }

        public List<Tuple<int, int>> GetGamesAvailableForReplayByUser(int userID)
        {
            lock (padlock)
            {
                User user = SystemControl.SystemControlInstance.GetUserWithId(userID); 
                return user._gamesAvailableToReplay;
            }
        }


        //return room by room if - suncronized due to for
        public ConcreteGameRoom GetRoomById(int roomId)
        {
            lock (padlock)
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
        }


        //todo - aviv to impl or to remove?
        internal GameReplay GetGameReplay(int roomID, int gameID)
        {
            throw new NotImplementedException();
        }

        //return true if there is a room with this id
        public bool IsRoomExist(int roomId)
        {
            lock (padlock)
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
        }



        //remove room form games list - remove the room remove the game from active game list and spectetor in user
        //
        public bool RemoveRoom(int roomId)
        {
            lock (padlock)
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
                    SystemControl sc = SystemControl.SystemControlInstance;
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
        }


        //Add new room the games list
        public bool AddRoom(ConcreteGameRoom roomToAdd)
        {
            lock (padlock)
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
        }


       //Add Player to room
       //todo - dec money from user acourding to buy in policy
        public bool AddPlayerToRoom(int roomId, int userId, int playerChipToEnterRoom)
        {
            lock (padlock)
            {
                bool toReturn = false;
                SystemControl sc = SystemControl.SystemControlInstance;
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
                Player playerToAdd = new Player(playerChipToEnterRoom, 0, user.Id, user.Name, user.MemberName,
                    user.Password, user.Points,
                    user.Money, user.Email, roomId);
                try
                {
                    ConcreteGameRoom toAdd = room;
                    User newUser = user; // add room to user list
                    newUser.ActiveGameList.Add(room);

                    room._players.Add(playerToAdd);

                   // sc.ReplaceUser(user, newUser);
                    //games.Remove(room);
                    //games.Add(toAdd);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //add spectetor to room - syncronized
        public bool AddSpectetorToRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                SystemControl sc = SystemControl.SystemControlInstance;
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
                  //  sc.ReplaceUser(user, newUser);
                   // games.Remove(room);
                    //games.Add(toAdd);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //remove player from room
        //todo - if player exit in the middle dec money
        public bool RemovePlayerFromRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                bool exist = IsRoomExist(roomId);
                if (!exist)
                {
                    return toReturn;
                }
                SystemControl sc = SystemControl.SystemControlInstance;
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
                    playerToRemove.IsActive = false;
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
        }



        //remove spectetor from room - sycronized
        public bool RemoveSpectetorFromRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                bool exist = IsRoomExist(roomId);
                if (!exist)
                {
                    return toReturn;
                }
                SystemControl sc = SystemControl.SystemControlInstance;
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
        }

        //create new league whith new gap
        public bool LeagueChangeAfterGapChange(int leagugap)
        {
            lock (padlock)
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

        //get all active games - syncronized
        public List<ConcreteGameRoom> GetAllActiveGame()
        {
            lock (padlock)
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
        }


        public List<ConcreteGameRoom> GetAllSpectetorGame()
        {
            lock (padlock)
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
        }



        public List<ConcreteGameRoom> GetAllGames()
        {
            lock (padlock)
            {
                List<ConcreteGameRoom> toReturn = new List<ConcreteGameRoom>();
                foreach (ConcreteGameRoom room in games)
                {
                    toReturn.Add(room);
                }
                return toReturn;
            }
        }


        //todo ??? potCount =? postsize
        //return list of games with pot size
        public List<ConcreteGameRoom> GetAllGamesByPotSize(int potSize)
        {
            lock (padlock)
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
        }


        //chaeck if game is spectetable
        //todo cahnge to game room
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


        //check if game is active game
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


        //seand notification to user
        public bool SendNotification(User reciver, Notification toSend)
        {
            lock (padlock)
            {
                bool toReturn = false;
                reciver.SendNotification(toSend);
                return toReturn;
            }
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
                lock (padlock)
                {
                    leagueTable = value;
                }
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
                lock (padlock)
                {
                    logs = value;
                }
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
                lock (padlock)
                {
                    LeagueGap = value;
                }
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
                lock (padlock)
                {
                    higherRank = value;
                }
            }
        }

        //todo - impl
        public Log FindLog(int logId)
        {
                //find log in the 2 list
                throw new NotImplementedException();
            
        }
    }

    
}
