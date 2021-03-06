using System;

namespace day_two
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            int depth = 0, depth2 = 0;
            int horizontal = 0;
            int aim = 0;
            foreach (var line in lines)
            {
                var instruction_move = line.Split(' ');
                var increment = int.Parse(instruction_move[1]);

                switch ( instruction_move[0] ) 
                {
                    case "forward":
                        horizontal += increment;

                        depth2 += increment * aim;
                        break;
                    case "up":
                        depth -= increment;

                        aim -= increment;
                        break;
                    case "down":
                        depth += increment;

                        aim += increment;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            var part_one_answer = depth * horizontal;
            Console.WriteLine($"Part one answer {part_one_answer}");

            var part_two_answer = depth2 * horizontal;
            Console.WriteLine($"Part two answer {part_two_answer}");
        }
    }
}
