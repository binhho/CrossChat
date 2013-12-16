using System.Linq;
using Abo.Client.WP.Silverlight.Converters;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_PhotoSelectionViewModel
    {
        public int[] BuiltinPhotos
        {
            get { return Enumerable.Range(1, IdToPhotoUrlConverter.LastFaceIndex).Select(i => i * -1).ToArray(); }
        }
    }
}