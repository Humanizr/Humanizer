using System;

namespace Humanizer
{
    /// <summary>
    /// This is thrown on String.DehumanizeTo enum when the provided string cannot be mapped to the target enum
    /// </summary>
    public class NoMatchFoundException : Exception
    {
        public NoMatchFoundException()
        {
        }

        public NoMatchFoundException(string message)
            : base(message)
        {
        }

        public NoMatchFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}