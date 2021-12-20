using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace euroDiffusion
{
    class Program
    {

        static void Main(string[] args)
        {
            Input input;
            try
            {
                if (args.Length < 1)
                {
                    input = new Input();
                }
                else
                {
                    input = new Input(args[0]);
                }
                for (int i = 0; i < input.getSize(); ++i)
                {
                    Simulation simulation = new Simulation(input.getData(i));

                    Console.Write("Case Number ");
                    Console.WriteLine(i + 1);
                    simulation.start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }


            while (true) ;
        }
    }
}
