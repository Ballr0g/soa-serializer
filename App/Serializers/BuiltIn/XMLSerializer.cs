using System.Xml.Serialization;

namespace SerializationDataLib.Serializers
{
    public class XMLSerializer : IExperimentSerializer
    {
        private XmlSerializer _xmlSerializer;
        public XMLSerializer() { }

        public string ExperimentExtension => ".xml";

        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
        {
            _xmlSerializer = new(typeof(T));
            outTarget = (T)_xmlSerializer.Deserialize(dataSrc);
        }

        public void Serialize<T>(FileStream dataSrc, ref T target)
        {
            _xmlSerializer = new(typeof(T));
            _xmlSerializer.Serialize(dataSrc, target);
        }

        public override string ToString() => ".NET-XmlSerializer";
    }
}