using Google.Protobuf;

namespace SerializationDataLib.Serializers.External
{
    public class ProtobufSerializer : IExperimentSerializer
    {
        MessageParser _messageParser = null;

        public string ExperimentExtension => ".dat";

        /// <summary>
        /// Notice that the same parser is used for deserialization.
        /// </summary>
        /// <typeparam name="T">Type used for serialization.</typeparam>
        /// <param name="dataSrc">Source file stream, supposedly in the Open mode.</param>
        /// <param name="outTarget">Optional output parameter for deserialization result.</param>
        public void Deserialize<T>(FileStream dataSrc, ref T outTarget) 
            => outTarget = (T)_messageParser.ParseFrom(dataSrc);

        public void Serialize<T>(FileStream dataSrc, ref T target)
        {
            IMessage protoMessage = (IMessage)target;
            _messageParser = protoMessage.Descriptor.Parser;
            protoMessage.WriteTo(dataSrc);
        }

        public override string ToString() => "Google-protobuf";
    }
}
