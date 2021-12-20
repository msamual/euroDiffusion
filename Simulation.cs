using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euroDiffusion
{
    class Simulation
    {

        List<Country> countries;
        List<Country> incompleteCountries;
        List<Country> completeToday;

        public      Simulation(inputStruct input)
        {
            this.countries           = new List<Country>(input.countriesNumber);
            this.incompleteCountries = new List<Country>(input.countriesNumber);
            this.completeToday       = new List<Country>(input.countriesNumber);
            for (int i = 0; i < input.countriesNumber; ++i)
            {
                this.countries.Add(new Country(input.countryNames[i],
                                            input.coordinates[i,0],
                                            input.coordinates[i,1],
                                            input.coordinates[i,2],
                                            input.coordinates[i,3],
                                            this));
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

        List<int>           getExtrCoords()
        {
            List<int> result = new List<int>();

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
            result.Add(lowX);
            result.Add(lowY);
            result.Add(maxX);
            result.Add(maxY);

            return result;
        }

        void                init_country(Town[,] map, Country country, List<int> extrCoords, int allCountryNumber)
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
            if (country.is_full() == false)
                this.incompleteCountries.Add(country);
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
            this.completeToday.Add(country);
            this.incompleteCountries.Remove(country);
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

            if (this.incompleteCountries.Count == 0)
            {
                foreach (var country in countries)
                {
                    printFinishedCountry(country.getName(), 0);
                }
            }

            while (true)
            {
                if (day != 0)
                    this.completeToday = new List<Country>();
                this.distribute(map, y, x);
                this.updateWalletsInCountries();
                this.completeToday.Sort();
                foreach (var country in this.completeToday)
                {
                    printFinishedCountry(country.getName(), day);
                }
                if (this.incompleteCountries.Count == 0)
                    break;
                ++day;
            }
        }

        bool            graphTravers(Town town)
        {
            if (town.isVisited() == true)
                return true;
            town.setVisited(true);
            for (int i = 0; i < town.getNeighbourgs().Count; ++i)
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
            List<int> extrCoords = getExtrCoords();
            int y = extrCoords[3] - extrCoords[1];
            int x = extrCoords[2] - extrCoords[0];

            Town[,] map = new Town[y, x];
            foreach (var country in this.countries)
            {
                init_country(map, country, extrCoords, this.countries.Count);
            }
            this.init_neighbourgs(map, y, x);
            if (this.isConnectedGraph(map[countries[0].yl, countries[0].xl]) == false)
                throw new Exception("Error: the sities is not connected.");
            this.loop_simulation(map, y, x);
        }
    }
}
