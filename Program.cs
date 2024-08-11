
namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mov = 0b100010;
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
            Byte[] bytes = new byte[2];

            while (reader.Read(bytes) != 0)
            {
                var source = string.Empty;
                var destination = string.Empty;
                var register = lowRegisters;

                (int opcode, int d_bit, int w_bit, int mod, int reg_field, int rm_field) 
                    = ReadBytes(bytes[0], bytes[1]);
                
                if (opcode == mov)
                {
                    if (w_bit == 1)
                        register = highRegisters;

                    if (d_bit == 1)
                    {
                        register.TryGetValue(reg_field, out destination);
                        register.TryGetValue(rm_field, out source);
                    }
                    else{
                        register.TryGetValue(reg_field, out source);
                        register.TryGetValue(rm_field, out destination);
                    }
                }

                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
                {
                    Console.WriteLine("Invalid Decoding. Ensure you ented a correct file");
                    Environment.Exit(0);
                } 

                Console.WriteLine($"mov {destination}, {source}");
            }
        }

        static (int opcode, int d_bit, int w_bit, int mod, int reg_field, int rm_field) ReadBytes(byte firstByte, byte secondByte)
        {
            var op_code = firstByte >> 2;
            var d_bit = (firstByte >> 1) & 1;
            var w_bit = firstByte & 1;
            var mod = secondByte >> 6;
            var reg_field = (secondByte >> 3) & 0b_111;
            var rm_field = secondByte & 0b_111;

            if (op_code != 34){
                Console.WriteLine("Incorrect file format or instruction");
                Environment.Exit(0);
            }
            return (op_code, d_bit, w_bit, mod, reg_field, rm_field);
        } 
    }
}