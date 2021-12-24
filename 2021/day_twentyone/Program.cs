using System;

namespace day_twentyone
{
    internal class Program
    {
        const int MAX_SPACES = 10;
        const int WINNING_SCORE = 1000;

        struct Dice
        {
            public int Roll()
            {
                int score = 0;
                // roll 3 at a time
                for (int i = 0; i < 3; i++)
                {
                    score += state++;
                    rolls++;

                    if (state > 100)
                        state = 1;
                }
                return score;
            }
            public int rolls { get; private set; } = 0;
            int state = 1;
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            var pos = new int[lines.Length];
            var scores = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                pos[i] = int.Parse(lines[i].Split(':')[1]);
            }

            var die = new Dice();
            int? winner_idx = null;
            do 
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    var result = die.Roll();

                    pos[i] += result;
                    while (pos[i] > MAX_SPACES)
                    {
                        pos[i] -= MAX_SPACES;
                    }

                    scores[i] += pos[i];

                    if (scores[i] >= WINNING_SCORE)
                    {
                        winner_idx = i;
                        break;
                    }
                }

            }
            while (winner_idx == null);

            var loser_idx = 1 - winner_idx.Value;

            var part_1_answer = scores[loser_idx] * die.rolls;

            Console.WriteLine($"Part one answer: {part_1_answer}");
        }
    }
}
