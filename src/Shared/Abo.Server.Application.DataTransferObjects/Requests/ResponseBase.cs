namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            RequestResult = true;
        }

        //In order to associate Request with response
        public long Token { get; set; }

        //false means timeout or connection close
        public bool RequestResult { get; set; } 
    }
}