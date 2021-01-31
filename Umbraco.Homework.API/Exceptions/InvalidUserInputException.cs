using System;
using System.Collections.Generic;

namespace Umbraco.Homework.API.Exceptions
{
    public class InvalidUserInputException : Exception
    {
        public IEnumerable<String> Errors { get; private set; }

        public InvalidUserInputException()
        {
        }

        public InvalidUserInputException(String msg):base(msg)
        {
        }

        public InvalidUserInputException(String msg, IEnumerable<String> errors) : base(msg)
        {
            this.Errors = errors;
        }
    }
}
