using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Domain.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace JuntosCodeChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository, IMemoryCache memoryCache)
        {
            _customerRepository = customerRepository;
            _memoryCache = memoryCache;
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
                //Inclui o erro no cache
                _memoryCache.Set("Errors", new Dictionary<string, object>() { { "Erro ao obter todos os clientes", e } });
                return BadRequest();
            }
        }

        [HttpPost("[action]")]
        public ActionResult<ResponseCustomerDTO> Filter([FromBody] FilterCustomerDTO filterCustomerDTO)
        {
            try
            {
                //Verifica se o objeto de filtro veio vazio
                if (filterCustomerDTO == null)
                    return BadRequest();

                //Filtra de acordo com o que foi enviado
                var customers = _customerRepository.Filter(filterCustomerDTO);

                //Verifica se houve algum retorno
                if (customers == null)
                    return BadRequest();

                return Ok(customers);
            }
            catch (Exception e)
            {
                //Inclui o erro no cache
                _memoryCache.Set("Errors", new Dictionary<string, object>() { { "Erro ao filtrar os clientes", e } });
                return BadRequest();
            }
        }
    }
}