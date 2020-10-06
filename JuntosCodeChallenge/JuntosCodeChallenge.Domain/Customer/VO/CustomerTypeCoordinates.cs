using JuntosCodeChallenge.Domain.Customer.Enum;

namespace JuntosCodeChallenge.Domain.Customer.VO
{
    public class CustomerTypeCoordinates
    {
        public CustomerTypeEnum Type { get; set; }
        public string Minlon { get; set; }
        public string Minlat { get; set; }
        public string Maxlon { get; set; }
        public string Maxlat { get; set; }
    }
}