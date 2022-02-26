using SerializationDataLib;
using SerializationDataLib.Protobuf;
using SerializationDataLib.AvroMessage;
using SerializationDataLib.Serializers;

namespace SerializationExperiment
{
    class Program
    {
        static void Main()
        {
            string exit = string.Empty;
            while (exit != "exit")
            {
                int iterationsCount = ReadIterationsFromConsole();
                string serializersInfoLine = ReadSerializerDataFromConsole();
                int dataSize = ReadExperimentInputSizeFromConsole();
                RunExperimentWithParameters(serializersInfoLine, iterationsCount, dataSize);
                Console.WriteLine("Enter \"exit\" to leave...");
                exit = Console.ReadLine().ToLower();
            }
        }

        static int ReadIterationsFromConsole()
        {
            Console.WriteLine("Welcome to the serialization experiment app C# edition!");
            Console.WriteLine("First, make sure to enter the amount of iteration [1; 100]:");
            int iterations;
            bool correctInput;
            do
            {
                correctInput = int.TryParse(Console.ReadLine(), out iterations)
                    && iterations >= 1 && iterations <= 100;
                if (!correctInput)
                {
                    Console.WriteLine("Incorrect input! the amount of iterations must be an int [1; 100].");
                }
            } while (!correctInput);
            return iterations;
        }

        static string ReadSerializerDataFromConsole()
        {
            Console.WriteLine("\n\nNow enter a line using the following rules to specify");
            Console.WriteLine("the serializers you are going to use, separated by spaces (with no extra symbols!)");

            Console.WriteLine("Serializers:");
            Console.WriteLine("0 - built-in BinaryFormatter");
            Console.WriteLine("1 - built-in XmlSerializer");
            Console.WriteLine("2 - built-in JsonSerializer");
            Console.WriteLine("3 - Apache Avro Serializer");
            Console.WriteLine("4 - MessagePack Serializer");
            Console.WriteLine("5 - Google protocol buffers serializer");
            Console.WriteLine("6 - YamlDotNet YAML Serializer");
            Console.WriteLine("\nExample:\n1 5 6 (for xml, protobuf and yaml)");

            string input;
            bool inputCorrect;
            do
            {
                input = Console.ReadLine();
                inputCorrect = InputCorrect(input);
                if (!inputCorrect)
                {
                    Console.WriteLine("Error! The input format didn't match. Please enter a new string.");
                }
            } while (!inputCorrect);
            return input;
        }

        static int ReadExperimentInputSizeFromConsole()
        {
            Console.WriteLine("\n\nAs a final step, please enter the serialized data size [1; 2000]");
            int result;
            bool inputCorrect;
            do
            {
                inputCorrect = int.TryParse(Console.ReadLine(), out result)
                    && result >= 1 && result <= 2000;
                if (!inputCorrect)
                {
                    Console.WriteLine("Error! Incorrect input: please enter an int [1; 2000]");
                }
            } while (!inputCorrect);

            return result;
        }

        static void RunExperimentWithParameters(string request, int iterationsCount, int dataSize)
        {
            try
            {
                Directory.CreateDirectory(DataUtils.DataDirPath);
                ExperimentSerializer experimentSerializer = ExperimentSerializer.Parse(request, iterationsCount);

                using FileStream fs = new($@"{DataUtils.DataDirPath}{DataUtils.Sep}benchmark-results.txt", FileMode.Create);
                RunExperimentOnMaps(dataSize, experimentSerializer, fs);
                RunExperimentOnIntLists(dataSize, experimentSerializer, fs);
                RunExperimentOnDoubleLists(dataSize, experimentSerializer, fs);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Serialization experiment error: {ex.Message}");
                Console.Error.WriteLine("Stack trace: " + ex.StackTrace);
            }
        }

        private static void RunExperimentOnMaps(int dataSize, ExperimentSerializer experimentSerializer, FileStream fs)
        {
            MapEntity normalMapIn = new(dataSize);
            ProtoMap protoMapIn = new();
            FillMap(protoMapIn.Data, dataSize);
            AvroMap avroMapIn = new();
            avroMapIn.Data = new Dictionary<string, int>();
            FillMap(avroMapIn.Data, dataSize);

            experimentSerializer.Serialize(fs, ref normalMapIn.data);
            experimentSerializer.Serialize(fs, ref protoMapIn);
            experimentSerializer.Serialize(fs, ref avroMapIn);

            MapEntity normalMapOut = new();
            ProtoMap protoMapOut = new();
            AvroMap avroMapOut = new();

            experimentSerializer.Deserialize(fs, ref normalMapOut.data);
            experimentSerializer.Deserialize(fs, ref protoMapOut);
            experimentSerializer.Deserialize(fs, ref avroMapOut);
        }

        private static void RunExperimentOnIntLists(int dataSize, ExperimentSerializer experimentSerializer, FileStream fs)
        {
            IntListEntity normalListIn = new(dataSize);
            ProtoIntList protoIntListIn = new();
            FillIntList(protoIntListIn.IntList, dataSize);
            AvroIntList avroIntListIn = new();
            avroIntListIn.IntList = new List<int>();
            FillIntList(avroIntListIn.IntList, dataSize);

            experimentSerializer.Serialize(fs, ref normalListIn.data);
            experimentSerializer.Serialize(fs, ref protoIntListIn);
            experimentSerializer.Serialize(fs, ref avroIntListIn);

            IntListEntity normalListOut = new();
            ProtoIntList protoIntListOut = new();
            AvroIntList avroIntListOut = new();

            experimentSerializer.Deserialize(fs, ref normalListOut.data);
            experimentSerializer.Deserialize(fs, ref protoIntListOut);
            experimentSerializer.Deserialize(fs, ref avroIntListOut);
        }

        private static void RunExperimentOnDoubleLists(int dataSize, ExperimentSerializer experimentSerializer, FileStream fs)
        {
            DoubleListEntity normalRealListIn = new(dataSize);
            ProtoDoubleList protoRealListIn = new();
            FillRealList(protoRealListIn.RealList, dataSize);
            AvroDoubleList avroRealListIn = new();
            avroRealListIn.DoubleList = new List<double>();
            FillRealList(avroRealListIn.DoubleList, dataSize);

            experimentSerializer.Serialize(fs, ref normalRealListIn.data);
            experimentSerializer.Serialize(fs, ref protoRealListIn);
            experimentSerializer.Serialize(fs, ref avroRealListIn);

            DoubleListEntity normalRealListOut = new();
            ProtoDoubleList protoRealListOut = new();
            AvroDoubleList avroRealListOut = new();

            experimentSerializer.Deserialize(fs, ref normalRealListOut.data);
            experimentSerializer.Deserialize(fs, ref protoRealListOut);
            experimentSerializer.Deserialize(fs, ref avroRealListOut);
        }

        static bool InputCorrect(string line)
        {
            char[] allowedSymbols = { '0', '1', '2', '3', '4', '5', '6' };
            foreach (var symbol in line)
            {
                if (!(Array.IndexOf(allowedSymbols, symbol) != -1 || char.IsWhiteSpace(symbol)))
                {
                    return false;
                }
            }
            return true;
        }

        static void FillMap<T>(T map, int count) where T : IDictionary<string, int>
        {
            for (int i = 0; i < count; ++i)
            {
                string key = DataUtils.Randomizer.NextString(20);
                // Extra pre-caution to keep the key count precise.
                if (!map.ContainsKey(key))
                    map.Add(key, i);
                else
                    --i;
            }
        }

        static void FillIntList<T>(T list, int count) where T : IList<int>
        {
            for (int i = 0; i < count; ++i)
            {
                list.Add(DataUtils.Randomizer.Next(int.MinValue + 5, int.MaxValue - 4));
            }
        }

        static void FillRealList<T>(T list, int count) where T : IList<double>
        {
            for (int i = 0; i < count; ++i)
            {
                list.Add(DataUtils.Randomizer.NextDouble());
            }
        }
    }
}