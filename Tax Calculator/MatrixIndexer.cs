using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tax_Calculator
{
    class MatrixIndexer
    {
        public string[,] PrimaryMatrix;
        public MatrixIndexer()
        {
        }

        public void SetPrimaryMatrix(string[,] matrix) => this.PrimaryMatrix = matrix;
        public int sx = 0; // Selected X
        public int sy = 0; // Selected Y
        public int Pairs = 0; // Selected Pair
        public int[,] matrix { get; set; }

        public void SetMatrix(int height)
        {
            this.matrix = new int[2, height];
            for (int y = 0; y < height; y++)
            {
                int x = 0;
                this.matrix[x, y] = 0; x++;
                this.matrix[x, y] = 0;
                this.Pairs = height;
            }
        }

        public System.Collections.Hashtable GetHashtable()
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            for (int y = 0; y < Pairs; y++)
            {
                table.Add(PrimaryMatrix[0, y], PrimaryMatrix[1, y]);
            }
            return table;
        }

        /// <summary>
        /// Hanldes rendering data to user and also hanldes all UI input such as editing values and moving around the visual matrix
        /// </summary>
        public void DoUI()
        {
            const string Banner = "VISUAL DATA MATRIX EDITOR. <TAB> TO EXIT\n";
            Console.CursorVisible = false;
            bool DO = true;
            while (DO)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Banner);
                Console.ForegroundColor = ConsoleColor.White;
                for (int y = 0; y < Pairs; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        string output = "";
                        const int BufferSize = 20;

                        output = PrimaryMatrix[x, y];
                        for (int e = 0; e < (BufferSize - output.Length); e++)
                        {
                            output += " ";
                        }

                        if (matrix[x, y] == 1)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        Console.Write(output);
                    }
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        UP();
                        break;
                    case ConsoleKey.DownArrow:
                        DOWN();
                        break;
                    case ConsoleKey.LeftArrow:
                        LEFT();
                        break;
                    case ConsoleKey.RightArrow:
                        RIGHT();
                        break;
                    case ConsoleKey.Tab:
                        DO = false;
                        break;
                    case ConsoleKey.Enter:
                        PrimaryMatrix[sx, sy] = UpdateValue();
                        break;

                }
            }
        }

        private string UpdateValue()
        {
            Console.CursorVisible = true;
            Console.Write("> ");
            string input = Console.ReadLine();
            Console.CursorVisible = false;
            return input;
        }

        public void UP()
        {
            if (sy > 0)
            {
                this.matrix[sx, sy] = 0;
                sy--;
                this.matrix[sx, sy] = 1;
            }
        }

        public void RIGHT()
        {
            if (sx == 0)
            {
                this.matrix[sx, sy] = 0;
                sx = 1;
                this.matrix[sx, sy] = 1;
            }
        }

        public void LEFT()
        {
            if (sx == 1)
            {
                this.matrix[sx, sy] = 0;
                sx = 0;
                this.matrix[sx, sy] = 1;
            }
        }

        public void DOWN()
        {
            if (sy < Pairs - 1)
            {
                this.matrix[sx, sy] = 0;
                sy++;
                this.matrix[sx, sy] = 1;
            }
        }
    }
}
