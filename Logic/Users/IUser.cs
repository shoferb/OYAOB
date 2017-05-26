﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Game;
using TexasHoldem.Logic.GameControl;
using TexasHoldem.Logic.Notifications_And_Logs;

namespace TexasHoldem.Logic.Users
{
    public interface IUser
    {
        //return true if playes in less than 11 games;
        bool IsUnKnow();
        //for testing only
        bool SetPoints(int amount);
        //inc num of games play
        bool IncGamesPlay();

        int Id();
       
        String Name();
       
        String MemberName();
        
        string Password();
       
        String Avatar();
        
        int Points();
     
        int Money();

         List<Notification> WaitListNotification();

        string Email();

        List<Tuple<int, int>> GamesAvailableToReplay();

        List<IGame> ActiveGameList();

        List<IGame> SpectateGameList();

        int WinNum { get; set; }

        bool IncWinNum();

        bool IncLoseNum();

        int LoseNum { get; set; }

        int HighestCashGainInGame { get; set; }

        void UpdateHighestCashInGame(int cashToChck);

        int TotalProfit { get; set; }

        void UpdateTotalProfit(int profit);

        double GetAvgProft();

        double GetWinRate();

        bool Login();

        bool Logout();

        bool EditId(int Id);

        bool EditEmail(string email);

        bool EditPassword(string password);

        bool  EditUserName(string username);

        bool EditName(string name);

        bool EditAvatar(string path);

        bool EditUserPoints(int point);

        bool EditUserMoney(int money);

        bool RemoveRoomFromActiveGameList(IGame game);
        
        bool RemoveRoomFromSpectetorGameList(IGame game);

        bool HasThisActiveGame(IGame game);

        bool HasThisSpectetorGame(IGame game);

        bool AddRoomToActiveGameList(IGame game);

        bool AddRoomToSpectetorGameList(IGame game);

        bool IsLogin();

        bool ReduceMoneyIfPossible(int amount);

        void AddMoney(int amount);

        bool SendNotification(Notification toSend);

        bool AddNotificationToList(Notification toAdd);

        LeagueName GetLeague();

        void SetLeague(LeagueName league);

        bool HasEnoughMoney(int startingChip, int fee);

        
    }
}
