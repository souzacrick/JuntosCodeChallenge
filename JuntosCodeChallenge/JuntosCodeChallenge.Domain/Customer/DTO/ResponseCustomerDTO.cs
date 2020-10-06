using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JuntosCodeChallenge.Domain.Customer.DTO
{
    public class ResponseCustomerDTO
    {
        public ResponseCustomerDTO()
        {
            users = new List<Customer>();
        }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }

        public List<Customer> users { get; set; }
    }
}