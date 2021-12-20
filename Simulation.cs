using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euroDiffusion
{
    class Simulation
    {

        Country[]       countries;
        int             size;
        int             incompleteCountries;
        Country[]       completeToday;
        int             completeTodayCount;

        public      Simulation(inputStruct input)
        {
            this.size                   = input.countriesNumber;
            this.countries              = new Country[this.size];
            this.incompleteCountries    = size;
            this.completeToday          = new Country[size];
            this.completeTodayCount     = 0;

            for (int i = 0; i < input.countriesNumber; ++i)
            {
                this.countries[i] = new Country(input.countryNames[i],
                                            input.coordinates[i, 0],
                                            input.coordinates[i, 1],
                                            input.coordinates[i, 2],
                                            input.coordinates[i, 3],
                                            this);
            }
        }

        static void         print_map(Town[,] map, int y, int x)
        {
            for (int j = 0; j < y; ++j)
            {
                for (int i = 0; i < x; ++i)
                {
                    if (map[j, i] != null)
                        Console.Write("0 ");
                    else
                        Console.Write(". ");
                }
                Console.WriteLine();
            }
        }

        int[]           getExtrCoords()
        {
            int[] result = new int[4];

            int lowX = int.MaxValue, lowY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            foreach (var country in countries)
            {
                if (country.xl < lowX)
                    lowX = country.xl;
                if (country.yl < lowY)
                    lowY = country.yl;
                if (country.xh > maxX)
                    maxX = country.xh;
                if (country.yh > maxY)
                    maxY = country.yh;
            }
            result[0] = lowX;
            result[1] = lowY;
            result[2] = maxX;
            result[3] = maxY;

            return result;
        }

        void                init_country(Town[,] map, Country country, int[] extrCoords, int allCountryNumber)
        {
            int dx = extrCoords[0], dy = extrCoords[1];
            for (int y = country.yl - dy; y < country.yh - dy; ++y)
            {
                for (int x = country.xl - dx; x < country.xh - dx; ++x)
                {
                    map[y, x] = new Town(country, allCountryNumber);
                    country.addTown(map[y, x]);
                }
            }
            if (country.is_full())
                this.incompleteCountries--;
        }

        void                init_neighbourgs(Town[,] map, int y, int x)
        {
            for (int j = 0; j < y; ++j)
            {
                for (int i = 0; i < x; ++i)
                {
                    if (map[j, i] != null)
                    {
                        if (j < y - 1 && map[j + 1, i] != null)
                            map[j, i].setNeigbourgh(map[j + 1, i]);     //north neighbourg
                        if (i < x - 1 && map[j, i + 1] != null)
                            map[j, i].setNeigbourgh(map[j, i + 1]);     //east neighbourg
                        if (j > 0 && map[j - 1, i] != null)
                            map[j, i].setNeigbourgh(map[j - 1, i]);     //south neighbourg
                        if (i > 0 && map[j, i - 1] != null)
                            map[j, i].setNeigbourgh(map[j, i - 1]);     //west neighbourg
                    }
                }
            }
        }

        public void    registFullCountry(Country country)
        {
            this.completeToday[completeTodayCount++] = country;
            this.incompleteCountries--;
        }

        void            updateWalletsInCountries()
        {
            foreach (var country in countries)
                country.updateWalletsInTowns();
        }

        void            distribute(Town[,] map, int y, int x)
        {
            for (int j = 0; j < y; ++j)
            {
                for (int i = 0; i < x; ++i)
                {
                    if (map[j, i] != null)
                        map[j, i].distribute();
                }
            }
        }


        void            swap(int i)
        {
            Country tmp             = completeToday[i];
            completeToday[i]        = completeToday[i - 1];
            completeToday[i - 1]    = tmp;
        }

        void            sortNames()
        {
            int count;
            do
            {
                count = 0;
                for (int i = 1; i < this.completeTodayCount; ++i)
                {
                    if (string.Compare(this.completeToday[i].getName(), this.completeToday[i - 1].getName()) < 0)
                    {
                        swap(i);
                        ++count;
                    }
                }
            } while (count != 0);
        }

        void            printFinishedCountry(string name, int day)
        {
            Console.Write("  ");
            Console.Write(name);
            Console.Write(' ');
            Console.WriteLine(day);
        }

        void            loop_simulation(Town[,] map, int y, int x)
        {
            int day = 1;

            if (this.incompleteCountries < 1)
            {
                foreach (var country in countries)
                {
                    printFinishedCountry(country.getName(), 0);
                }
            }

            while (true)
            {
                if (day != 0)
                {
                    this.completeToday      = new Country[size];
                    this.completeTodayCount = 0;
                }
                this.distribute(map, y, x);
                this.updateWalletsInCountries();
                sortNames();
                for (int i = 0; i < this.completeTodayCount; ++i)
                {
                    printFinishedCountry(this.completeToday[i].getName(), day);
                }
                if (this.incompleteCountries < 1)
                    break;
                ++day;
            }
        }

        bool            graphTravers(Town town)
        {
            if (town.isVisited() == true)
                return true;
            town.setVisited(true);
            for (int i = 0; i < town.getNeigbCount(); ++i)
            {
                graphTravers(town.getNeighbourgs()[i]);
            }
            return true;
        }

        bool            isConnectedGraph(Town town)
        {
            graphTravers(town);
            foreach (var country in countries)
            {
                foreach (var t in country.getTowns())
                {
                    if (t.isVisited() == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public void     start()
        {
            int[] extrCoords = getExtrCoords();
            int y = extrCoords[3] - extrCoords[1];
            int x = extrCoords[2] - extrCoords[0];

            Town[,] map = new Town[y, x];
            foreach (var country in this.countries)
            {
                init_country(map, country, extrCoords, this.size);
            }
            this.init_neighbourgs(map, y, x);
            if (this.isConnectedGraph(map[countries[0].yl, countries[0].xl]) == false)
                throw new Exception("Error: the sities is not connected.");
            this.loop_simulation(map, y, x);
        }
    }
}
