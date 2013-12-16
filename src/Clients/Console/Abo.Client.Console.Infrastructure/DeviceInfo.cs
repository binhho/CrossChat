using System.Linq;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Contracts;

namespace Abo.Client.Desktop.Infrastructure
{
    public class DeviceInfo : IDeviceInfo
    {
        public async Task InitAsync()
        {
            Huid = "0XAAAAAAAAAAAAAAA93424";
        }

        public string Huid { get; private set; }
        public string PushUri { get; private set; }
    }
}
