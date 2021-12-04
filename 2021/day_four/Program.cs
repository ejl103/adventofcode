using System;
using System.Collections.Generic;
using System.Linq;

namespace day_four
{
    internal class Program
    {
        private const int dimension = 5;

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            // Line 0 is the bingo caller numbers
            var called = from entry in lines[0].Split(',') select int.Parse(entry);

            var all_cards = new List<int[,]>();

            // All cards are 5x5
            int[,]? current_card = null;
            int row_index = 0;


            // Get card data into memory
            for (int i = 1; i < lines.Length; i++)
            {
                var row = lines[i].Split(' ');
                // Single digit are padded
                if (row.Length >= dimension)
                {
                    if (current_card is null)
                    {
                        current_card = new int[dimension, dimension];
                        all_cards.Add(current_card);
                    }

                    for (int j = 0, card_index = 0; j < row.Length; j++, card_index++)
                    {
                        if (!int.TryParse(row[j], out current_card[row_index, card_index]))
                        {
                            --card_index;
                        }
                    }

                    row_index++;

                }
                else
                {
                    current_card = null;
                    row_index = 0;
                }
            }

            int[,]? winning_card = null;
            int? winning_num = null;

            int? last_winning_num = null;
            var cards_won = new List<int[,]>();

            // Workout the winner
            foreach (var num in called)
            {
                foreach (var card in all_cards)
                {
                    if (!cards_won.Contains(card))
                    {
                        for (int x = 0; x < card.GetLength(0); x++)
                        {
                            for (int y = 0; y < card.GetLength(1); y++)
                            {
                                
                                if (card[y, x] == num)
                                {
                                    card[y, x] = int.MaxValue;
                                    if (IsWinning(card))
                                    {
                                        winning_card ??= card;
                                        cards_won.Add(card);
                                        last_winning_num = num;
                                    }
                                }

                                
                                if (card[x, y] == num)
                                {
                                    card[x, y] = int.MaxValue;

                                    if (IsWinning(card))
                                    {
                                        winning_card ??= card;
                                        cards_won.Add(card);
                                        last_winning_num = num;
                                    }
                                }
                            }

                        }

                    }

                }

                if (winning_card is not null && winning_num is null)
                {
                    winning_num = num;
                    // to calcuate the last card we can't stop yet
                    // break;
                }

            }

            System.Diagnostics.Debug.Assert(winning_card != null);
            System.Diagnostics.Debug.Assert(winning_num != null);
            System.Diagnostics.Debug.Assert(cards_won.Count > 0);
            System.Diagnostics.Debug.Assert(last_winning_num != null);

            var answer = GetAnswer(winning_card, (int)winning_num);

            var answer2 = GetAnswer(cards_won.Last(), (int)last_winning_num);

            Console.WriteLine($"Part one answer: {answer}");
            Console.WriteLine($"Part two answer: {answer2}");
        }

        private static bool IsWinning(int[,] card)
        {
            for (int x = 0; x < card.GetLength(0); x++)
            {
                bool fail = false;
                for (int y = 0; y < card.GetLength(1); y++)
                {
                    if (card[x, y] != int.MaxValue)
                    {
                        fail = true;
                        break;
                    }    
                }

                if (!fail)
                    return true;
            }

            for (int x = 0; x < card.GetLength(0); x++)
            {
                bool fail = false;
                for (int y = 0; y < card.GetLength(1); y++)
                {
                    if (card[y, x] != int.MaxValue)
                    {
                        fail = true;
                        break;
                    }
                }

                if (!fail)
                    return true;
            }


            return false;
        }

        private static int GetAnswer(int[,] card, int winning_num)
        {
            int sum = 0;
            for (int x = 0; x < card.GetLength(0); x++)
            {
                for (int y = 0; y < card.GetLength(1); y++)
                {
                    if (card[x, y] != int.MaxValue)
                    {
                        sum += card[x, y];
                    }
                }
            }

            return sum * winning_num;
        }
    }
}
