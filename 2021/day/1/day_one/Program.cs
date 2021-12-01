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
            Func<int[], int> get_increase = (array) =>
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

            var summed_list = as_int_array
                .Where((s, i) => i + 2 < as_int_array.Length)
                .Select((s, i) =>
                {
                    return s + as_int_array[i + 1] + as_int_array[i + 2];
                }).ToArray();


            Console.WriteLine($"Increase values for three measure: {get_increase(summed_list)}");
            Console.ReadKey();
        }
    }
}
