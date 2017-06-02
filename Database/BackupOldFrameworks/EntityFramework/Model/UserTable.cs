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
    
    public partial class UserTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTable()
        {
            this.GameRooms = new HashSet<GameRoom>();
            this.GameRooms1 = new HashSet<GameRoom>();
            this.GameRooms2 = new HashSet<GameRoom>();
            this.GameRooms3 = new HashSet<GameRoom>();
            this.GameRooms4 = new HashSet<GameRoom>();
            this.Players = new HashSet<Player>();
            this.GameRooms5 = new HashSet<GameRoom>();
            this.GameRooms6 = new HashSet<GameRoom>();
            this.GameRooms7 = new HashSet<GameRoom>();
        }
    
        public int userId { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
        public int points { get; set; }
        public int money { get; set; }
        public int gamesPlayed { get; set; }
        public int leagueName { get; set; }
        public int winNum { get; set; }
        public int HighestCashGainInGame { get; set; }
        public int TotalProfit { get; set; }
        public bool inActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Player> Players { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms6 { get; set; }
        public virtual GameRoom GameRoom { get; set; }
        public virtual GameRoom GameRoom1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameRoom> GameRooms7 { get; set; }
    }
}
