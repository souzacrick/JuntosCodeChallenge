using Newtonsoft.Json;
using System.Collections.Generic;

namespace JuntosCodeChallenge.Domain.Customer.DTO
{
    public class CustomerJSONDTO : CustomerDTO
    {
        [JsonProperty("results")]
        public List<CustomerDTO> customers { get; set; }
    }
}