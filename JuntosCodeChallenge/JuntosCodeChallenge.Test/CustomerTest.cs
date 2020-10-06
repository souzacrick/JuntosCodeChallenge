using JuntosCodeChallenge.API;
using JuntosCodeChallenge.Domain.Customer;
using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Domain.Customer.Enum;
using JuntosCodeChallenge.Domain.Customer.VO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace JuntosCodeChallenge.Test
{
    public class CustomerTest
    {
        private readonly HttpClient _client;
        public CustomerTest()
        {
            var server = new TestServer(new WebHostBuilder().UseConfiguration(new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json").Build()).UseStartup<Startup>());

            _client = server.CreateClient();
        }

        [Fact]
        public void LoadCustomerInvalid()
        {
            Customer customer = new Customer();
            Assert.Throws<Exception>(() => customer.LoadCustomerIsValid(4, string.Empty));
            Assert.Throws<Exception>(() => customer.LoadCustomerIsValid(2, string.Empty));
        }
        [Fact]
        public void SpecialCustomer()
        {
            Customer customer = new Customer();
            var ev = CustomerTypeEnum.Special;

            var res = customer.GetCustomerType(Convert.ToDouble("-46.361899", CultureInfo.InvariantCulture), Convert.ToDouble("-2.196998", CultureInfo.InvariantCulture), new List<CustomerTypeCoordinates>
            {
                new CustomerTypeCoordinates
                {
                    Type = CustomerTypeEnum.Special,
                    Minlon = "-2.196998|-19.766959",
                    Minlat = "-46.361899|-52.997614",
                    Maxlat = "-34.276938|-44.428305",
                    Maxlon = "-15.411580|-23.966413"
                }
            });

            Assert.Equal(ev, res);
        }
        [Fact]
        public void NotSpecialCustomer()
        {
            Customer customer = new Customer();
            var ev = CustomerTypeEnum.Special;

            var res = customer.GetCustomerType(Convert.ToDouble("-64.123456", CultureInfo.InvariantCulture), Convert.ToDouble("8.196998", CultureInfo.InvariantCulture), new List<CustomerTypeCoordinates>
            {
                new CustomerTypeCoordinates
                {
                    Type = CustomerTypeEnum.Special,
                    Minlon = "14.196998|-19.766959",
                    Minlat = "-46.361899|-52.997614",
                    Maxlat = "-34.276938|-44.428305",
                    Maxlon = "7.411580|-23.966413"
                }
            });

            Assert.NotEqual(ev, res);
        }
        [Fact]
        public void LaboriousCustomer()
        {
            Customer customer = new Customer();
            var ev = CustomerTypeEnum.Laborious;

            var res = customer.GetCustomerType(Convert.ToDouble("-64.123456", CultureInfo.InvariantCulture), Convert.ToDouble("8.196998", CultureInfo.InvariantCulture), new List<CustomerTypeCoordinates>
            {
                new CustomerTypeCoordinates
                {
                    Type = CustomerTypeEnum.Special,
                    Minlon = "14.196998|-19.766959",
                    Minlat = "-46.361899|-52.997614",
                    Maxlat = "-34.276938|-44.428305",
                    Maxlon = "7.411580|-23.966413"
                }
            });

            Assert.Equal(ev, res);
        }
        [Fact]
        public void NormalCustomer()
        {
            Customer customer = new Customer();
            var ev = CustomerTypeEnum.Normal;

            var res = customer.GetCustomerType(Convert.ToDouble("-46.361899", CultureInfo.InvariantCulture), Convert.ToDouble("-27.196998", CultureInfo.InvariantCulture), new List<CustomerTypeCoordinates>
            {
                new CustomerTypeCoordinates
                {
                    Type = CustomerTypeEnum.Normal,
                    Minlon = "-26.155681",
                    Minlat = "-54.777426",
                    Maxlat = "-34.016466",
                    Maxlon = "-46.603598"
                }
            });

            Assert.Equal(ev, res);

        }
        [Fact]
        public void NotNormalCustomer()
        {
            Customer customer = new Customer();
            var ev = CustomerTypeEnum.Normal;

            var res = customer.GetCustomerType(Convert.ToDouble("10.361899", CultureInfo.InvariantCulture), Convert.ToDouble("3.196998", CultureInfo.InvariantCulture), new List<CustomerTypeCoordinates>
            {
                new CustomerTypeCoordinates
                {
                    Type = CustomerTypeEnum.Normal,
                    Minlon = "-26.155681",
                    Minlat = "-54.777426",
                    Maxlat = "-34.016466",
                    Maxlon = "-46.603598"
                }
            });

            Assert.NotEqual(ev, res);

        }
        [Fact]
        public void BuildCustomer()
        {
            Customer customer = new Customer();
            customer.InitializeRegions();
            customer = customer.Build(new CustomerDTO
            {
                cell = "(10) 4284 - 5756",
                gender = "female",
                phone = "(99) 2468-7865",
                email = "teste@teste.com",
                name = new Domain.Customer.VO.Name { Title = "Seu", First = "Zé", Last = "Silva" },
                dob = new CustomerDTO.Dob { age = 45, date = DateTime.Now },
                location = new Domain.Customer.VO.Location
                {
                    City = "Cidade",
                    Postcode = 123456,
                    State = "Paraná",
                    Street = "ali",
                    Coordinates = new Domain.Customer.VO.Coordinates { Latitude = "30.361899", Longitude = "-3.196998" },
                    Timezone = new Domain.Customer.VO.Timezone { Description = "teste", Offset = "offset" }
                },
                picture = new Domain.Customer.VO.Picture { Large = "large", Medium = "medium", Thumbnail = "thumb" },
                registered = new CustomerDTO.Registered { age = 20, date = DateTime.Now }
            }, new List<CustomerTypeCoordinates>
            {
                new CustomerTypeCoordinates
                {
                    Type = CustomerTypeEnum.Normal,
                    Minlon = "-26.155681",
                    Minlat = "-54.777426",
                    Maxlat = "-34.016466",
                    Maxlon = "-46.603598"
                }
            },
            "CA");

            Assert.Equal("teste@teste.com", customer.Email);
            Assert.Equal("F", customer.Gender);
            Assert.NotEqual("female", customer.Gender);
            Assert.Equal("+551042845756", customer.MobileNumbers.First());
            Assert.NotEqual("(10) 4284 - 5756", customer.Gender);
            Assert.Equal("+559924687865", customer.TelephoneNumbers.First());
            Assert.NotEqual("(99) 2468-7865", customer.Gender);
            Assert.Equal(CustomerRegionEnum.Sul, customer.Location.RegionEnum);
            Assert.Equal("Sul", customer.Location.RegionEnum.ToString());
            Assert.Equal("CA", customer.Nationality);

        }
        [Fact]
        public void GetNorthCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Region = 1 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerRegionEnum.Norte.ToString(), responseCustomer.users.FirstOrDefault().location.region);
            Assert.NotEqual(CustomerRegionEnum.Sul.ToString(), responseCustomer.users.LastOrDefault().location.region);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(10, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetNortheastCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Region = 2 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerRegionEnum.Nordeste.ToString(), responseCustomer.users.FirstOrDefault().location.region);
            Assert.NotEqual(CustomerRegionEnum.Sul.ToString(), responseCustomer.users.LastOrDefault().location.region);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(10, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetMidWestCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Region = 3 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerRegionEnum.CentroOeste.ToString(), responseCustomer.users.FirstOrDefault().location.region);
            Assert.NotEqual(CustomerRegionEnum.Sul.ToString(), responseCustomer.users.LastOrDefault().location.region);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(10, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetSoutheastCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Region = 4 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerRegionEnum.Sudeste.ToString(), responseCustomer.users.FirstOrDefault().location.region);
            Assert.NotEqual(CustomerRegionEnum.Sul.ToString(), responseCustomer.users.LastOrDefault().location.region);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(10, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetSouthCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Region = 5 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.NotEqual(CustomerRegionEnum.Norte.ToString(), responseCustomer.users.FirstOrDefault().location.region);
            Assert.Equal(CustomerRegionEnum.Sul.ToString(), responseCustomer.users.LastOrDefault().location.region);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(10, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetSpecialCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Type = 1, PageSize = 20 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerTypeEnum.Special.ToString(), responseCustomer.users.FirstOrDefault().type);
            Assert.NotEqual(CustomerTypeEnum.Normal.ToString(), responseCustomer.users.LastOrDefault().type);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(20, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetNormalCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Type = 2, PageSize = 5 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerTypeEnum.Normal.ToString(), responseCustomer.users.FirstOrDefault().type);
            Assert.NotEqual(CustomerTypeEnum.Laborious.ToString(), responseCustomer.users.LastOrDefault().type);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(5, responseCustomer.pageSize);
            Assert.Equal(0, responseCustomer.pageNumber);
        }
        [Fact]
        public void GetLaboriousCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Type = 3, PageSize = 20, PageNumber = 3 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.NotNull(responseCustomer);
            Assert.Equal(CustomerTypeEnum.Laborious.ToString(), responseCustomer.users.FirstOrDefault().type);
            Assert.NotEqual(CustomerTypeEnum.Special.ToString(), responseCustomer.users.LastOrDefault().type);
            Assert.True(responseCustomer.users.Any());
            Assert.Equal(20, responseCustomer.pageSize);
            Assert.Equal(3, responseCustomer.pageNumber);
            Assert.NotEqual(0, responseCustomer.pageNumber);
            Assert.Equal(20, responseCustomer.users.Count);
        }
        [Fact]
        public void GetLaboriousAndNorthCustomers()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Type = 3, Region = 1, PageSize = 25, PageNumber = 0 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal(CustomerTypeEnum.Laborious.ToString(), responseCustomer.users.FirstOrDefault().type);
            Assert.NotEqual(CustomerTypeEnum.Special.ToString(), responseCustomer.users.LastOrDefault().type);
            Assert.Equal(CustomerRegionEnum.Norte.ToString(), responseCustomer.users.FirstOrDefault().location.region);
            Assert.NotEqual(CustomerRegionEnum.Sul.ToString(), responseCustomer.users.LastOrDefault().location.region);
        }
        [Fact]
        public void FilterCustomersByEmail()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Email = "aes@exa", PageSize = 12, PageNumber = 1 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.True(responseCustomer.users.Exists(x => x.email.Contains("aes@exa")));
        }
        [Fact]
        public void FilterCustomersByGender()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Gender = "F", PageSize = 20, PageNumber = 0 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("F", responseCustomer.users.FirstOrDefault().gender);
            Assert.Equal("F", responseCustomer.users.LastOrDefault().gender);
            Assert.NotEqual("M", responseCustomer.users.LastOrDefault().type);
        }
        [Fact]
        public void BadRequestCustomers()
        {
            CustomerResponseTest responseCustomer = null;
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(string.Empty), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.NotEqual(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Null(responseCustomer);
        }
        [Fact]
        public void IncorrectPageNumber()
        {
            CustomerResponseTest responseCustomer = new CustomerResponseTest();
            HttpResponseMessage responseMessage = _client.PostAsync("/api/customer/Filter", new StringContent(JsonSerializer.Serialize(new FilterCustomerDTO { Type = 1, PageSize = 10, PageNumber = 3 }), Encoding.UTF8, "application/json")).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                string response = responseMessage.Content.ReadAsStringAsync().Result;
                responseCustomer = JsonSerializer.Deserialize<CustomerResponseTest>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            Assert.NotEqual(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }
    }
}