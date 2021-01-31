using System;
namespace Umbraco.Homework.API.Exceptions
{
    public class InvalidSerialNumberException : Exception
    {
        public InvalidSerialNumberException()
        {
        }

        public InvalidSerialNumberException(String msg):base(msg)
        {
        }
    }
}
