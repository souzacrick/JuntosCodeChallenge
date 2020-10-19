using JuntosCodeChallenge.Domain.Customer;
using JuntosCodeChallenge.Domain.Customer.CustomException;
using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Domain.Customer.Enum;
using JuntosCodeChallenge.Domain.Customer.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace JuntosCodeChallenge.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMemoryCache _memoryCache;
        private const string errorTotalPages = "O total de páginas é inferior a página solicitada.";

        public CustomerRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<Customer> GetAll()
        {
            return (List<Customer>)_memoryCache.Get("Customers");
        }

        public ResponseCustomerDTO Filter(FilterCustomerDTO filters)
        {
            int totalPages = 0, pageSize = 10, pageNumber = 0;

            var customers = (List<Customer>)_memoryCache.Get("Customers");

            if (filters.Type.HasValue)
                customers = customers.Where(x => x.TypeEnum == (CustomerTypeEnum)filters.Type.Value).ToList();

            if (filters.Region.HasValue)
                customers = customers.Where(x => x.Location.RegionEnum == (CustomerRegionEnum)filters.Region.Value).ToList();

            if (!string.IsNullOrEmpty(filters.Gender))
                customers = customers.Where(x => x.Gender.ToLower().Equals(filters.Gender.ToLower())).ToList();

            if (!string.IsNullOrEmpty(filters.Email))
                customers = customers.Where(x => x.Email.ToLower().Contains(filters.Email.ToLower())).ToList();

            if (filters.PageSize.HasValue)
                pageSize = filters.PageSize.Value;

            if (filters.PageNumber.HasValue)
                pageNumber = filters.PageNumber.Value;

            totalPages = customers.Count / pageSize;

            if (totalPages < pageNumber)
                throw new CustomerException(errorTotalPages);

            return new ResponseCustomerDTO { users = customers.Skip(pageNumber * pageSize).Take(pageSize).ToList(), pageNumber = pageNumber, pageSize = pageSize, totalCount = customers.Count() };
        }
    }
}