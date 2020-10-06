using JuntosCodeChallenge.Domain.Customer.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosCodeChallenge.Domain.Customer.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        ResponseCustomerDTO Filter(FilterCustomerDTO filters);
    }
}
