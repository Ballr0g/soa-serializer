using MessagePack;

namespace SerializationDataLib.Serializers.External
{
    public class MsgPackSerializer : IExperimentSerializer
    {
        public string ExperimentExtension => ".msgpack";

        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
            => outTarget = MessagePackSerializer.Deserialize<T>(dataSrc);

        public void Serialize<T>(FileStream dataSrc, ref T target)
            => MessagePackSerializer.Serialize(dataSrc, target);

        public override string ToString() => "neuecc-aarnott-MessagePack";
    }
}
