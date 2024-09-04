namespace PerformanceAwareProgramming
{
    public class Registers
    {
        public Dictionary<int,string> lowRegisters = new Dictionary<int,string>()
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

        public Dictionary<int,string> highRegisters = new Dictionary<int, string>()
        {
            {0b000,"ax"},
            {0b001,"cx"},
            {0b010,"dx"},
            {0b011,"bx"},
            {0b100,"sp"},
            {0b101,"bp"},
            {0b110,"si"},
            {0b111,"di"}
        };

        public Dictionary<int,string> sourceAddressMode00 = new Dictionary<int, string>()
        {
            {0b000, "[bx + si]"},
            {0b001, "[bx + di]"},
            {0b010, "[bp + si]"},
            {0b011, "[bp + di]"},
            {0b100, "[si]"},
            {0b101, "[di]"},
            {0b111, "[bx]"},
        };
        
        public List<Dictionary<int,string>> GetAddresses()
        {
            return new List<Dictionary<int, string>>()
            {
                lowRegisters,
                highRegisters,
                sourceAddressMode00
            };
        }
    }
}