using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euroDiffusion
{
    class Country : IComparer<Country>, IComparable<Country>
    {
        string              name;
        List<Town>          towns;
        public int          xl, yl, xh, yh;
        List<Town>          incompleteTowns;
        Simulation          simulation;

        public  Country(string name, int xl, int yl, int xh, int yh, Simulation simulation)
        {
            this.simulation = simulation;
            towns = new List<Town>();
            incompleteTowns = new List<Town>();
            this.name = name;
            this.xl = xl;
            this.yl = yl;
            this.xh = xh;
            this.yh = yh;
        }

        public int          Compare(Country left, Country right)
        {
            return string.Compare(left.getName(), right.getName());
        }

        public int          CompareTo(Country other)
        {
            return this.name.CompareTo(other.getName());
        }

        public bool         is_full()
        {
            return this.incompleteTowns.Count == 0;
        }
        public void         addTown(Town town)
        {
            this.towns.Add(town);
            if (town.is_full() == false)
                this.incompleteTowns.Add(town);
        }
        
        public void         registCompleteTown(Town town)
        {
            this.incompleteTowns.Remove(town);
            if (is_full())
                this.simulation.registFullCountry(this);
        }

        public string       getName()
        {
            return this.name;
        }

        public List<Town>   getTowns()
        {
            return this.towns;
        }

        public void         updateWalletsInTowns()
        {
            foreach (var town in this.towns)
                town.update_wallets();
        }
    }
}
