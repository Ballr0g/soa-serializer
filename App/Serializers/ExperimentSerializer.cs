using SerializationDataLib.Serializers.External;
using SerializationDataLib.Timers;
using Google.Protobuf;
using Avro.Specific;

namespace SerializationDataLib.Serializers
{
    public class ExperimentSerializer : IExperimentSerializer
    {
        private IList<IExperimentSerializer> _serializers;
        private ActionTimer _actionTimer;

        /// <summary>
        /// The global serializer writer benchmarks to a text file.
        /// </summary>
        public string ExperimentExtension => ".txt";

        public ExperimentSerializer(int iterationsCount)
        {
            _serializers = new List<IExperimentSerializer>();
            _actionTimer = new ActionTimer(iterationsCount);
        }

        public ExperimentSerializer() : this(1) { }

        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
        {
            using StreamWriter sw = new(dataSrc, leaveOpen: true);
            foreach (var deserializer in _serializers)
            {
                if (deserializer is not XMLSerializer && 
                    (outTarget is IMessage && deserializer is ProtobufSerializer ||
                    outTarget is ISpecificRecord && deserializer is AvroSerializer ||
                    outTarget is not IMessage && outTarget is not ISpecificRecord
                    && deserializer is not ProtobufSerializer && deserializer is not AvroSerializer))
                {
                    DeserializeHelper(ref outTarget, sw, deserializer);
                }
                // A very nasty solution for an XML issue :/
                if (deserializer is XMLSerializer)
                {
                    if (outTarget is Dictionary<string, int> d)
                    {
                        List<MapEntry> entries = new();
                        d = new Dictionary<string, int>();
                        DeserializeHelper(ref entries, sw, deserializer);
                        foreach (var pair in entries)
                        {
                            d.Add(pair.Key, pair.Value);
                        }
                    }
                    else if (outTarget is IList<int> || outTarget is IList<double>)
                    {
                        DeserializeHelper(ref outTarget, sw, deserializer);
                    }
                }
            }
        }

        private void DeserializeHelper<T>(ref T outTarget, StreamWriter sw, IExperimentSerializer deserializer)
        {
            using FileStream fs = new(deserializer.GetSerializedDataPath(outTarget.GetType().Name), FileMode.Open);
            TimeSpan duration = _actionTimer.MeasureTimeForAction(deserializer.Deserialize, fs, ref outTarget);
            sw.WriteLine($"{deserializer} deserialization on {outTarget}: {duration}");
        }

        public void Serialize<T>(FileStream dataSrc, ref T target)
        {
            using StreamWriter sw = new(dataSrc, leaveOpen: true);
            foreach (var serializer in _serializers)
            {
                if (serializer is not XMLSerializer &&
                    (target is IMessage && serializer is ProtobufSerializer ||
                    target is ISpecificRecord && serializer is AvroSerializer ||
                    target is not IMessage && target is not ISpecificRecord 
                    && serializer is not ProtobufSerializer && serializer is not AvroSerializer))
                {
                    SerializeHelper(ref target, sw, serializer);
                }
                // A very nasty solution for an XML issue :/
                if (serializer is XMLSerializer)
                {
                    if (target is Dictionary<string, int> d)
                    {
                        List<MapEntry> entries = new(d.Count);
                        foreach (var pair in d)
                        {
                            entries.Add(new() { Key = pair.Key, Value = pair.Value });
                        }
                        SerializeHelper(ref entries, sw, serializer);
                    }
                    else if (target is IList<int> || target is IList<double>)
                    {
                        SerializeHelper(ref target, sw, serializer);
                    }
                }
            }
        }

        private void SerializeHelper<T>(ref T target, StreamWriter sw, IExperimentSerializer serializer)
        {
            using FileStream fs = new(serializer.GetSerializedDataPath(target.GetType().Name), FileMode.Create);
            TimeSpan duration = _actionTimer.MeasureTimeForAction(serializer.Serialize, fs, ref target);
            sw.WriteLine($"{serializer} serialization on {target}: {duration}");
        }

        public static ExperimentSerializer Parse(string line, int iterationsCount)
        {
            ExperimentSerializer serializer = new(iterationsCount);
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                IExperimentSerializer addedSerializer = MapSerializer(token);
                if (addedSerializer != null)
                {
                    serializer._serializers.Add(addedSerializer);
                }
            }

            return serializer;
        }

        private static IExperimentSerializer MapSerializer(string token) =>
            token switch
            {
                "0" => new BinarySerializer(),
                "1" => new XMLSerializer(),
                "2" => new JSONSerializer(),
                "3" => new AvroSerializer(),
                "4" => new MsgPackSerializer(),
                "5" => new ProtobufSerializer(),
                "6" => new YAMLSerializer(),
                _ => null
            };
    }
}
