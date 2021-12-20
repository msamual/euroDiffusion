using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euroDiffusion
{
    class Wallet
    {
        public const int StartCoinsNumber = 1000000;

        string[]    countryNames;
        int[]       counts;
        int         size;
        int         capacity;

        public  Wallet(string countryName, int allCountriesNumber)
        {
            this.countryNames    = new string[allCountriesNumber];
            this.counts          = new int[allCountriesNumber];
            this.capacity        = allCountriesNumber;
            this.size            = 1;

            this.countryNames[0] = countryName;
            this.counts[0]       = StartCoinsNumber;
        }

        public  Wallet(int allCountriesNumber)
        {
            this.countryNames = new string[allCountriesNumber];
            this.counts = new int[allCountriesNumber];
            this.capacity = allCountriesNumber;
            this.size = 0;
        }

        int             find(string key)
        {
            for (int i = 0; i < this.size; ++i)
            {
                if (this.countryNames[i] == key)
                    return i;
            }
            return -1;
        }

        public bool     contains(string key)
        {
            int res = find(key);
            if (res == -1)
                return false;
            return true;
        }

        public int      at(string key)
        {
            int res = find(key);

            if (res == -1)
                return res;
            return this.counts[res];
        }

        public int at(int i)
        {
            if (i < 0 || i > size)
                throw new Exception("Error: Wallet.at() : index is out of range : \"" + i + "\"");
            return this.counts[i];
        }

        public void     add(string key, int count)
        {
            int i = this.find(key);
            if (i == -1)
            {
                if (size == capacity)
                    throw new Exception("Wallet overflow.");
                this.countryNames[size] = key;
                this.counts[size++] = count;
            }
            else
            {
                this.counts[i] += count;
            }
        }

        public void     add(int i, int count)
        {
            if (i < 0 || i > size)
                throw new Exception("Error: Wallet.add() : index is out of range : \"" + i + "\"");
            this.counts[i] += count;
        }

        public bool     isFull()
        {
            return this.size == this.capacity;
        }

        public string[] getNames()
        {
            return countryNames;
        }

        public string   getName(int i)
        {
            if (i < 0 || i > size)
                throw new Exception("Error: Wallet.getName() : index is out of range : \"" + i + "\"");
            return this.countryNames[i];
        }

        public int      getSize()
        {
            return size;
        }
    }
}
