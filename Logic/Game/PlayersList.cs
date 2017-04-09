using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Users;

namespace TexasHoldem.Logic.Game
{
    public class PlayersList :IList<Player>
    {
        List<Player> list = new List<Player>();

        public PlayersList(PlayersList pl)
        {
            this.list.AddRange(pl);
        }

        public PlayersList()
        {
        }

        public IEnumerator<Player> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Player item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Player item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Player[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);

        }

        public bool Remove(Player item)
        {
            return list.Remove(item);
        }

        public bool IsReadOnly { get; }
        public int IndexOf(Player p)
        {
            return list.IndexOf(p);
        }

        public void Insert(int index, Player item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public Player this[int index]
        {
            get
            {
                while (index > list.Count() - 1)
                    index -= list.Count();
                while (index < 0)
                    index += list.Count();
                return list[index];
            }
            set
            {
                while (index > list.Count() - 1)
                    index -= list.Count();
                while (index < 0)
                    index += list.Count();
                list[index] = value;
            }
        }

        public Player GetPlayer(ref int index)
        {
            //wrap index and changing the index passed through
            while (index > list.Count() - 1)
                index -= list.Count();
            while (index < 0)
                index += list.Count();
            return list[index];
        }
        
        public void AddRange(PlayersList players)
        {
            list.AddRange(players);
        }
        public int Count
        {
            get { return list.Count; }
        }

        public void ResetPlayers()
        {
            foreach (Player player in this)
                player.Reset();
        }

        public void Sort()
        {
            list.Sort();
        }

    }
}
