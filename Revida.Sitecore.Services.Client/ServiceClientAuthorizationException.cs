using System;
using System.Runtime.Serialization;

namespace Revida.Sitecore.Services.Client
{
    public class ServiceClientAuthorizationException : Exception
    {
        public ServiceClientAuthorizationException()
        {

        }

        public ServiceClientAuthorizationException(string message) : base(message)
        {

        }

        public ServiceClientAuthorizationException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected ServiceClientAuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
