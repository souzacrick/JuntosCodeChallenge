using JuntosCodeChallenge.Domain.Customer.CustomException;
using System;

namespace JuntosCodeChallenge.Domain.Customer.DTO
{
    public class FilterCustomerDTO 
    {
        private const string validateError = "É necessário informar ao menos um campo para filtrar.";

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Type { get; set; }
        public int? Region { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        public void IsValid()
        {
            if (!PageSize.HasValue && !PageNumber.HasValue && !Type.HasValue && !Region.HasValue && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Gender))
                throw new ArgumentException(validateError);
        }
    }
}