using JuntosCodeChallenge.Domain.Customer.Enum;
using System.Text.Json.Serialization;

namespace JuntosCodeChallenge.Domain.Customer.VO
{
    public class Location
    {
        [JsonIgnore]
        public CustomerRegionEnum RegionEnum { get; set; }
        public string Region { get => RegionEnum.ToString(); }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Postcode { get; set; }
        public Coordinates Coordinates { get; set; }
        public Timezone Timezone { get; set; }
    }
}