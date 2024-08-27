using System;
using System.Runtime.Serialization;

namespace AspShowcase.Application.Services
{
    [Serializable]
    public class ServiceException : Exception
    {
        public bool NotFound { get; set; }
        public ServiceException()
        {
        }

        public ServiceException(string? message) : base(message)
        {
        }

        public ServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}