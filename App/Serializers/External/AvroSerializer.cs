using Avro.File;
using Avro.Generic;
using Avro.Specific;

namespace SerializationDataLib.Serializers.External
{
    public class AvroSerializer : IExperimentSerializer
    {
        public string ExperimentExtension => ".avro";

        public void Deserialize<T>(FileStream dataSrc, ref T outTarget)
        {
            using var dataFileReader = DataFileReader<T>.OpenReader(dataSrc, leaveOpen: true);
            outTarget = dataFileReader.Next();
        }

        public void Serialize<T>(FileStream dataSrc, ref T target)
        {
            ISpecificRecord serializedRecord = (ISpecificRecord)target;
            DatumWriter<T> writer = new SpecificDatumWriter<T>(serializedRecord.Schema);
            using var dataFileWriter = DataFileWriter<T>.OpenWriter(writer, dataSrc, leaveOpen: true);
            dataFileWriter.Append(target);
        }

        public override string ToString() => "Apache-Avro-SpecificRecord";
    }
}
