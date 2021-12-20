using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace euroDiffusion
{
    public struct inputStruct
    {
        public int         countriesNumber;
        public string[]    countryNames;
        public int[,]      coordinates;

    }

    class Input
    {
        List<inputStruct>   data;
        char[]              separators;  

        public  Input()
        {
            reading(Console.In);
        }

        public  Input(string filePath)
        {
            if (File.Exists(filePath) == false)
                throw new Exception("file \"" + filePath + "\" doesn't exist");
            StreamReader file = new StreamReader(filePath);
            if (file.EndOfStream == true)
                throw new Exception("file is empty");
            this.data = new List<inputStruct>();
            this.separators = new char[2]{' ', '\t'};
            while (reading(file) != false)
            {
                ;
            }
        }

        private bool    reading(TextReader istream)
        {
            inputStruct inp = new inputStruct();
            string str = istream.ReadLine();
            if (int.TryParse(str, out inp.countriesNumber) == false) {
                throw new Exception("Incorrect countries number. It is not a number. : \"" + str + "\"");
            }
            if (inp.countriesNumber == 0)
                return false;
            if (inp.countriesNumber < 1) {
                throw new Exception("Incorrect countries number. Value should be more than 0");
            }
            if (inp.countriesNumber > 20) {
                throw new Exception("Incorrect countries number. Value can't be more than 20");
            }
            inp.countryNames = new string[inp.countriesNumber];
            inp.coordinates = new int[inp.countriesNumber, 4];
            for (int i = 0; i < inp.countriesNumber; ++i)
            {
                str = istream.ReadLine();
                parseString(str.Trim(), inp, i);
            }
            this.data.Add(inp);
            return true;
        }

        bool            isAllLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                    return false;
            }
            return true;
        }

        void parseString(string str, inputStruct inp, int i)
        {
            string[] country = str.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int xl, yl, xh, yh;

            if (country.Length != 5) {
                throw new Exception("Incorrect input : to few parameters : \"" + str + "\"");
            }
            if (country[0].Length > 25) {
                throw new Exception("Country name too long + : \"" + country[0] + "\"");
            }
            if (isAllLetters(country[0]) == false) {
                throw new Exception("Unexpected symbol in country name : \"" + country[0] + "\"");
            }
            inp.countryNames[i] = country[0];
            if (int.TryParse(country[1], out xl) == false) {
                throw new Exception("Incorrect input : xl is not a number : \"" + str + "\"");
            }
            if (int.TryParse(country[2], out yl) == false) {
                throw new Exception("Incorrect input : yl is not a number : \"" + str + "\"");
            }
            if (int.TryParse(country[3], out xh) == false) {
                throw new Exception("Incorrect input : xh is not a number : \"" + str + "\"");
            }
            if (int.TryParse(country[4], out yh) == false) {
                throw new Exception("Incorrect input : yh is not a number : \"" + str + "\"");
            }
            if (xl < 1 || xl > 20 || yl < 1 || yl > 20 || xh < 1 || xh > 20 || yh < 1 || yh > 20){
                throw new Exception("Incorrect input : coordinate is out of range 1 <= coordinate <= 20 : \"" + str + "\"");
            }
            inp.coordinates[i,0] = xl - 1;
            inp.coordinates[i,1] = yl - 1;
            inp.coordinates[i,2] = xh;
            inp.coordinates[i,3] = yh;
        }

        public inputStruct      getData(int i)
        {
            return this.data[i];
        }

        public int              getSize()
        {
            return this.data.Count;
        }
    }
}
