//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TexasHoldem.Database.BackupOldFrameworks.EntityFramework.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Player
    {
        public int room_Id { get; set; }
        public int user_Id { get; set; }
        public bool is_player_active { get; set; }
        public string player_name { get; set; }
        public int Total_chip { get; set; }
        public int Round_chip_bet { get; set; }
        public bool Player_action_the_round { get; set; }
        public int first_card { get; set; }
        public int secund_card { get; set; }
        public int Game_Id { get; set; }
    
        public virtual GameRoom GameRoom { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}
