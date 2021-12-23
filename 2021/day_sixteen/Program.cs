using System;
using System.Linq;

namespace day_sixteen
{
    internal class Program
    {
        enum Type
        {
            SUM,
            PRODUCT,
            MIN,
            MAX,
            LITERAL,
            GREATER_THAN,
            LESS_THAN,
            EQUAL_TO
        }
        static uint GetValue(string as_binary, ref int bits_consumed, int bits_to_red)
        {
            var value = Convert.ToUInt32(as_binary.Substring(bits_consumed, bits_to_red), 2);
            bits_consumed += bits_to_red;

            return value;
        }

        static (uint version, Type type) ReadHeader(string as_binary, ref int bits_consumed)
        {
            // Top 3 version, so discard 1
            var version = GetValue(as_binary, ref bits_consumed, 3);

            Type type = (Type)GetValue(as_binary, ref bits_consumed, 3);

            return (version, type);
        }

        static ulong ReadLiteral(string as_binary, ref int bits_consumed)
        {
            string val = string.Empty;
            int check_val;
            do
            {
                check_val = bits_consumed;
                ++bits_consumed;
                val += as_binary.Substring(bits_consumed, 4);

                bits_consumed += 4;
            }
            while (as_binary[check_val] == '1');

            return Convert.ToUInt64(val, 2);
        }

        static ulong PerformOperation(Type type, ulong value, ulong? existing_value)
        {
            if (existing_value is null)
                return value;

            return type switch
            {
                Type.SUM => value + existing_value.Value,
                Type.PRODUCT => value * existing_value.Value,
                Type.MIN => Math.Min(value, existing_value.Value),
                Type.MAX => Math.Max(value, existing_value.Value),
                Type.LITERAL => throw new InvalidOperationException(),
                Type.GREATER_THAN => existing_value.Value > value ? 1u : 0,
                Type.LESS_THAN => existing_value.Value < value ? 1u : 0,
                Type.EQUAL_TO => existing_value.Value == value ? 1u : 0,
            };
        }

        static (uint version, ulong value) ProcessPacket(string as_binary, ref int bits_consumed)
        {
            var header = ReadHeader(as_binary, ref bits_consumed);
            ulong? existing_value = null;

            if (header.type == Type.LITERAL)
            {
                existing_value = ReadLiteral(as_binary, ref bits_consumed);
            }
            else // Operator packet
            {

                switch (GetValue(as_binary, ref bits_consumed, 1))
                {
                    case 0:
                        var length = GetValue(as_binary, ref bits_consumed, 15); 
                        var current_consumed = bits_consumed;

                        while (bits_consumed - current_consumed < length)
                        {
                            var (version, value) = ProcessPacket(as_binary, ref bits_consumed);
                            header.version += version;
                            existing_value = PerformOperation(header.type, value, existing_value);
                        }
                        break;
                    case 1:
                        var num_sub_packets = GetValue(as_binary, ref bits_consumed, 11);

                        for (int i = 0; i < num_sub_packets; i++)
                        {
                            var (version, value) = ProcessPacket(as_binary, ref bits_consumed);
                            header.version += version;
                            existing_value = PerformOperation(header.type, value, existing_value);
                        }
                        break;
                    default:
                        throw new Exception("Unexpected val");
                }
            }

            return (header.version, existing_value!.Value);
        }

        static void Main(string[] args)
        {
            // Just one line
            string line = System.IO.File.ReadAllLines(@"..\..\..\input.txt")[0];

            string as_binary = string.Join(string.Empty, line.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            var bits_consumed = 0;
            
            var (version_total, value) = ProcessPacket(as_binary, ref bits_consumed);


            Console.WriteLine($"Part one answer: {version_total}");
            Console.WriteLine($"Part two answer: {value}");
        }
    }
}
