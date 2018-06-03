using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tax_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            const string version = "0.1d";
            string prompt = $"TAX CALCULATOR [{ version }] % ";
            Console.CursorSize = 100; // Block Cursor
            while (true)
            {
                Console.Write(prompt);
                CommandParser.DoParse(Console.ReadLine());
            }
        }
    }
}
