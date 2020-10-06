using JuntosCodeChallenge.Domain.Customer.DTO;
using System.Collections.Generic;

namespace JuntosCodeChallenge.Domain.Customer.Interfaces
{
    public interface ICustomerProcessing
    {
        List<CustomerDTO> GetCustomer(string url);
    }
}