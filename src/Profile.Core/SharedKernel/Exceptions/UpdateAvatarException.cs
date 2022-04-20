using System;

namespace Profile.Core.SharedKernel.Exceptions
{
    public class UpdateAvatarException : Exception
    {
        public UpdateAvatarException()
        {
        }

        public UpdateAvatarException(string message)
            : base(message)
        {
        }

        public UpdateAvatarException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
