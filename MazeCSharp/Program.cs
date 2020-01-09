using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            String line, NewCord, RPAMazeLocation = @"C:\Users\rvjaz\source\repos\MazeCSharp\MazeCSharp\bin\Debug\RPAMaze.txt";
            Boolean skipfirst = true, CameFromLeft = false, CameFromRight = false, CameFromTop = false, CameFromBottom = false, StartModified = false;
            int row = 1, column, StartX = 0, StartY = 0, MazeHeight = 0, MazeWidth = 0;
            StreamReader readfile = new StreamReader(RPAMazeLocation);

            while ((line = readfile.ReadLine()) != null)
            {
                if (skipfirst)
                {
                    skipfirst = false;
                    MazeWidth = Convert.ToInt32(line.Split(' ')[0]);
                    MazeHeight = Convert.ToInt32(line.Split(' ')[1]);
                    continue;
                }

                Console.WriteLine(line);
                column = 1;
                foreach (String elem in line.Split(new string[] { " " }, StringSplitOptions.None))
                {
                    if (elem == "2")
                    {
                        StartX = row;
                        StartY = column;
                        break;
                    }
                    column++;
                }
                row++;
            }
            readfile.Close();
            Console.WriteLine("");
            Console.WriteLine("Keep original(1) or change(0) starting position?");
            if (Console.ReadLine() == "0")
            {
                Console.WriteLine("Enter new coordinates. For example: 10,2(Current [" + StartX + "," + StartY + "])");
                NewCord = Console.ReadLine();
                StartX = Convert.ToInt32(NewCord.Split(',')[0]);
                StartY = Convert.ToInt32(NewCord.Split(',')[1]);
                StartModified = true;
            }

            if (StartX < 1 || StartX > MazeWidth || StartY < 0 || StartY > MazeHeight)
            {
                Console.WriteLine("X coordinate can not be lower than [1] or higher than [" + MazeWidth + "], while Y coordinate can not be lower than [0] or higher than [" + MazeHeight + "]!" +
                    " Further execution will not be proceeded!");
                Console.ReadLine();
                return;
            }

            if (StartY == 0 || StartY == MazeHeight)
            {
                StartY--;
            }

            if (StartX == 1 || StartX == MazeWidth)
            {
                StartX--;
            }

            using (StreamWriter writetext = new StreamWriter("Log.txt"))
            {
                writetext.WriteLine("(x,y coordinates)");
                writetext.WriteLine("START");
                writetext.WriteLine("[" + StartX + "," + StartY + "]");
            }

            row = StartX;
            column = StartY - 1;
            while (column != MazeWidth || row != MazeHeight || column != 0 || row != 1)
            {
                if ((File.ReadLines(RPAMazeLocation).Skip(row).Take(1).First().Split(' ')[column - 1] == "0" 
                    || File.ReadLines(RPAMazeLocation).Skip(row).Take(1).First().Split(' ')[column - 1] == "2" && StartModified) && CameFromLeft != true)
                {
                    column--;
                    CameFromBottom = false;
                    CameFromTop = false;
                    CameFromRight = true;
                    CameFromLeft = false;
                    Console.WriteLine("I went to [" + row + "," + (column + 1)+ "]");
                }
                else if ((File.ReadLines(RPAMazeLocation).Skip(row).Take(1).First().Split(' ')[column + 1] == "0" 
                    || File.ReadLines(RPAMazeLocation).Skip(row).Take(1).First().Split(' ')[column + 1] == "2" && StartModified) && CameFromRight != true)
                {
                    column++;
                    CameFromBottom = false;
                    CameFromTop = false;
                    CameFromRight = false;
                    CameFromLeft = true;
                    Console.WriteLine("I went to [" + row + "," + (column + 1) + "]");
                }
                else if ((File.ReadLines(RPAMazeLocation).Skip(row - 1).Take(1).First().Split(' ')[column] == "0" 
                    || File.ReadLines(RPAMazeLocation).Skip(row - 1).Take(1).First().Split(' ')[column] == "2" && StartModified) && CameFromBottom != true)
                {
                    row--;
                    CameFromBottom = false;
                    CameFromTop = true;
                    CameFromRight = false;
                    CameFromLeft = false;
                    Console.WriteLine("I went to [" + row + "," + (column + 1) + "]");
                }
                else if ((File.ReadLines(RPAMazeLocation).Skip(row + 1).Take(1).First().Split(' ')[column] == "0" 
                    || File.ReadLines(RPAMazeLocation).Skip(row + 1).Take(1).First().Split(' ')[column] == "2" && StartModified) && CameFromTop != true)
                {
                    row++;
                    CameFromBottom = true;
                    CameFromTop = false;
                    CameFromRight = false;
                    CameFromLeft = false;
                    Console.WriteLine("I went to [" + row + "," + (column + 1) + "]");
                }
                else
                {
                    Console.WriteLine("There is no possible solution");
                    break;
                }

                using (StreamWriter writetext = new StreamWriter("Log.txt", true))
                {
                    writetext.WriteLine("[" + row + "," + (column + 1) + "]");
                    if (column == MazeWidth || row == MazeHeight || column == 0 || row == 1)
                    {
                        writetext.WriteLine("EXIT");
                        writetext.Close();
                        break;
                    }
                }
            }
            Console.WriteLine("Footsteps were recorded into Log.txt which is stored in project's 'debug' folder");
            Console.ReadLine();
            
        }
    }
}
