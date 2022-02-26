using MessagePack;

namespace SerializationDataLib
{
    [Serializable, MessagePackObject]
    public class MapEntity
    {
        [Key(0)]
        public Dictionary<string, int> data;

        public MapEntity()
        {
            data = new Dictionary<string, int>();
        }

        public MapEntity(int count)
        {
            data = new Dictionary<string, int>(count);
            for (int i = 0; i < count; ++i)
            {
                string key = DataUtils.Randomizer.NextString(20);
                // Extra pre-caution to keep the key count precise.
                if (!data.ContainsKey(key))
                    data.Add(key, i);
                else
                    --i;
            }
        }

        public MapEntity(Dictionary<string, int> data) => this.data = data;
    }

    /// <summary>
    /// The type used for XML serializer compaitability.
    /// </summary>
    [Serializable]
    public struct MapEntry
    {
        public string Key;
        public int Value;
    }
}