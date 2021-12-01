using System;
using System.Collections.Generic;
using System.Linq;

namespace day_one
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            var as_int_array = lines.Select(l =>
            {
                if (int.TryParse(l, out int result))
                {
                    return result;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }).ToArray();

            // In C#10 can use var, but using 9 here to mirror Unity
            Func<IEnumerable<int>, int> get_increase = (array) =>
            {
                int increase_count = 0;
                int? previous_value = null;
                foreach (var val in array)
                {
                    if (previous_value is not null && val > previous_value)
                    {
                        ++increase_count;
                    }

                    previous_value = val;
                }

                return increase_count;
            };

            Console.WriteLine($"Increase values: {get_increase(as_int_array)}");

            var chunks = as_int_array
            .Select((s, i) =>
            {
                if (i + 2 < as_int_array.Length)
                {
                    return new int[] { s, as_int_array[i + 1], as_int_array[i + 2], };
                }
                return null;
            }).ToArray();

            var summed_list = new List<int>(chunks.Length);
            foreach (var triplet in chunks)
            {
                if (triplet is not null)
                    summed_list.Add( triplet.Sum() );
            }

            Console.WriteLine($"Increase values for three measure: {get_increase(summed_list)}");
            Console.ReadKey();
        }
    }
}
