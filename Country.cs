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
        Town[]              towns;
        int                 incompleteTowns;
        int                 countTowns;
        public int          xl, yl, xh, yh;
        Simulation          simulation;

        public  Country(string name, int xl, int yl, int xh, int yh, Simulation simulation)
        {
            this.simulation = simulation;
            towns           = new Town[(xh - xl) * (yh - yl)];
            incompleteTowns = 0;
            this.countTowns = 0;
            this.name       = name;
            this.xl         = xl;
            this.yl         = yl;
            this.xh         = xh;
            this.yh         = yh;
        }

        public Country(Country other)
        {
            this.simulation         = other.simulation;
            this.towns              = other.towns;
            this.incompleteTowns    = other.incompleteTowns;
            this.countTowns         = other.countTowns;
            this.name               = other.name;
            this.xl                 = other.xl;
            this.yl                 = other.yl;
            this.xh                 = other.xh;
            this.yh                 = other.yh;
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
            return this.incompleteTowns < 1;
        }
        public void         addTown(Town town)
        {
            this.towns[this.countTowns++] = town;
            if (!town.is_full())
                this.incompleteTowns++;
        }
        
        public void         registCompleteTown()
        {
            this.incompleteTowns--;
            if (is_full())
                this.simulation.registFullCountry(this);
        }

        public string       getName()
        {
            return this.name;
        }

        public Town[]      getTowns()
        {
            return this.towns;
        }

        public int         getSize()
        {
            return this.countTowns;
        }

        public void         updateWalletsInTowns()
        {
            foreach (var town in this.towns)
                town.update_wallets();
        }
    }
}
