using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euroDiffusion
{
    class Program
    {
        static void Main(string[] args)
        {

            List<List<string>>  input = new List<List<string>>();
            
            while (true)
            {
                int countriesNumber = int.Parse(Console.ReadLine());
                if (countriesNumber == 0)
                    break;
                List<string> list = new List<string>();
                for (int i = 0; i < countriesNumber; ++i)
                {
                    list.Add(Console.ReadLine());
                }
                input.Add(list);
            }
            for (int i = 0; i < input.Count; ++i)
            {
                Console.Write("Case Number ");
                Console.WriteLine(i + 1);
                Simulation simulation = new Simulation(input[i]);

                simulation.start();
            }
            while (true) ;
        }
    }
}
