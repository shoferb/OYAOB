using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;
using TexasHoldem.Logic.Users;
using TexasHoldem.Service;
using TexasHoldemShared.CommMessages;

namespace TexasHoldem.Logic.Game_Control
{
    public class  GameCenter
    {
        private List<League> leagueTable;
        private List<Log> logs;
        private User higherRank;
        public int leagueGap { get; set; }
        private List<GameRoom> games;
        
        private static int roomIdCounter = 1;
        private static GameCenter singlton;
        private SystemControl _systemControl = SystemControl.SystemControlInstance;

        private static GameCenter instance;
        private LogControl logControl = LogControl.Instance;

        private static readonly object padlock = new object();

        private GameCenter()
        {
            this.leagueTable = new List<League>();
            CreateFirstLeague(100);
            this.higherRank = null;
            this.logs = new List<Log>();
            this.games = new List<GameRoom>();            
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


        public bool DoAction(IUser user, CommunicationMessage.ActionType action, int amount, int roomId)
        {
            GameRoom gm = GetRoomById(roomId);

            return gm.DoAction(user, action, amount);
        }

        public void SendMessageToClient(IUser player, int roomId, CommunicationMessage.ActionType action, bool isSucceed, string msg)
        {
            GameServiceHandler.sendMessageToClient(player, roomId, action, isSucceed, msg);
        }







        //TODO: fix this
        public bool CreateNewRoomWithRoomId(int roomId, int userId, int startingChip, bool isSpectetor, GameMode gameModeChosen,
            int minPlayersInRoom, int maxPlayersInRoom, int enterPayingMoney, int minBet)
        {
            lock (padlock)
            {
                if (this._systemControl.GetUserWithId(userId) == null)
                {
                    ErrorLog log =
                        new ErrorLog("Error while trying to create room, there is no user with Id: " + userId);
                    logControl.AddErrorLog(log);
                    return false;
                }
               
                if (startingChip < 0)
                {
                    return false;
                }

                if (minPlayersInRoom <= 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, mim amount of player is invalid - less or equal to zero");
                    logControl.AddErrorLog(log);
                    return false;
                }

                if (minBet <= 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, min bet is invalid - less or equal to zero");
                    logControl.AddErrorLog(log);
                    return false;
                }
                if (maxPlayersInRoom <= 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, Max amount of player is invalid - less or equal to zero");
                    logControl.AddErrorLog(log);
                    return false;
                }
                if (minPlayersInRoom > maxPlayersInRoom)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, invalid input - min player in room is bigger than max player in room");
                    logControl.AddErrorLog(log);
                    return false;
                }
                if (enterPayingMoney < 0)
                {
                    ErrorLog log = new ErrorLog(
                        "Error while trying to create room, invalid input - the entering money of the player is a negative number");
                    logControl.AddErrorLog(log);
                    return false;
                }
            
                List<Player> players = new List<Player>();
                IUser user = SystemControl.SystemControlInstance.GetUserWithId(userId);

                if (enterPayingMoney > 0)
                {
                    int newMoney = user.Money() - enterPayingMoney;
                    user.EditUserMoney(newMoney);
                }

                if (startingChip == 0)
                {
                    startingChip = user.Money();
                    user.EditUserMoney(0);
                }
                Player player = new Player(startingChip, 0, user.Id, user.Name, user.MemberName, user.Password,
                    user.Points,
                    user.Money, user.Email, nextId);
                players.Add(player);
                player._isInRoom = false;
                GameRoom room = new GameRoom(players, startingChip, nextId, isSpectetor, gameModeChosen,
                    minPlayersInRoom, maxPlayersInRoom, enterPayingMoney, minBet);
                room.SetThread(new Thread(room._gm.ThreadPlay));
                // user.ActiveGameList.Add(room);
                user.AddRoomToActiveGameList(room);
                toReturn = AddRoom(room);
                return toReturn;
            }
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
                    logControl.AddErrorLog(log);
                    toReturn = false;
                }
            }
            return toReturn;
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


        public List<GameRoom> GetAllActiveGamesAUserCanJoin(int userId)
        {
            lock (padlock)
            {
                List<GameRoom> toReturn = new List<GameRoom>();
                List<GameRoom> tempList = GetAllActiveGame();
                IUser user = SystemControl.SystemControlInstance.GetUserWithId(userId);
                foreach (GameRoom room in games)
                {
                    if (room.MaxRank <= user.Points() && room.MinRank >= user.Points())
                    {
                        toReturn.Add(room);
                    }
                }
                return toReturn;
            }

        }

        //return room by room if - suncronized due to for
        //return null if room Id smaller than 0 or not found
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
                    if (room.Id == roomId)
                    {
                        toReturn = room;
                        return toReturn;
                    }
                }
                return toReturn;
            }          
        }

        public int CurrRoomId()
        {
            return roomIdCounter;
        }
        //return true if there is a room with this Id
        public bool IsRoomExist(int roomId)
        {
            lock (padlock)
            {
                bool toReturn = false;
                if (!IsValidInputNotSmallerZero(roomId))
                {
                    return toReturn;
                }
                GameRoom room = GetRoomById(roomId);
                List<GameRoom> all = GetAllGames();
                toReturn = all.Contains(room);
                /*
                foreach (GameRoom room in games)
                {
                    
                    if (room.Id == roomId)
                    {
                        toReturn = true;
                    }
                }*/
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
                    foreach (Player p in toRemove.Players)
                    {
                        userId = p.user.Id();
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
                    logControl.AddErrorLog(log);
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
                    logControl.AddErrorLog(log);
                    toReturn = false;
                }
                return toReturn;
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
                
                leagueTable = new List<League>();
                int currpoint = 0;
                int i = 1;
                int to = 0;
                String leaugeName=null;
                int gap = 0;
                League l = null;
                while (i < this.LeagueTable.Count)
                {
                    gap= this.LeagueTable[i].getMaxRank() - this.LeagueTable[i].getMinRank();
                    l = this.LeagueTable[i];
                    if (gap > initGap)
                    {
                        i++;
                    }
                    leaugeName = "" + i;
                }
                League toAdd =null ;
                if (l != null)
                {
                     toAdd = new League(leaugeName, l.getMaxRank(), l.getMaxRank() + initGap);
                }
                else
                {
                     toAdd = new League(leaugeName, 0, initGap);
                }
                leagueTable.Add(toAdd);
                this.LeagueTable = leagueTable;
                this.leagueGap = initGap;
                
            
                return toReturn;
            }
        }

        public string UserLeageInfo(User user)
        {
            string toReturn = "";
            int userRank = user.Points();
            int i = 1;
            int min = 0;
            int max = leagueGap;
            bool flag = ((userRank >= min) && (userRank < max));
            while (!flag)
            {
                if ((userRank >= min) && (userRank < max))
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
            IUser user = SystemControl.SystemControlInstance.GetUserWithId(userId);
            if (user == null)
            {
                return new Tuple<int, int>(-1, -1);
                //return toReturn;
            }

            int tempPoints = user.Points();
            int count = 0;
            while (tempPoints > 0)
            {
                tempPoints -= leagueGap;
                count++;
            }
            return new Tuple<int, int>(count * leagueGap, (count + 1) * leagueGap);
        }


        //get all active games - syncronized
        public List<IGame> GetAllActiveGame()
        {
            lock (padlock)
            {
                List<IGame> toReturn = new List<IGame>();
                foreach (IGame room in games)
                {
                    if (room.IsActiveGame())
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
                    if (room.IsSpectetor)
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
                return games;
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
                        if (room.PotCount == potSize)
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
                    if (room.GameMode == gm)
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
                        if (room.EnterPayingMoney == buyIn)
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
                        if (room.MinPlayersInRoom == min)
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
                        if (room.MaxPlayersInRoom == max)
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
                        if (room.MinBetInRoom == minBet)
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
                        if (room.StartingChip == startingChip)
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
            if (room.IsSpectetor)
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
            if (room.IsActiveGame)
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

        public List<GameRoom> Games
        {
            get
            {

                return games;
            }

            set
            {
                lock (padlock)
                {
                    games = value;
                }
            }
        }
        
        //TODO: Should not be here. call from GameRoom should be direct to service
        public Tuple<GameMove, int> SendUserAvailableMovesAndGetChoosen(List<Tuple<GameMove, bool, int, int>> moves)
        {
            lock (padlock)
            {
                
                GameServiceHandler gsh = new GameServiceHandler();
                Tuple<GameMove, int> happend = gsh.SendUserAvailableMovesAndGetChoosen( moves);
                
                return happend;
            }
            
        }
       
        //TODO: maybe not needed?
        public String Displaymoves(List<Tuple<GameMove, bool, int, int>> moves)
        {
            lock (padlock)
            {
                string toReturn = "";
                foreach (Tuple<GameMove, bool, int, int> t in moves)
                {
                    String info = "";
                    if (t.Item2)
                    {
                        if (t.Item1 == GameMove.Bet)
                        {
                            info = info + "move avilble is: " + t.Item1 +
                                   " the game is limit holdem, so the bet is  - 'small bet' and equal " +
                                   "to big blind: " + t.Item4;
                        }
                        else if (t.Item1 == GameMove.Raise)
                        {
                            info = info + "move avilble is: " + t.Item1 +
                                   " the game is limit holdem, so the Raise is  - 'small bet' and equal " +
                                   "to big blind: " + t.Item4;
                        }
                    }
                    else
                    {
                        if (t.Item1 == GameMove.Bet)
                        {
                            info = info + "move avilble is: " + t.Item1 + "Raise must be withIn: " + t.Item3 +
                                   " and: " + t.Item4;
                        }
                        else if (t.Item1 == GameMove.Bet)
                        {
                            info = info + "move avilble is: " + t.Item1 + "Bet must be withIn: " + t.Item3 + " and: " +
                                   t.Item4;
                        }
                        else if (t.Item1 == GameMove.Call)
                        {
                            info = info + "move avilble is: " + t.Item1 + "the amount need to call is: " + t.Item3;
                        }
                        else if (t.Item1 == GameMove.Check)
                        {
                            info = info + "move avilble is: " + t.Item1;
                        }
                        else if (t.Item1 == GameMove.Fold)
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

        //should be in decorator inside gameRoom
      public bool IsValidMove(List<Tuple<GameMove, bool, int, int>> moves, Tuple<GameMove, int> moveAndBet)
        {
            lock (padlock)
            {
                bool toReturn = false;
                GameMove toCheck = moveAndBet.Item1;
                int betToCheck = moveAndBet.Item2;
                int maxBet = 0;
                int minBet = 0;
                bool isLimitGame = false;

                foreach (Tuple<GameMove, bool, int, int> tuple in moves)
                {
                    if (tuple.Item1 == toCheck)
                    {
                        isLimitGame = tuple.Item2;
                        minBet = tuple.Item4;
                        maxBet = tuple.Item3;
                    }
                }
                if (toCheck == GameMove.Bet || toCheck == GameMove.Raise)
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
                if (toCheck == GameMove.Call)
                {
                    toReturn = betToCheck == maxBet; //amount to call
                    return toReturn;
                }
                if (toCheck == GameMove.Fold)
                {
                    toReturn = betToCheck == maxBet && maxBet == -1;
                    return toReturn;
                }
                if (toCheck == GameMove.Check)
                {
                    toReturn = betToCheck == maxBet && maxBet == 0;
                    return toReturn;
                }
                return toReturn;
            }
        }

        //TODO: no need for this any more (after we have client)
        public Tuple<Logic.Game.GameMove, int> GetRandomMove(List<Tuple<GameMove, bool, int, int>> moves)
        {
            lock (padlock)
            {
                int size = moves.Count;
                int selectedMove = GetRandomNumber(0, size);
                int bet = moves[selectedMove].Item3;
                GameMove selectedGameMove = moves[selectedMove].Item1;
                Tuple<Logic.Game.GameMove, int> toReturn = new Tuple<GameMove, int>(selectedGameMove, bet);
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

        //TODO: should 
        public Tuple<GameMove, int>  SendMoveBackToPlayer(Tuple<GameMove, int> moveAndBet)
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

     
    }

    
}
