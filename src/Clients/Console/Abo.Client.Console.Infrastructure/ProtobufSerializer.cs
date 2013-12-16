using System.IO;
using System.Linq;
using System.Reflection;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Infrastructure.Protocol;
using ProtoBuf.Meta;

namespace Abo.Client.Desktop.Infrastructure
{
    public class ProtobufSerializer : IDtoSerializer
    {
        private readonly RuntimeTypeModel _protobufModel;

        public ProtobufSerializer()
        {     
            _protobufModel = TypeModel.Create();
            var publicTypes = Assembly.GetAssembly(typeof (UserDto)).GetTypes().Where(i => i.IsPublic).ToList();
            foreach (var type in publicTypes)
            {
                var properties = type.GetProperties().Select(p => p.Name).OrderBy(name => name);
                var subClasses = publicTypes.Where(t => t.IsSubclassOf(type)).ToList();
                
                var meta = _protobufModel.Add(type, true).Add(properties.ToArray());
                for (int i = 0; i < subClasses.Count; i++)
                {
                    var subClass = subClasses[i];
                    meta.AddSubType(10 + i, subClass);
                }
            }
        }

        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                _protobufModel.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return (T)_protobufModel.Deserialize(ms, null, typeof(T));
            }
        }
    }
}
