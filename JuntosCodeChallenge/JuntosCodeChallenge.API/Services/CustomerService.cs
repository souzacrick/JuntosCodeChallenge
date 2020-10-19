using JuntosCodeChallenge.Domain.Customer;
using JuntosCodeChallenge.Domain.Customer.Enum;
using JuntosCodeChallenge.Domain.Customer.Interfaces;
using JuntosCodeChallenge.Domain.Customer.VO;
using JuntosCodeChallenge.Infrastructure.CrossCutting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace JuntosCodeChallenge.API.Services
{
    public class CustomerService : ICustomer
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public CustomerService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public void Initialize()
        {
            List<Customer> customers;
            LogHelper log = new LogHelper();
            Customer customer = new Customer();
            ICustomerProcessing customerProcessing = null;

            //Monta o objeto
            var customerTypeCoordinates =
                new List<CustomerTypeCoordinates>
                {
                    new CustomerTypeCoordinates {
                        Type = CustomerTypeEnum.Special,
                        Minlon = _configuration["Customer:CustomerTypeCoordinates:Special:Minlon"],
                        Minlat = _configuration["Customer:CustomerTypeCoordinates:Special:Minlat"],
                        Maxlat = _configuration["Customer:CustomerTypeCoordinates:Special:Maxlat"],
                        Maxlon = _configuration["Customer:CustomerTypeCoordinates:Special:Maxlon"]
                    }, new CustomerTypeCoordinates{
                        Type = CustomerTypeEnum.Normal,
                        Minlon = _configuration["Customer:CustomerTypeCoordinates:Normal:Minlon"],
                        Minlat = _configuration["Customer:CustomerTypeCoordinates:Normal:Minlat"],
                        Maxlat = _configuration["Customer:CustomerTypeCoordinates:Normal:Maxlat"],
                        Maxlon = _configuration["Customer:CustomerTypeCoordinates:Normal:Maxlon"]
                    }
                };

            //Iniciliza as regiões por estado
            log.Information("Inicializando as regiões.");
            customer.InitializeRegions();

            //Armazena os clientes no cache
            foreach (var item in Enum.GetValues(typeof(CustomerOriginEnum)))
            {
                customer.LoadCustomerIsValid(Convert.ToInt32(item), _configuration[$"AppSettings:{item}Url"]);

                switch ((CustomerOriginEnum)item)
                {
                    case CustomerOriginEnum.CSV:
                        log.Information("Carregando os clientes via CSV.");
                        customerProcessing = new CustomerCSVProcessing();
                        break;
                    case CustomerOriginEnum.JSON:
                        log.Information("Carregando os clientes via JSON.");
                        customerProcessing = new CustomerJSONProcessing();
                        break;
                    default:
                        break;
                }

                var customerDTO = customerProcessing.GetCustomer(_configuration[$"AppSettings:{item}Url"]);

                if (customerDTO != null)
                {
                    log.Information("Adicionando os clientes no cache.");
                    if (!_memoryCache.TryGetValue("Customers", out customers))
                        customers = new List<Customer>();

                    foreach (var c in customerDTO)
                        customers.Add(customer.Build(c, customerTypeCoordinates, _configuration["Customer:DefaultNationality"].ToString()));

                    _memoryCache.Set("Customers", customers);
                }
            }
        }
    }
}