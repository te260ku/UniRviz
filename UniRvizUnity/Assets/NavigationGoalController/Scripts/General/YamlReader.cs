using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public static class YamlReader
{
    public class DeserializedObject
    {
        public string image;
        public string mode;
        public float resolution;
        public float[] origin;
        public int negate;
        public float occupied_thresh;
        public float free_thresh;
    }

    public static DeserializedObject Deserialize(string yamlName)
    {
        StreamReader sr = new StreamReader(yamlName);
        string text = sr.ReadToEnd();
        var input = new StringReader(text);
        var deserializer = new DeserializerBuilder()
                            .WithNamingConvention(UnderscoredNamingConvention.Instance)
                            .Build();

        DeserializedObject deserializeObject = deserializer.Deserialize<DeserializedObject>(input);
        return deserializeObject;
    }
}
