
namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mov_register_to_register = 0b100010;
            var immediate_to_register = 0b1011;
            var immediate_to_register_memory = 0b1100011;

            var lowRegisters = new Dictionary<int,string>()
            {
                {0b000,"al"},
                {0b001,"cl"},
                {0b010,"dl"},
                {0b011,"bl"},
                {0b100,"ah"},
                {0b101,"ch"},
                {0b110,"dh"},
                {0b111,"bh"},
            };

            var highRegisters = new Dictionary<int, string>(){
                {0b000,"ax"},
                {0b001,"cx"},
                {0b010,"dx"},
                {0b011,"bx"},
                {0b100,"sp"},
                {0b101,"bp"},
                {0b110,"si"},
                {0b111,"di"}
            };
            
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a file");
                Environment.Exit(0);       
            }

            using var file = File.Open(args[0], FileMode.Open);
            using var reader = new BinaryReader(file);
            Byte[] bytes = new byte[1];

            while(reader.Read(bytes) != 0)
            {

                int w_bit;
                int d_bit;
                int reg;
                int data;
                var register = lowRegisters;
                string source;
                string destination;
                int reg_field;
                int rm_field;
                int mod;

                var registers = new List<Dictionary<int, string>>{
                   lowRegisters,
                   highRegisters  
                };

                if (bytes[0] >> 2 == mov_register_to_register)
                {
                    MoveRegisterToRegister(bytes, reader, registers);
                }

                if (bytes[0] >> 1 == immediate_to_register_memory)
                {
                    

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
                Console.WriteLine("8 bit");
                reader.Read(inputBytes);
                data = inputBytes[0];
            }

            register.TryGetValue(reg, out source);
            Console.WriteLine($"mov {source}, {data}");
        }

        public static void MoveRegisterToRegister(Byte[] inputBytes, BinaryReader reader, List<Dictionary<int,string>> registers)
        {
            var d_bit = (inputBytes[0] >> 1) & 1;
            var w_bit = inputBytes[0] & 1;   

            reader.Read(inputBytes);

            var reg_field = (inputBytes[0] >> 3) & 7;
            var rm_field = inputBytes[0] & 7;

            var register = registers[0];
            
            if (w_bit == 1)
                register = registers[1];

            var destination = String.Empty;
            var source = String.Empty;

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

            Console.WriteLine("; Register-to-register");
            Console.WriteLine($"mov {destination}, {source}");
        }
    }
}