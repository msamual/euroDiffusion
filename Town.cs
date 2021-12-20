using System.Collections.Generic;

namespace euroDiffusion
{
    class Town
    {
        public const int StartCapital = 1000000;
        public const int SendLimit = 1000;


        Dictionary<string, int> wallets;
        Dictionary<string, int> newWallets;
        List<Town>              neigbourgs;
        int                     allCountriesNumber;
        bool                    visited;
        bool                    full;
        Country                 country;

        public Town(Country country, int allCountriesNumber)
        {
            wallets         = new Dictionary<string, int>();
            newWallets      = new Dictionary<string, int>();
            neigbourgs      = new List<Town>();
            this.country    = country;

            this.wallets.Add(country.getName(), StartCapital);
            this.allCountriesNumber = allCountriesNumber;
            this.full = false;
            this.visited = false;
            if (allCountriesNumber == this.wallets.Count)
            {
                this.full = true;
                this.country.registCompleteTown(this);
            }
        }

        public void         distribute()
        {
            foreach (var town in this.neigbourgs)
            {
                foreach (var countryName in this.wallets.Keys)
                {
                    if (this.wallets[countryName] >= SendLimit)
                    {
                        town.receive(countryName, this.wallets[countryName] / SendLimit);
                    }
                }
            }
            foreach (var countryName in new List<string>(wallets.Keys))
            {
                int count = this.wallets[countryName] / SendLimit * this.neigbourgs.Count;
                this.wallets[countryName] -= count;
            }
        }

        public void         update_wallets()
        {
            foreach (var wallet in this.newWallets)
            {
                if (this.wallets.ContainsKey(wallet.Key))
                    this.wallets[wallet.Key] += wallet.Value;
                else
                    this.wallets.Add(wallet.Key, wallet.Value);
            }
            this.newWallets = new Dictionary<string, int>();
            if (this.full == false && this.wallets.Count == this.allCountriesNumber)
            {
                this.full = true;
                this.country.registCompleteTown(this);
            }
        }

        public void         receive(string contryName, int count)
        {
            if (this.newWallets.ContainsKey(contryName))
                this.newWallets[contryName] += count;
            else
                this.newWallets.Add(contryName, count);
        }


        public void         setNeigbourgh(Town town)
        {
            this.neigbourgs.Add(town);
        }

        public void         setVisited(bool visited)
        {
            this.visited = visited;
        }

        public List<Town>   getNeighbourgs()
        {
            return this.neigbourgs;
        }

        public bool         is_full()
        {
            return this.full;
        }

        public string       getCountry()
        {
            return country.getName();
        }

        public bool         isVisited()
        {
            return this.visited;
        }

    }
}
