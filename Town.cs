using System.Collections.Generic;

namespace euroDiffusion
{
    class Town
    {
        public const int        SendLimit      = 1000;

        Wallet                  wallet;
        Wallet                  newWallet;
        Town[]                  neigbourgs;
        int                     neigbourgsCount;
        int                     allCountriesNumber;
        bool                    visited;
        bool                    full;
        Country                 country;

        public Town(Country country, int allCountriesNumber)
        {
            wallet              = new Wallet(country.getName(), allCountriesNumber);
            newWallet           = new Wallet(allCountriesNumber);
            neigbourgs          = new Town[4];
            this.country        = country;

            this.allCountriesNumber = allCountriesNumber;
            this.full = false;
            this.visited = false;
            if (wallet.isFull())
            {
                this.full = true;
                this.country.registCompleteTown();
            }
        }

        public void         distribute()
        {
            for (int j = 0; j < neigbourgsCount; ++j)
            {
                for (int i = 0; i < wallet.getSize(); ++i)
                {
                    if (this.wallet.at(i) >= SendLimit)
                    {
                        neigbourgs[j].receive(wallet.getName(i), this.wallet.at(i) / SendLimit);
                    }
                }
            }
            for (int i = 0; i < wallet.getSize(); ++i)
            {
                int count = this.wallet.at(i) / SendLimit * this.neigbourgsCount;
                this.wallet.add(i, -count);
            }
        }

        public void         update_wallets()
        {
            for (int i = 0; i < newWallet.getSize(); ++i)
            {
                wallet.add(newWallet.getName(i), newWallet.at(i));
            }
            newWallet = new Wallet(allCountriesNumber);
            if (!this.is_full() && this.wallet.isFull())
            {
                this.country.registCompleteTown();
                this.full = true;
            }
        }

        public void         receive(string contryName, int count)
        {
            this.newWallet.add(contryName, count);
        }


        public void         setNeigbourgh(Town town)
        {
            this.neigbourgs[this.neigbourgsCount++] = town;
        }

        public void         setVisited(bool visited)
        {
            this.visited = visited;
        }

        public Town[]       getNeighbourgs()
        {
            return this.neigbourgs;
        }

        public int          getNeigbCount()
        {
            return this.neigbourgsCount;
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
