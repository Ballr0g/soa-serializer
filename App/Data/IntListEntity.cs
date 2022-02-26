using MessagePack;

namespace SerializationDataLib
{
    [Serializable, MessagePackObject]
    public class IntListEntity
    {
        [Key(0)]
        public List<int> data;

        public IntListEntity()
        {
            data = new List<int>();
        }

        public IntListEntity(int count)
        {
            data = new List<int>(count);
            for (int i = 0; i < count; ++i)
            {
                // Avoiding corner cases potentially breaking serializers.
                data.Add(DataUtils.Randomizer.Next(int.MinValue + 5, int.MaxValue - 4));
            }
        }

        public IntListEntity(IList<int> data) => this.data = new List<int>(data);
    }
}
