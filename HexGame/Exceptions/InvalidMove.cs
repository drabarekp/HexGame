using System;

namespace HexGame.Exceptions
{
    internal class InvalidMoveException : Exception
    {
        public InvalidMoveException()
        {
        }

        public InvalidMoveException(string message)
            : base(message)
        {
        }

        public InvalidMoveException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
