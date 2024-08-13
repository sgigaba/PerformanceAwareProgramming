
namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mov_register_to_register = 0b100010;
            var immediate_to_register = 0b1011;
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
                int data16;
                var register = lowRegisters;
                string source;
                string destination;
                int reg_field;
                int rm_field;

                if (bytes[0] >> 2 == mov_register_to_register)
                {
                    d_bit = (bytes[0] >> 1) & 1;
                    w_bit = bytes[0] & 1;   

                    reader.Read(bytes);

                    reg_field = (bytes[0] >> 3) & 7;
                    rm_field = bytes[0] & 7;

                    if (w_bit == 1)
                        register = highRegisters;
                        
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
                    Console.WriteLine($"mov {destination}, {source}");
                }

                if (bytes[0] >> 4 == immediate_to_register)
                {
                    w_bit = bytes[0] >> 3 & 1;
                    reg = bytes[0] & 7;

                    reader.Read(bytes);
                    data = bytes[0];
                    
                    if (w_bit == 1)
                    {
                        register = highRegisters;
                        reader.Read(bytes);
                        data16 = bytes[0];
                    }

                    register.TryGetValue(reg, out source);
                    Console.WriteLine($"mov {source}, {data}");
                }
            }
        }
    }
}