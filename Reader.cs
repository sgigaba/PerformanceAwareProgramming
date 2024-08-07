
namespace Decoder
{
    public static class Reader
    {
        private static List<Byte> Read(string path)
        {
            var input = new List<byte>(); 
            var value = File.ReadAllBytes(path);

            for (int i = 0; i < value.Length; i++)
            {
                input.Add(value[i]);
            }

            return input;
        }
    }
}   