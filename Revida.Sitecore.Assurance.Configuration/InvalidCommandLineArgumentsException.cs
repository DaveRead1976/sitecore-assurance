using System;
using System.Runtime.Serialization;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class InvalidCommandLineArgumentsException : Exception
    {
        public InvalidCommandLineArgumentsException()
        {
            
        }

        public InvalidCommandLineArgumentsException(string message) : base(message)
        {
            
        }

        public InvalidCommandLineArgumentsException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected InvalidCommandLineArgumentsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
