using System.IO;
using System.Xml.Serialization;
using Abo.Server.Infrastructure.Protocol;

namespace Abo.Server.Infrastructure.Serialization.Implementations
{
    public class XmlDtoSerializer : IDtoSerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(ms);
            }
        }
    }
}