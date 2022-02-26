using System.Text.Json;

namespace SerializationDataLib.Serializers
{
    public class JSONSerializer : IExperimentSerializer
    {
        public string ExperimentExtension => ".json";

        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
            => outTarget = JsonSerializer.Deserialize<T>(dataSrc);

        public void Serialize<T>(FileStream dataSrc, ref T target)
            => JsonSerializer.Serialize(dataSrc, target);

        public override string ToString() => ".NET-JsonSerializer";
    }
}
