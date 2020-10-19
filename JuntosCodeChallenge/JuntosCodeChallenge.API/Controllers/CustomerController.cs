using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Domain.Customer.Interfaces;
using JuntosCodeChallenge.Infrastructure.CrossCutting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace JuntosCodeChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : _BaseController
    {
        private readonly LogHelper logHelper;
        private readonly IMemoryCache _memoryCache;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            logHelper = new LogHelper();
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public ActionResult<ResponseCustomerDTO> Get()
        {
            try
            {
                return Ok(new ResponseCustomerDTO { users = _customerRepository.GetAll() });
            }
            catch (Exception e)
            {
                logHelper.Error(e, "Ocorreu um erro ao tentar obter todos os clientes já carregados.");
                return VerifyException(e);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<ResponseCustomerDTO> Filter([FromBody] FilterCustomerDTO filterCustomerDTO)
        {
            try
            {
                //Verifica se o objeto de filtro veio vazio
                filterCustomerDTO.IsValid();

                //Filtra de acordo com o que foi enviado
                var customers = _customerRepository.Filter(filterCustomerDTO);

                return Ok(customers);
            }
            catch (SystemException e)
            {
                logHelper.Error(e, "Ocorreu um erro ao filtrar os clientes já.", filterCustomerDTO);
                return VerifyException(e);
            }
        }
    }
}