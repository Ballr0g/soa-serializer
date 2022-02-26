using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationDataLib.Serializers
{
    public class BinarySerializer : IExperimentSerializer
    {
        private BinaryFormatter _binaryFormatter;

        public BinarySerializer() => _binaryFormatter = new BinaryFormatter();

        public string ExperimentExtension => ".bin";

        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
        {
#pragma warning disable SYSLIB0011
            outTarget = (T)_binaryFormatter.Deserialize(dataSrc);
#pragma warning restore SYSLIB0011
        }

        public void Serialize<T>(FileStream dataSrc, ref T target)
        {
#pragma warning disable SYSLIB0011
            // Suppressing the warning related to binary serialization being unsafe.
            // Reasoning: https://docs.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide
            _binaryFormatter.Serialize(dataSrc, target);
#pragma warning restore SYSLIB0011
        }

        public override string ToString() => ".NET-BinaryFormatter";
    }
}
