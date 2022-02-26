namespace SerializationDataLib.Serializers
{
    public interface IExperimentSerializer
    {
        public string ExperimentExtension { get; }
        public void Serialize<T>(FileStream dataSrc, ref T target);
        public void Deserialize<T>(FileStream dataSrc, ref T outTarget);
    }
}
