using System.Collections.Generic;

namespace euroDiffusion
{
    class Town
    {
        Dictionary<string, int> wallets;
        Dictionary<string, int> newWallets;
        List<Town> neigbourgs;
        int allCountriesNumber;
        bool full;
        Country country;

        public Town(Country country, int allCountriesNumber)
        {
            wallets = new Dictionary<string, int>();
            newWallets = new Dictionary<string, int>();
            neigbourgs = new List<Town>();
            this.country = country;
            wallets.Add(country.getName(), 1000000);
            this.allCountriesNumber = allCountriesNumber;
            full = false;
            if (allCountriesNumber == wallets.Count)
            {
                full = true;
                country.registCompleteTown(this);
            }
        }

        public void distribute()
        {
            foreach (var town in neigbourgs)
            {
                Dictionary<string, int> wallets = new Dictionary<string, int>(this.wallets);
                foreach (var wallet in wallets)
                {
                    if (wallet.Value > 999)
                    {
                        send(town, wallet.Key, this.wallets[wallet.Key] / 1000);
                    }
                }
            }
            foreach (var wallet in new Dictionary<string,int>(wallets))
            {
                int count = wallet.Value / 1000 * neigbourgs.Count;
                this.wallets[wallet.Key] -= count;
            }
        }

        public void receive(string contryName, int count)
        {
            if (newWallets.ContainsKey(contryName))
                newWallets[contryName] += count;
            else
                newWallets.Add(contryName, count);
        }

        private void send(Town town, string countryName, int count)
        {
            town.receive(countryName, count);
            //wallets[countryName] -= count;
        }

        public void setNeigbourgh(Town town)
        {
            neigbourgs.Add(town);
        }

        public bool is_full() { return full; }

        public string getCountry() { return country.getName(); }

        public void     update_wallets()
        {
            foreach (var wallet in newWallets)
            {
                if (wallets.ContainsKey(wallet.Key))
                    wallets[wallet.Key] += wallet.Value;
                else
                    wallets.Add(wallet.Key, wallet.Value);
            }
            newWallets = new Dictionary<string, int>();
            if (full == false && wallets.Count == allCountriesNumber)
            {
                full = true;
                country.registCompleteTown(this);
            }
        }
    }
}
