using System.Linq;
using System.Reflection;

namespace Abo.Client.WP.Silverlight.Assets.Flags
{
    public partial class CountriesList
    {
        public static string[] GetCountries()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(i => i.StartsWith("Abo.Client.WP.Silverlight.Assets.Flags.") && i.EndsWith(".png"))
                .Select(System.IO.Path.GetFileNameWithoutExtension).ToArray();
        }

        public const string FileImagePathTemplate = "/Abo.Client.WP.Silverlight;component/Assets/Flags/{0}.png";
    }
}
