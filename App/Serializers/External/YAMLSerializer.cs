using System.Text;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SerializationDataLib.Serializers.External
{
    public class YAMLSerializer : IExperimentSerializer
    {
        private ISerializer _yamlSerializer;
        private IDeserializer _yamlDeserializer;

        public YAMLSerializer()
        {
            _yamlSerializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        public string ExperimentExtension => ".yml";

        /// <summary>
        /// The stream remains open for general API consistency.
        /// </summary>
        /// <typeparam name="T">Type used for deserialization.</typeparam>
        /// <param name="dataSrc">FileStream for deserialized file, supposedly in Open mode.</param>
        /// <param name="target">Optional parameter for saving the result.</param>
        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
        {
            using StreamReader sr = new(stream: dataSrc, encoding: Encoding.UTF8, leaveOpen: true);
            outTarget = _yamlDeserializer.Deserialize<T>(sr);
        }

        /// <summary>
        /// The stream remains open for general API consistency.
        /// </summary>
        /// <typeparam name="T">Type used for serialization.</typeparam>
        /// <param name="dataSrc">FileStream for serialized file, supposedly in Create mode.</param>
        /// <param name="target">The object being serialized.</param>
        public void Serialize<T>(FileStream dataSrc, ref T target)
        {
            using StreamWriter sw = new(stream: dataSrc, encoding: Encoding.UTF8, leaveOpen: true);
            _yamlSerializer.Serialize(sw, target);
        }

        public override string ToString() => "Antoine-Aubry-YamlDotNet";
    }
}
