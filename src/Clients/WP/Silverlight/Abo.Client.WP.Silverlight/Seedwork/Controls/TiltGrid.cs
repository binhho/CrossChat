using Abo.Client.WP.Silverlight.Seedwork.Extensions;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    /// <summary>
    /// Just a grid wich can be used as host for tilt-effects over it's content
    /// see TiltEffectExtension class
    /// </summary>
    public class TiltGrid : TapGrid
    {
        public TiltGrid()
        {
            TiltEffectExtension.SetIsTiltEnabled(this, true);
        }
    }
}
