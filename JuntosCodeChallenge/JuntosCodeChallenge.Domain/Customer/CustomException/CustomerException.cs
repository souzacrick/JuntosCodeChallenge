using System;

namespace JuntosCodeChallenge.Domain.Customer.CustomException
{
    public class CustomerException : Exception
    {
        public CustomerException(string message) :
            base(message)
        { }
    }
}