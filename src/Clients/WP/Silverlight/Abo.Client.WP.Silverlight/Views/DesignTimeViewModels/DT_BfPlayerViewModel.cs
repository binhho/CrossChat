namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_BfPlayerViewModel
    {
        public dynamic Player
        {
            get
            {
                return new
                {
                    Name = "Egor Bogatov",
                    Health = 43
                };
            }
        }

        public string OverlayText
        {
            get { return "+12"; }
        }
    }
}