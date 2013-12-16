using System;

namespace Abo.Server.Domain.Exceptions
{
    public class InvalidPlayerRegistrationDataException : Exception
    {
        public InvalidPlayerRegistrationDataException(string message) : base(message)
        {
        }
    }
}
