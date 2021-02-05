namespace Ecommerce.Api.Customers.Profiles
{
    public class CustomersProfile : AutoMapper.Profile
    {
        public CustomersProfile()
        {
            CreateMap<Db.Customer, Models.Customer>();
        }
    }
}
