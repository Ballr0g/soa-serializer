using System.Text;
using SerializationDataLib.Serializers;

namespace SerializationDataLib
{
    public delegate void RefAction<T>(ref T value);
    public delegate void RefAction<T1, T2>(T1 dataSource, ref T2 value);
    public static class DataUtils
    {
        public static readonly string DataDirPath = @"serialized";
        public static readonly char Sep = Path.DirectorySeparatorChar;
        public static readonly Random Randomizer = new();

        public static string NextString(this Random random, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("Random string length cannot be less than 0.");

            StringBuilder sb = new(length);
            for (int i = 0; i < length; ++i)
            {
                if (Randomizer.Next(2) == 0)
                    sb.Append((char)random.Next('a', 'z' + 1));
                else
                    sb.Append((char)random.Next('A', 'Z' + 1));
            }

            return sb.ToString();
        }

        public static string GetSerializedDataPath(this IExperimentSerializer serializer, string type)
            => $@"{DataDirPath}{Sep}{serializer}-{type}{serializer.ExperimentExtension}";
    }
}
