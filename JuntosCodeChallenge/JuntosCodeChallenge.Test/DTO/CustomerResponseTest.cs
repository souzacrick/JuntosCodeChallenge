using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosCodeChallenge.Test
{
    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Coordinates
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class Timezone
    {
        public string offset { get; set; }
        public string description { get; set; }
    }

    public class Location
    {
        public string region { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int postcode { get; set; }
        public Coordinates coordinates { get; set; }
        public Timezone timezone { get; set; }
    }

    public class Picture
    {
        public string large { get; set; }
        public string medium { get; set; }
        public string thumbnail { get; set; }
    }

    public class User
    {
        public string type { get; set; }
        public string gender { get; set; }
        public Name name { get; set; }
        public Location location { get; set; }
        public string email { get; set; }
        public DateTime birthday { get; set; }
        public DateTime registered { get; set; }
        public List<string> telephoneNumbers { get; set; }
        public List<string> mobileNumbers { get; set; }
        public Picture picture { get; set; }
        public string nationality { get; set; }
    }

    public class CustomerResponseTest
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public List<User> users { get; set; }
    }


}
