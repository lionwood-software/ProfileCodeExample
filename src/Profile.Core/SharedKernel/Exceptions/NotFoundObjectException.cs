using System;

namespace Profile.Core.SharedKernel.Exceptions
{
    public class NotFoundObjectException : Exception
    {
        public NotFoundObjectException()
        {
        }

        public NotFoundObjectException(string message)
            : base(message)
        {
        }

        public NotFoundObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
