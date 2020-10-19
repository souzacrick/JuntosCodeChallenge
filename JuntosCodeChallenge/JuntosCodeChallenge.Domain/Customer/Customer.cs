using JuntosCodeChallenge.Domain.Customer.CustomException;
using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Domain.Customer.Enum;
using JuntosCodeChallenge.Domain.Customer.VO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace JuntosCodeChallenge.Domain.Customer
{
    public class Customer
    {
        public Customer()
        {
            MobileNumbers = new List<string>();
            TelephoneNumbers = new List<string>();
        }
        
        private Dictionary<CustomerRegionEnum, string> regions;

        [JsonIgnore]
        public CustomerTypeEnum TypeEnum { get; private set; }
        public string Type { get => TypeEnum.ToString(); }
        public string Gender { get; private set; }
        public Name Name { get; private set; }
        public Location Location { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public DateTime Registered { get; private set; }
        public List<string> TelephoneNumbers { get; private set; }
        public List<string> MobileNumbers { get; private set; }
        public Picture Picture { get; private set; }
        public string Nationality { get; private set; }

        public void LoadCustomerIsValid(int customerOrigin, string url)
        {
            string error = string.Empty;

            if (string.IsNullOrEmpty(url))
                error = "É necessário enviar uma URL para obter os clientes.";

            if (!System.Enum.IsDefined(typeof(CustomerOriginEnum), customerOrigin))
                error = "É necessário informar um tipo existente.";

            if (!string.IsNullOrEmpty(error))
                throw new CustomerException(error);
        }

        public void InitializeRegions()
        {
            regions = new Dictionary<CustomerRegionEnum, string>();

            foreach (CustomerRegionEnum item in System.Enum.GetValues(typeof(CustomerRegionEnum)))
            {
                switch (item)
                {
                    case CustomerRegionEnum.Norte:
                        regions.Add(item, "amazonas, pará, acre, rondônia, roraima, amapá, tocantins");
                        break;
                    case CustomerRegionEnum.Nordeste:
                        regions.Add(item, "maranhão, piauí, ceará, rio grande do norte, paraíba, pernambuco, bahia, alagoas, sergipe");
                        break;
                    case CustomerRegionEnum.CentroOeste:
                        regions.Add(item, "mato grosso, mato grosso do sul, goiás, distrito federal");
                        break;
                    case CustomerRegionEnum.Sudeste:
                        regions.Add(item, "rio de janeiro, são paulo, minas gerais, espírito santo");
                        break;
                    case CustomerRegionEnum.Sul:
                        regions.Add(item, "paraná, santa catarina, rio grande do sul");
                        break;
                    default:
                        break;
                }
            }
        }

        public Customer Build(CustomerDTO customerDTO, object customerTypeCoordinates, string defaultNationality)
        {
            Customer customer = new Customer();
            var customerProperties = customer.GetType().GetProperties();
            var customerDTOProperties = customerDTO.GetType().GetProperties();

            foreach (var customerDTOProperty in customerDTOProperties)
            {
                if (customerDTOProperty.Name.Equals("cell") || customerDTOProperty.Name.Equals("phone"))
                {
                    string n = $"+55{new string(customerDTOProperty.GetValue(customerDTO).ToString().Where(char.IsDigit).ToArray())}";

                    if (customerDTOProperty.Name.Equals("cell"))
                        customer.MobileNumbers.Add(n);
                    else
                        customer.TelephoneNumbers.Add(n);
                }
                else if (customerDTOProperty.Name.Equals("gender"))
                    customer.Gender = customerDTOProperty.GetValue(customerDTO).ToString().Equals("female") ? "F" : "M";
                else if (customerDTOProperty.Name.Equals("dob"))
                    customer.Birthday = ((CustomerDTO.Dob)customerDTOProperty.GetValue(customerDTO)).date;
                else if (customerDTOProperty.Name.Equals("registered"))
                    customer.Registered = ((CustomerDTO.Registered)customerDTOProperty.GetValue(customerDTO)).date;
                else
                {
                    foreach (var customerProperty in customerProperties)
                    {
                        if (customerDTOProperty.Name.ToLower() == customerProperty.Name.ToLower() && customerDTOProperty.PropertyType == customerProperty.PropertyType)
                        {
                            customerProperty.SetValue(customer, customerDTOProperty.GetValue(customerDTO));

                            if (customerDTOProperty.Name.Equals("location"))
                            {
                                var l = (Location)customerDTOProperty.GetValue(customerDTO);
                                customer.Location.RegionEnum = GetCustomerRegionByState(l.State);
                                customer.TypeEnum = GetCustomerType(Convert.ToDouble(l.Coordinates.Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(l.Coordinates.Longitude, CultureInfo.InvariantCulture), (List<CustomerTypeCoordinates>)customerTypeCoordinates);
                            }

                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(customer.Nationality))
                customer.Nationality = defaultNationality;

            return customer;
        }

        public CustomerTypeEnum GetCustomerType(double lat, double lon, List<CustomerTypeCoordinates> coordinates)
        {
            foreach (var item in System.Enum.GetValues(typeof(CustomerTypeEnum)))
            {
                var t = coordinates.FirstOrDefault(x => x.Type.Equals(item));

                if (t != null)
                {
                    var minLon = t.Minlon.Split("|");
                    var maxlon = t.Maxlon.Split("|");
                    var minlat = t.Minlat.Split("|");
                    var maxlat = t.Maxlat.Split("|");

                    for (int i = 0; i < minLon.Length; i++)
                        if ((lat <= Convert.ToDouble(maxlat[i], CultureInfo.InvariantCulture) && lat >= Convert.ToDouble(minlat[i], CultureInfo.InvariantCulture)) &&
                                (lon <= Convert.ToDouble(minLon[i], CultureInfo.InvariantCulture) && lon >= Convert.ToDouble(maxlon[i], CultureInfo.InvariantCulture)))
                            return t.Type;
                }
            }

            return CustomerTypeEnum.Laborious;
        }

        private CustomerRegionEnum GetCustomerRegionByState(string state)
        {
            return regions.FirstOrDefault(x => x.Value.ToLower().Contains(state.ToLower())).Key;
        }
    }
}