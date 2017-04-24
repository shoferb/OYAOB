using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Replay;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using Action = TexasHoldem.Logic.Game.Action;

namespace TexasHoldem.Logic.Game_Control
{
    public class  GameCenter
    {
        private List<League> leagueTable;
        private List<Log> logs;
        private User higherRank;
        public int leagueGap { get; set; }
        private List<GameRoom> games;
        public List<ErrorLog> errorLog { get; set; }
        public List<SystemLog> systemLog { get; set; }
        private static int roomIdCounter = 1;
        private static GameCenter singlton;
        private ReplayManager _replayManager;


        private static GameCenter instance;


        private static readonly object padlock = new object();

        private GameCenter()
        {
            this.leagueTable = new List<League>();
            CreateFirstLeague(100);
            this.higherRank = null;
            this.logs = new List<Log>();
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

        //edit the gap field - syncronized 
        public bool EditLeagueGap(int newGap)
        {
            bool toReturn = false;
            lock (padlock)
            {
                if (!IsValidInputNotSmallerEqualZero(newGap))
                {
                    return toReturn;
                }
                try
                {
                    LeagueGap = newGap;
                    toReturn = true;
                }
                catch (Exception e)
                {
                    ErrorLog log = new ErrorLog("Error in edit league gap");
                    AddErrorLog(log);
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

        public bool saveActionFromGameReplay(int roomID, int gameID, int userID, int actionNum)
        {
            GameReplay gr = GetGameReplay(roomID, gameID, userID);
            if (gr == null)
            {
                return false;
            }
            TexasHoldem.Logic.Actions.Action action = gr.GetActionAt(actionNum);
            if (action == null)
            {
                return false;
            }
            User user = SystemControl.SystemControlInstance.GetUserWithId(userID);
            if (user == null)
            {
                return false;
            }
            return user.AddActionToFavorite(action);
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

        public int GetLastGameRoom()
        {
            return roomIdCounter;
        }

        //create new game room
        //game type  policy, limit, no-limit, pot-limit
        //אם הכסף של השחקן 
        public bool CreateNewRoom(int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen, int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (SystemControl.SystemControlInstance.GetUserWithId(userId) == null)
                {
                    //there is no such user
                    ErrorLog log = new ErrorLog("Error while trying to create room, there is no user with id: "+ userId);
                    AddErrorLog(log);
                    return toReturn;
                }
                if (!IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                if (startingChip < 0)
                {
                    //not valid value

                    return toReturn;
                }
                if (minPlayersInRoom <= 0)
                {
                    ErrorLog log = new ErrorLog("Error while trying to create room, mim amount of player is invalid - less or equal to zero");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (minBet <= 0)
                {
                    ErrorLog log = new ErrorLog("Error while trying to create room, min bet is invalid - less or equal to zero");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (maxPlayersInRoom <= 0)
                {
                    ErrorLog log = new ErrorLog("Error while trying to create room, Max amount of player is invalid - less or equal to zero");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (minPlayersInRoom > maxPlayersInRoom)
                {
                    ErrorLog log = new ErrorLog("Error while trying to create room, invalid input - min player in room is bigger than max player in room");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (enterPayingMoney < 0)
                {
                    ErrorLog log = new ErrorLog("Error while trying to create room, invalid input - the entering money of the player is a negative number");
                    AddErrorLog(log);
                    return toReturn;
                }
               /*
                if (startingChip == 0)
                {
                    return toReturn;
                }*/

                //get next valid room id
                int nextId = GetNextIdRoom();

                List<Player> players = new List<Player>();
               
                User user = SystemControl.SystemControlInstance.GetUserWithId(userId);   
                if (enterPayingMoney > 0)
                {
                    int newMoney = user.Money - enterPayingMoney;
                    user.Money = newMoney;
                }
                if (startingChip == 0)
                {
                    startingChip = user.Money;
                    user.Money = 0;
                }
                Player player = new Player(startingChip, 0, user.Id, user.Name, user.MemberName, user.Password, user.Points,
                    user.Money, user.Email, nextId);
                players.Add(player);
                ConcreteGameRoom room = new ConcreteGameRoom(players, startingChip, nextId, isSpectetor, gameModeChosen, minPlayersInRoom, maxPlayersInRoom, enterPayingMoney,minBet);
                Thread MyThread = new Thread(new ThreadStart(room._gm.Start));
                toReturn = AddRoom(room);
                return toReturn;
            }
            
        }

       
        public List<Tuple<int, int>> GetGamesAvailableForReplayByUser(int userID)
        {
            lock (padlock)
            {
                User user = SystemControl.SystemControlInstance.GetUserWithId(userID);

                if (user != null)
                {

                    return user._gamesAvailableToReplay;
                }
                return null;
            }
        }


        //return room by room if - suncronized due to for
        //return null if room id smaller than 0 or not found
        public GameRoom GetRoomById(int roomId)
        {
            lock (padlock)
            {
                GameRoom toReturn = null;
                if (!IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                foreach (GameRoom room in games)
                {
                    if (room._id == roomId)
                    {
                        toReturn = room;
                    }
                }
                return toReturn;
            }          
        }

        //return true if there is a room with this id
        public bool IsRoomExist(int roomId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                foreach (GameRoom room in games)
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
                if (!IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                GameRoom toRemove = GetRoomById(roomId);
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
                    ErrorLog log = new ErrorLog("Error while trying to remove game room");
                    AddErrorLog(log);
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //Add new room the games list
        public bool AddRoom(GameRoom roomToAdd)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (roomToAdd == null)
                {
                    return toReturn;
                }
                try
                {
                    this.games.Add(roomToAdd);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    ErrorLog log = new ErrorLog("Error while trying to add new room to game center");
                    AddErrorLog(log);
                    toReturn = false;
                }
                return toReturn;
            }
        }

        
       //Add Player to room
        public bool AddPlayerToRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                SystemControl sc = SystemControl.SystemControlInstance;
                if (!IsValidInputNotSmallerZero(roomId) || !IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                User user = sc.GetUserWithId(userId);

                if (user == null)
                {
                    return toReturn;
                }
                
                bool exist = IsRoomExist(roomId);
                if (!exist)
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - invalid input - there is no user with user id: "+ userId + "(user with id: " + userId + " to room: " + roomId);
                    AddErrorLog(log);
                    return toReturn;
                }
                GameRoom room = GetRoomById(roomId);
                if (room == null)
                {
                    return toReturn;
                }
                
                int MoneyAferBuyIn = user.Money - room._enterPayingMoney;
                int moneyAfterDecStartingChip = MoneyAferBuyIn - room._startingChip;
                if(MoneyAferBuyIn < 0) 
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with id: " + userId + " to room: " + roomId +"user dont have money to pay the buy in policey of this room");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (moneyAfterDecStartingChip < 0)
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with id: " + userId + " to room: " + roomId+" user dont have money to get sarting chip and buy in policey");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (SystemControl.SystemControlInstance.HasThisSpectetorGame(roomId, userId)) //user is spectetor cant be also a player
                {
                    ErrorLog log = new ErrorLog("Error while tring to add player to room - user with id: " + userId + " to room: " + roomId + " user is a spectetor in this room need to leave first than join game");
                    AddErrorLog(log);
                    return toReturn;
                }

                List<Player> players = room._players;
                
                int numOfPlayerInRoom = 0;
                foreach (Player p in players)
                {
                    if (p.isPlayerActive)
                    {
                        numOfPlayerInRoom++;
                    }
                }
                /*
                if (playerChipToEnterRoom < sb) //
                {
                    ErrorLog log = new ErrorLog("???????????????");
                    AddErrorLog(log);
                    
                    return toReturn;
                }*/
                if (numOfPlayerInRoom == room._maxPlayersInRoom)
                {
                    ErrorLog log = new ErrorLog("Error while trying to add player to room thaere is no place in the room - max amount of player tight now: "+ numOfPlayerInRoom+ "(user with id: " + userId + " to room: " + roomId);
                    AddErrorLog(log);
                    return toReturn;
                }
                if (!(user.Points <= room._maxRank && user.Points >= room._minRank))
                {
                    ErrorLog log = new ErrorLog("Error while trying to add player, user with id: " + userId + " to room: " + roomId + "user point: " + user.Points + " are not in this game critiria (nimRank , maxRank: " + room._minRank + ", " + room._maxRank);
                    AddErrorLog(log);
                    return toReturn;
                }
                int newMoney = moneyAfterDecStartingChip;
                user.Money = newMoney;
                /*if (playerChipToEnterRoom == 0)
                {
                    playerChipToEnterRoom = user.Money;
                    user.Money = 0;
                }*/
                Player playerToAdd = new Player(room._startingChip, 0, user.Id, user.Name, user.MemberName, user.Password, user.Points,
                    user.Money, user.Email, roomId);
               
                try
                {
                    GameRoom toAdd = room;
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
                    ErrorLog log = new ErrorLog("Error while trying to add player (user with id: "+userId+" to room: "+roomId);
                    AddErrorLog(log);
                    
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
                if (!IsValidInputNotSmallerZero(roomId) || !IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                SystemControl sc = SystemControl.SystemControlInstance;
                User user = sc.GetUserWithId(userId);
                bool exist = IsRoomExist(roomId);
                if (!exist)
                {
                    return toReturn;
                }
                //if user is player in room cant be also spectetor
                if (SystemControl.SystemControlInstance.HasThisActiveGame(roomId, userId))
                {
                    return toReturn;
                }
                GameRoom room = GetRoomById(roomId);
                try
                {
                    GameRoom toAdd = room;
                    User newUser = user; // add room to user list
                    newUser.SpectateGameList.Add(room);
                    if (toAdd._isSpectetor)
                    {
                        Spectetor spectetor = new Spectetor(user.Id, user.Name, user.MemberName, user.Password, user.Points,
                            user.Money, user.Email, roomId);
                        toAdd._spectatores.Add(spectetor);
                        //  sc.ReplaceUser(user, newUser);
                        // games.Remove(room);
                        //games.Add(toAdd);
                        toReturn = true;
                    }
                }
                catch (Exception e)
                {
                    ErrorLog log = new ErrorLog("Error while trying to add spectetor, (user with id: " + userId + " to room: " + roomId);
                    AddErrorLog(log);
                    toReturn = false;
                }
                return toReturn;
            }
        }


        //remove player from room

        public bool RemovePlayerFromRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(roomId) || !IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                bool exist = IsRoomExist(roomId);
                if (!exist)
                {
                    ErrorLog log = new ErrorLog("Error while trying to remove player from room, (user with id: " + userId + " from room: " + roomId + "user does not exist in this room");
                    AddErrorLog(log);
                    return toReturn;
                }
                SystemControl sc = SystemControl.SystemControlInstance;
                GameRoom room = GetRoomById(roomId);
                GameRoom toAdd = room;
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
                if (playerToRemove == null)//user is not in room
                {
                    return toReturn;
                }
                try
                {
                    playerToRemove._isInRoom = false; //not in room - need to remove in end od round
                    playerToRemove.IsActive = false;
                    //allPlayers.Remove(playerToRemove);
                    // toAdd._players = allPlayers;
                    //games.Remove(room);
                    //games.Add(toAdd);
                    int moneyToadd = playerToRemove._totalChip -playerToRemove._gameChip;
                    int newMoney = moneyToadd + user.Money;
                    user.Money = newMoney;
                    user.ActiveGameList.Remove(room);
                    //sc.ReplaceUser(user, newUser);
                    toReturn = true;
                }
                catch (Exception e)
                {
                    ErrorLog log = new ErrorLog("Error while trying to remove player from room, (user with id: " + userId + " from room: " + roomId + "user does not exist in this room");
                    AddErrorLog(log);
                    toReturn = false;
                }

                return toReturn;
            }
        }

        //todo - contine loging from here
        //remove spectetor from room - sycronized
        public bool RemoveSpectetorFromRoom(int roomId, int userId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                bool exist = IsRoomExist(roomId);
                if (!IsValidInputNotSmallerZero(roomId) || !IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                if (!exist)
                {
                    return toReturn;
                }
                SystemControl sc = SystemControl.SystemControlInstance;
                GameRoom room = GetRoomById(roomId);
                GameRoom toAdd = room;
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
                if (sprctetorToRemove == null)//spectetor not in room
                {
                    return toReturn;
                }
                try
                {
                    allSpectetors.Remove(sprctetorToRemove);
                    toAdd._spectatores = allSpectetors;
                   // games.Remove(room);
                    newUser.SpectateGameList.Remove(room);
                    //sc.ReplaceUser(user, newUser);
                   // games.Add(toAdd);
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
                if (!IsValidInputNotSmallerEqualZero(leagugap))
                {
                    return false;
                }
                int higherRank = HigherRank.Points;
                leagueTable = new List<League>();
                int currpoint = 0;
                int i = 1;
                int to = 0;
                String leaugeName;
                while (currpoint < higherRank)
                {
                    leaugeName = "" + i;
                    to = currpoint + leagueGap;
                    League toAdd = new League(leaugeName, currpoint, to);
                    leagueTable.Add(toAdd);
                    i++;
                    currpoint = to;
                }
                return true;
            }
        }

        //create new league whith new gap
        public bool CreateFirstLeague(int initGap)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerEqualZero(initGap))
                {
                    return toReturn;
                }
                
                LeagueGap = initGap;
                leagueTable = new List<League>();
                int currpoint = 0;
                int i = 1;
                int to = 0;
                String leaugeName;
                while (i < 100)
                {
                    leaugeName = "" + i;
                    to = currpoint + leagueGap;
                    League toAdd = new League(leaugeName, currpoint, to);
                    leagueTable.Add(toAdd);
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


        //Tuple<int, int> - <min,max>
        public Tuple<int,int> UserLeageGapPoint(int userId)
        {
            //Tuple<int, int> toReturn;
            User user = SystemControl.SystemControlInstance.GetUserWithId(userId);
            if (user == null)
            {
                return new Tuple<int, int>(-1, -1);
                //return toReturn;
            }

            int tempPoints = user.Points;
            int count = 0;
            while (tempPoints > 0)
            {
                tempPoints -= leagueGap;
                count++;
            }
            return new Tuple<int, int>(count * leagueGap, (count + 1) * leagueGap);
        }

        //get all active games - syncronized
        public List<GameRoom> GetAllActiveGame()
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                foreach (GameRoom room in games)
                {
                    if (room._isActiveGame)
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }
        }


        public List<GameRoom> GetAllSpectetorGame()
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                foreach (GameRoom room in games)
                {
                    if (room._isSpectetor)
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }
        }



        public List<GameRoom> GetAllGames()
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                foreach (GameRoom room in games)
                {
                    toReturn.Add(room);
                }
                return toReturn;
            }
        }


        //todo ?????? potCount =? postsize
        //return list of games with pot size
        public List<GameRoom> GetAllGamesByPotSize(int potSize)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                try
                {
                    if (!IsValidInputNotSmallerZero(potSize))
                    {
                        toReturn = null;
                        return toReturn;
                    }
                    foreach (GameRoom room in games)
                    {
                        if (room._potCount == potSize)
                        {
                            toReturn.Add(room);
                        }

                    }
                }
                catch (Exception e)
                {
                    toReturn = null;
                }
                return toReturn;
            }
        }

        //return list of games with game mode:
        //limit / no - limit / pot limit
        public List<GameRoom> GetGamesByGameMode(GameMode gm)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                foreach (GameRoom room in games)
                {
                    if (room._gameMode == gm)
                    {
                        toReturn.Add(room);
                    }

                }
                return toReturn;
            }
        }

        //return list of games by buy in policy
        public List<GameRoom> GetGamesByBuyInPolicy(int buyIn)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                try
                {
                    if (!IsValidInputNotSmallerZero(buyIn))
                    {
                        toReturn = null;
                        return toReturn;
                    }
                    foreach (GameRoom room in games)
                    {
                        if (room._enterPayingMoney == buyIn)
                        {
                            toReturn.Add(room);
                        }

                    }
                }
                catch (Exception e)
                {
                    toReturn = null;
                }
                
                return toReturn;
            }
        }

        //return list of games by min player in room
        public List<GameRoom> GetGamesByMinPlayer(int min)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                try
                {
                    if (!IsValidInputNotSmallerZero(min))
                    {
                        toReturn = null;
                        return toReturn;
                    }
                    foreach (GameRoom room in games)
                    {
                        if (room._minPlayersInRoom == min)
                        {
                            toReturn.Add(room);
                        }

                    }
                }
                catch (Exception e)
                {
                    toReturn = null;
                }
                
                return toReturn;
            }
        }


        //return list of games by max player in room
        public List<GameRoom> GetGamesByMaxPlayer(int max)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                try
                {
                    if (!IsValidInputNotSmallerEqualZero(max))
                    {
                        toReturn = null;
                        return toReturn;
                    }
                    foreach (GameRoom room in games)
                    {
                        if (room._maxPlayersInRoom == max)
                        {
                            toReturn.Add(room);
                        }

                    }
                }
                catch (Exception e)
                {
                    toReturn = null;
                }
               
                return toReturn;
            }
        }



        //return list of games by min bet in room
        //syncronized - due to for
        public List<GameRoom> GetGamesByMinBet(int minBet)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                try
                {
                    
                    if (!IsValidInputNotSmallerZero(minBet))
                    {
                        toReturn = null;
                        return toReturn;
                    }
                    foreach (GameRoom room in games)
                    {
                        if (room._minBetInRoom == minBet)
                        {
                            toReturn.Add(room);
                        }

                    }
                }
                catch (Exception e)
                {
                    toReturn = null;
                }
                
                return toReturn;
            }
        }


        //return list of games by starting chip policy
        //return null if startingChup <=0
        //syncronized - due to for
        public List<GameRoom> GetGamesByStartingChip(int startingChip)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                if (!IsValidInputNotSmallerZero(startingChip))
                {
                    toReturn = null;
                    return toReturn;
                }
                try
                {
                    
                    foreach (GameRoom room in games)
                    {
                        if (room._startingChip == startingChip)
                        {
                            toReturn.Add(room);
                        }

                    }
               
                }
                catch (Exception e)
                {
                    toReturn = null;
                }
                return toReturn;
            }
        }

        //chaeck if game is spectetable
       
        public bool IsGameCanSpectete(int roomId)
        {
            bool toReturn = false;
            GameRoom room = GetRoomById(roomId);
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
            GameRoom room = GetRoomById(roomId);
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
                return leagueGap;
            }

            set
            {
                lock (padlock)
                {
                    leagueGap = value;
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

        
        public Log FindLog(int logId)
        {
            lock (padlock)
            {
                Log toReturn = null;
                bool found = false;
                foreach (SystemLog sl in systemLog)
                {
                    if (sl.LogId == logId)
                    {
                        toReturn = sl;
                        found = true;
                    }
                }
                if (!found)
                {
                    foreach (ErrorLog el in errorLog)
                    {
                        if (el.LogId == logId)
                        {
                            toReturn = el;
                            found = true;
                        }
                    }
                }
                return toReturn;
            }
        }

        public void AddSystemLog(SystemLog log)
        {
            lock (padlock)
            {
                systemLog.Add(log);
            }
        }

        public void AddErrorLog(ErrorLog log)
        {
            lock (padlock)
            {
                errorLog.Add(log);
            }
        }



        public Tuple<Action, int> SendUserAvailableMovesAndGetChoosen(List<Tuple<Action, bool, int, int>> moves)
        {
            lock (padlock)
            {
                
                GameServiceHandler gsh = new GameServiceHandler();
                Tuple<Action, int> happend = gsh.SendUserAvailableMovesAndGetChoosen( moves);
                
                return happend;
            }
            
        }
        /*
        public Tuple<Action, int> SendUserAvailableMovesAndGetChoosenAcceptence(List<Tuple<Action, bool, int, int>> moves)
        {
            lock (padlock)
            {

                GameServiceHandler gsh = new GameServiceHandler();
                Tuple<Action, int> happend = gsh.SendUserAvailableMovesAndGetChoosen(moves);

                return happend;
            }

        }*/


        public String Displaymoves(List<Tuple<Action, bool, int, int>> moves)
        {
            lock (padlock)
            {
                string toReturn = "";
                foreach (Tuple<Action, bool, int, int> t in moves)
                {
                    String info = "";
                    if (t.Item2)
                    {
                        if (t.Item1 == Action.Bet)
                        {
                            info = info + "move avilble is: " + t.Item1 +
                                   " the game is limit holdem, so the bet is  - 'small bet' and equal " +
                                   "to big blind: " + t.Item4;
                        }
                        else if (t.Item1 == Action.Raise)
                        {
                            info = info + "move avilble is: " + t.Item1 +
                                   " the game is limit holdem, so the Raise is  - 'small bet' and equal " +
                                   "to big blind: " + t.Item4;
                        }
                    }
                    else
                    {
                        if (t.Item1 == Action.Bet)
                        {
                            info = info + "move avilble is: " + t.Item1 + "Raise must be withIn: " + t.Item3 +
                                   " and: " + t.Item4;
                        }
                        else if (t.Item1 == Action.Bet)
                        {
                            info = info + "move avilble is: " + t.Item1 + "Bet must be withIn: " + t.Item3 + " and: " +
                                   t.Item4;
                        }
                        else if (t.Item1 == Action.Call)
                        {
                            info = info + "move avilble is: " + t.Item1 + "the amount need to call is: " + t.Item3;
                        }
                        else if (t.Item1 == Action.Check)
                        {
                            info = info + "move avilble is: " + t.Item1;
                        }
                        else if (t.Item1 == Action.Fold)
                        {
                            info = info + "move avilble is: " + t.Item1;
                        }
                    }
                    Console.WriteLine(info);
                    toReturn = toReturn + "/n" + info;
                }
                return toReturn;
            }
        }

       
        /*
        //retun Action Selected and the sum for bet - bet is valid is after check/ 
        public List<Tuple<Action, int>> GetSelectedMoveFromPlayer(Action selected, int bet, int roomId, int playerId)
        {
            lock (padlock)
            {
                List<Tuple<Action, int>> toRetun = new List<Tuple<Action, int>>();

                return toRetun;
            }
        }
        */


      

        public bool IsValidMove(List<Tuple<Action, bool, int, int>> moves, Tuple<Action, int> moveAndBet)
        {
            lock (padlock)
            {
                bool toReturn = false;
                Action toCheck = moveAndBet.Item1;
                int betToCheck = moveAndBet.Item2;
                int maxBet = 0;
                int minBet = 0;
                bool isLimitGame = false;

                foreach (Tuple<Action, bool, int, int> tuple in moves)
                {
                    if (tuple.Item1 == toCheck)
                    {
                        isLimitGame = tuple.Item2;
                        minBet = tuple.Item4;
                        maxBet = tuple.Item3;
                    }
                }
                if (toCheck == Action.Bet || toCheck == Action.Raise)
                {
                    if (isLimitGame)
                    {
                        toReturn = (betToCheck == maxBet);
                        return toReturn;
                    }
                    else
                    {
                        toReturn = (betToCheck >= minBet) && (betToCheck <= maxBet);
                        return toReturn;
                    }
                }
                if (toCheck == Action.Call)
                {
                    toReturn = betToCheck == maxBet; //amount to call
                    return toReturn;
                }
                if (toCheck == Action.Fold)
                {
                    toReturn = betToCheck == maxBet && maxBet == -1;
                    return toReturn;
                }
                if (toCheck == Action.Check)
                {
                    toReturn = betToCheck == maxBet && maxBet == 0;
                    return toReturn;
                }
                return toReturn;
            }
        }

        public Tuple<Logic.Game.Action, int> GetRandomMove(List<Tuple<Action, bool, int, int>> moves)
        {
            lock (padlock)
            {
                int size = moves.Count;
                int selectedMove = GetRandomNumber(0, size);
                int bet = moves[selectedMove].Item3;
                Action selectedAction = moves[selectedMove].Item1;
                Tuple<Logic.Game.Action, int> toReturn = new Tuple<Action, int>(selectedAction, bet);
                return toReturn;
            }
        }

        private int GetRandomNumber(int minimum, int maximum)
        {
            lock (padlock)
            {
                Random random = new Random();
                return random.Next() * (maximum - minimum) + minimum;
            }
        }

        public Tuple<Action, int>  SendMoveBackToPlayer(Tuple<Action, int> moveAndBet)
        {
            return moveAndBet;
        }

        private bool IsValidInputNotSmallerEqualZero(int toCheck)
        {
            return toCheck > 0;
        }

        private bool IsValidInputNotSmallerZero(int toCheck)
        {
            return toCheck >= 0;
        }
        //return all games in the system 0 active and non active
        public List<GameRoom> GetGames()
        {
            return games;
        }


        public bool CreateNewRoomWithRoomId(int roomId, int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (SystemControl.SystemControlInstance.GetUserWithId(userId) == null)
                {
                    //there is no such user
                    ErrorLog log =
                        new ErrorLog("Error while trying to create room, there is no user with id: " + userId);
                    AddErrorLog(log);
                    return toReturn;
                }
                //TODO: Orelie Oded changed this
                if (!IsValidInputNotSmallerZero(userId))
                {
                    return toReturn;
                }
                if (startingChip < 0)
                {
                    //not valid value

                    return toReturn;
                }
                if (minPlayersInRoom <= 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, mim amount of player is invalid - less or equal to zero");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (minBet <= 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, min bet is invalid - less or equal to zero");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (maxPlayersInRoom <= 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, Max amount of player is invalid - less or equal to zero");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (minPlayersInRoom > maxPlayersInRoom)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, invalid input - min player in room is bigger than max player in room");
                    AddErrorLog(log);
                    return toReturn;
                }
                if (enterPayingMoney < 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, invalid input - the entering money of the player is a negative number");
                    AddErrorLog(log);
                    return toReturn;
                }
                /*
                 if (startingChip == 0)
                 {
                     return toReturn;
                 }*/

                //get next valid room id
                int nextId = roomId;

                List<Player> players = new List<Player>();

                User user = SystemControl.SystemControlInstance.GetUserWithId(userId);
                if (enterPayingMoney > 0)
                {
                    int newMoney = user.Money - enterPayingMoney;
                    user.Money = newMoney;
                }
                if (startingChip == 0)
                {
                    startingChip = user.Money;
                    user.Money = 0;
                }
                Player player = new Player(startingChip, 0, user.Id, user.Name, user.MemberName, user.Password,
                    user.Points,
                    user.Money, user.Email, nextId);
                players.Add(player);
                ConcreteGameRoom room = new ConcreteGameRoom(players, startingChip, nextId, isSpectetor, gameModeChosen,
                    minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
                Thread MyThread = new Thread(new ThreadStart(room._gm.Start));
                toReturn = AddRoom(room);
                return toReturn;
            }
        }
    }

    
}
