using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Domain.Customer.Interfaces;
using JuntosCodeChallenge.Infrastructure.CrossCutting;
using System.Collections.Generic;

namespace JuntosCodeChallenge.API.Services
{
    public class CustomerJSONProcessing : ICustomerProcessing
    {
        public List<CustomerDTO> GetCustomer(string url)
        {
            return new APIHelper().GetStringAsync<CustomerJSONDTO>(url).Result.customers;
        }
    }
}