using MessagePack;

namespace SerializationDataLib
{
    [Serializable, MessagePackObject]
    public class MixedEntity
    {
        [Key(0)]
        public List<int> list;

        [Key(1)]
        public Dictionary<string, double> dict;

        [Key(2)]
        public string text;

        [Key(3)]
        public int integral;

        [Key(4)]
        public float real;

        public MixedEntity()
        {
            list = new List<int>();
            dict = new Dictionary<string, double>();
            text = DataUtils.Randomizer.NextString(20);
            integral = 42;
            real = 3.1415926f;
        }

        public MixedEntity(int count)
        {
            list = new List<int>(count);
            dict = new Dictionary<string, double>(count);
            for (int i = 0; i < count; ++i)
            {
                // Avoiding corner cases potentially breaking serializers.
                list.Add(DataUtils.Randomizer.Next(int.MinValue + 5, int.MaxValue - 4));
            }
        }

        public MixedEntity(IList<int> list, IDictionary<string, double> dict)
            => (this.list, this.dict) = (new List<int>(list), new Dictionary<string, double>(dict));
    }
}
