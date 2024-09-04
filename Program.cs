using PerformanceAwareProgramming;

namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a file");
                Environment.Exit(0);       
            }

            var mov_register_to_register = 0b100010;
            var immediate_to_register = 0b1011;
            var immediate_to_register_memory = 0b1100011;
            var registers = new Registers().GetAddresses();

            using var file = File.Open(args[0], FileMode.Open);
            using var reader = new BinaryReader(file);
            Byte[] bytes = new byte[1];

            while(reader.Read(bytes) != 0)
            {
                if (bytes[0] >> 2 == mov_register_to_register)
                {
                    MoveRegisterToRegister(bytes, reader, registers);
                }

                if (bytes[0] >> 1 == immediate_to_register_memory)
                {
                   // Still need to cater for this 

                }

                if (bytes[0] >> 4 == immediate_to_register)
                {
                    MoveImmediateToRegister(bytes, reader, registers);
                }
            }
        }



        public static void MoveImmediateToRegister(Byte[] inputBytes, BinaryReader reader, List<Dictionary<int,string>> registers)
        {
            var w_bit = inputBytes[0] >> 3 & 1;
            var reg = inputBytes[0] & 7;
            int data;
            var register = registers[1];
            string source;
            var mod = inputBytes[0] >> 7 & 3;

            if (w_bit == 1)
            {
                Byte[] twelve_bits = new Byte[2];
                
                reader.Read(twelve_bits);

                if (BitConverter.IsLittleEndian){
                    Array.Reverse(twelve_bits);
                    data = 256 * twelve_bits[0] + twelve_bits[1];
                }
                else{
                    data = 256 * twelve_bits[1] + twelve_bits[0];
                }
            }
            else{
                register = registers[0];
                reader.Read(inputBytes);
                data = inputBytes[0];
            }

            register.TryGetValue(reg, out source);
            Console.WriteLine($"mov {source}, {data}");
        }

        static void MoveRegisterToRegister(Byte[] inputBytes, BinaryReader reader, List<Dictionary<int,string>> registers)
        {
            var d_bit = (inputBytes[0] >> 1) & 1;
            var w_bit = inputBytes[0] & 1;   

            reader.Read(inputBytes);

            var reg_field = (inputBytes[0] >> 3) & 7;
            var rm_field = inputBytes[0] & 7;
            var mod = inputBytes[0] >> 6;
            var register = registers[0];
            string destination = "";
            string source = "";
            
            if (w_bit == 1)
                register = registers[1];

            switch(mod)
            {
                case 3:
                    Console.WriteLine("; Register-to-register");
                    (destination, source) = RegisterModeSourceAndDest(d_bit, reg_field, rm_field, register);
                    break;
                case 0:
                    Console.WriteLine("; Source address calculation");
                    register.TryGetValue(reg_field, out destination);
                    registers[2].TryGetValue(rm_field, out source);
                    break;
                case 1:
                    Console.WriteLine("; Source address calculation plus 8-bit displacement");
                break;
                case 2:
                    Console.WriteLine("; Source address calculation plus 16-bit displacement");
                break;
            }

            Console.WriteLine($"mov {destination}, {source}");
        }

        static (string destination, string source) RegisterModeSourceAndDest(int d_bit, int reg_field, int rm_field, Dictionary<int, string> register)
        {
            string destination;
            string source;
            if (d_bit == 1)
            {
                register.TryGetValue(reg_field, out destination);
                register.TryGetValue(rm_field, out source);
            }
            else
            {
                register.TryGetValue(reg_field, out source);
                register.TryGetValue(rm_field, out destination);
            }

            return (destination, source);
        }
    }
}