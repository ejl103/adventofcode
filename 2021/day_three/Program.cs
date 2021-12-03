using System;
using System.Linq;

namespace day_three
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            // Assumung line length consistent so just get 0
            int column_count = lines[0].Length;
            var counts = new int[column_count];

            // Convert from binary
            int[] data = lines.Select(l => Convert.ToInt32(l, 2)).ToArray();

            foreach (var val in data)
            {
                for (int i = 0; i < column_count; i++)
                {
                    int test_bit = 1 << i;
                    if ((val & test_bit) == test_bit)
                    {
                        counts[i]++;
                    }
                }
            }

            var data_co2 = (int[])data.Clone();

            uint gamma_rate = 0;
            uint epsilon_rate = 0;
            var threshold = lines.Length / 2.0f;
            for (int i = 0; i < column_count; i++)
            {
                // What if its a tie?
                if (counts[i] > threshold)
                {
                    gamma_rate |= 1u << i;
                }
                else
                {
                    epsilon_rate |= 1u << i;
                }

                int backwards_i = column_count - i - 1;
                var test_bit = 1 << backwards_i;
                if (data.Length > 1)
                {
                    var count = 0;
                    foreach (var val in data)
                    {
                        if ((val & test_bit) == test_bit)
                        {
                            count++;
                        }
                    }

                    data = data.Where(val =>
                    {
                        if (count >= data.Length / 2.0f)
                        {
                            return (val & test_bit) == test_bit;
                        }
                        else
                        {
                            return (val & test_bit) != test_bit;
                        }
                    }).ToArray();
                }
                
                if (data_co2.Length > 1)
                {
                    var count = 0;
                    foreach (var val in data_co2)
                    {
                        if ((val & test_bit) == test_bit)
                        {
                            count++;
                        }
                    }

                    data_co2 = data_co2.Where(val =>
                    {
                        if (count < data_co2.Length / 2.0f)
                        {
                            return (val & test_bit) == test_bit;
                        }
                        else
                        {
                            return (val & test_bit) != test_bit;
                        }
                    }).ToArray();
                }

            }

            System.Diagnostics.Debug.Assert(data.Length == 1);
            System.Diagnostics.Debug.Assert(data_co2.Length == 1);

            Console.WriteLine($"Part 1 Gamme Rate: {gamma_rate}");
            Console.WriteLine($"Part 1 Epsilon Rate: {epsilon_rate}");
            Console.WriteLine($"Part 1 Answer: {gamma_rate * epsilon_rate}");

            Console.WriteLine($"Part 2 Answer: {data[0] * data_co2[0]}");
        }
    }
}
