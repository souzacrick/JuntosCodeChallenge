namespace JuntosCodeChallenge.Domain.Customer.DTO
{
    public class FilterCustomerDTO 
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Type { get; set; }
        public int? Region { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}