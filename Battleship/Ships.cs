using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Ship;

namespace Battleship
{
    public class Ships
    {
        public enum TypeShip
        {
            one_deck = 1,
            two_deck = 2,
            three_deck = 3,
            four_deck = 4
        }
        List<Ship> ships = new List<Ship>();
        public Ships(List<Ship> ships)
        {

            for (int i = 0; i < ships.Count; i++)
            {
                this.ships.Add(new Ship { Type = ships[i].Type, CountShip = ships[i].CountShip });
            }
            
        }
        public int GetCountShips()
        {
            int count = 0;
            foreach(var item in this.ships)
            {
                count += item.CountShip;
            }
            return count;
        }
        public List<Ship> GetShips()
        {
            return ships;
        }
        public Ship this[TypeShip index]
        {
            get
            {
                foreach (var item in ships)
                    if (item.Type == index)
                        return item;
                return null;
            }

            set {
                for (int i=0;i<ships.Count;i++)
                    if (ships[i].Type == index)
                        ships[i] = value;
                }
        }
    }
}
