using MessagePack;

namespace SerializationDataLib
{
    [Serializable, MessagePackObject]
    public class DoubleListEntity
    {
        [Key(0)]
        public List<double> data;

        public DoubleListEntity()
        {
            data = new List<double>();
        }

        public DoubleListEntity(int count)
        {
            data = new List<double>(count);
            for (int i = 0; i < count; ++i)
            {
                // Avoiding corner cases potentially breaking serializers.
                data.Add(DataUtils.Randomizer.NextDouble());
            }
        }

        public DoubleListEntity(IList<double> data) => this.data = new List<double>(data);
    }
}
