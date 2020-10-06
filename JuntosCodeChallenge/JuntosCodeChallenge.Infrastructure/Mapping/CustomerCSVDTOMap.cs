using CsvHelper.Configuration;
using JuntosCodeChallenge.Domain.Customer.DTO;

namespace JuntosCodeChallenge.Infrastructure.Mapping
{
    public sealed class CustomerCSVDTOMap : ClassMap<CustomerDTO>
    {
        public CustomerCSVDTOMap()
        {
            Map(m => m.gender).Name("gender");
            Map(m => m.name.Title).Name("name__title");
            Map(m => m.name.First).Name("name__first");
            Map(m => m.name.Last).Name("name__last");
            Map(m => m.location.Street).Name("location__street");
            Map(m => m.location.City).Name("location__city");
            Map(m => m.location.State).Name("location__state");
            Map(m => m.location.Postcode).Name("location__postcode");
            Map(m => m.location.Coordinates.Latitude).Name("location__coordinates__latitude");
            Map(m => m.location.Coordinates.Longitude).Name("location__coordinates__longitude");
            Map(m => m.location.Timezone.Offset).Name("location__timezone__offset");
            Map(m => m.location.Timezone.Description).Name("location__timezone__description");
            Map(m => m.email).Name("email");
            Map(m => m.dob.date).Name("dob__date");
            Map(m => m.dob.age).Name("dob__age");
            Map(m => m.registered.date).Name("registered__date");
            Map(m => m.registered.age).Name("registered__age");
            Map(m => m.phone).Name("phone");
            Map(m => m.cell).Name("cell");
            Map(m => m.picture.Large).Name("picture__large");
            Map(m => m.picture.Medium).Name("picture__medium");
            Map(m => m.picture.Thumbnail).Name("picture__thumbnail");
        }
    }
}