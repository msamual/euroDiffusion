using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euroDiffusion
{
    class Country
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

        public bool is_full()
        {
            return incompleteTowns.Count == 0;
        }
        public void addTown(Town town)
        {
            towns.Add(town);
            if (town.is_full() == false)
                incompleteTowns.Add(town);
        }
        
        public void registCompleteTown(Town town)
        {
            incompleteTowns.Remove(town);
            if (is_full())
                simulation.registFullCountry(this);
        }

        public string   getName() { return name; }

        public void     updateWalletsInTowns()
        {
            foreach (var town in towns)
                town.update_wallets();
        }
    }
}
