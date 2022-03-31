using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public interface IHomeControllerRepository
    {
        // User Table
        List<User> GetUserByPostalCodeAndCustomerId(string postalCode, int customerId);

        // UserAddress Table
        public List<UserAddress> GetUserAddress(int userId, string postalCode);

        public UserAddress AddUserAddress(UserAddress userAddress);

        public UserAddress SelectUserAddressByPK(int addressId);

        // City Table
        List<City> GetCitiesByPostalCode(string postalCode);


        // State Table
        State GetStateByCityName(string cityName);

        // ServiceRequest Table
        ServiceRequest AddServiceRequest(ServiceRequest serviceRequest);

        // ServiceRequestAddress Table
        ServiceRequestAddress AddServiceRequestAddress(ServiceRequestAddress serviceRequestAddress);

        // ServiceRequestExtra Table
        ServiceRequestExtra AddServiceRequestExtra(ServiceRequestExtra serviceRequestExtra);

        // Contact Us Table
        ContactU AddContactUs(ContactU contactU);


    }
}
