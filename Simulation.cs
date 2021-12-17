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

        public      Simulation(List<string> input)
        {
            input.Sort();
            countries           = new List<Country>();
            incompleteCountries = new List<Country>();
            completeToday       = new List<Country>();
            foreach (var str in input)
            {
                string[] arr = str.Split(new[] { ' ', '\t', ',', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                string countryName = arr[0];
                int xl = int.Parse(arr[1]) - 1, yl = int.Parse(arr[2]) - 1, xh = int.Parse(arr[3]), yh = int.Parse(arr[4]);
                countries.Add(new Country(countryName, xl, yl, xh, yh, this));
            }
        }

        static void print_map(Town[,] map, int y, int x)
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

        List<int> getExtrCoords()
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

        void init_country(Town[,] map, Country country, List<int> extrCoords, int allCountryNumber)
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
                incompleteCountries.Add(country);
        }

        void init_neighbourgs(Town[,] map, int y, int x)
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
            completeToday.Add(country);
            incompleteCountries.Remove(country);
        }

        void    updateWalletsInCountries()
        {
            foreach (var country in countries)
                country.updateWalletsInTowns();
        }

        void    distribute(Town[,] map, int y, int x)
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

        void loop_simulation(Town[,] map, int y, int x)
        {
            int day = 1;

            if (incompleteCountries.Count == 0)
            {
                foreach(var country in countries)
                {
                    Console.Write(country.getName());
                    Console.Write(' ');
                    Console.WriteLine(0);
                }
            }

            while (true)
            {
                if (day != 0)
                    completeToday = new List<Country>();
                distribute(map, y, x);
                updateWalletsInCountries();
                foreach (var country in completeToday)
                {
                    Console.Write(country.getName());
                    Console.Write(' ');
                    Console.WriteLine(day);
                }
                if (incompleteCountries.Count == 0)
                    break;
                ++day;
            }
        }


        public void     start()
        {
            List<int> extrCoords = getExtrCoords();
            int y = extrCoords[3] - extrCoords[1];
            int x = extrCoords[2] - extrCoords[0];

            Town[,] map = new Town[y, x];
            foreach (var country in countries)
            {
                init_country(map, country, extrCoords, countries.Count);
            }
            init_neighbourgs(map, y, x);
            loop_simulation(map, y, x);
        }
    }
}
