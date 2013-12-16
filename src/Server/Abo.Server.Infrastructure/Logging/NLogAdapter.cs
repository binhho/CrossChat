using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abo.Utils.Logging;

namespace Abo.Server.Infrastructure.Logging
{
    public class NLogAdapter : ILogger
    {
        public void Exception(Exception exc)
        {
            throw new NotImplementedException();
        }

        public void Exception(Exception exc, string captionFormat, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warning(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
