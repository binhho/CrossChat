using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Contracts;

namespace Abo.Client.WindowsPhone.Infrastructure
{
    public class DeviceInfo : IDeviceInfo
    {
        public Task InitAsync()
        {
            return Task.FromResult<object>(null);
        }
        
        public string Huid { get { return "TODO:HUID"; }}
        public string PushUri { get { return string.Empty; }}
    }
}