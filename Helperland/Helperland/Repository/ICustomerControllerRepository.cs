using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public interface ICustomerControllerRepository
    {
        //City Table
        List<City> GetCitiesByPostalCode(string postalCode);

        //ServiceRequest Table
        IEnumerable<ServiceRequest> GetCurrentServiceRequestByCustomerId(int customerId);

        ServiceRequest GetServiceRequest(int serviceRequestId);

        List<ServiceRequest> GetFutureServiceRequestByServiceProviderId(int serviceProviderId);

        IEnumerable<ServiceRequest> GetServiceRequestHistoryListByCustomerId(int customerId);

        ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest);

        //Rating Table
        List<Rating> GetRatingsByServiceProviderId(int? serviceProviderId);

        Rating GetRatingsByServiceRequestId(int? serviceRequestId);

        Rating AddRating(Rating rating);

        //User Table

        User GetUserByPK(int userId);

        User UpdateUser(User user);

        //UserAddress Table
        List<UserAddress> GetUserAddressByUserId(int userId);

        UserAddress GetUserAddressByPK(int AddressId, int userId);

        UserAddress AddUserAddress(UserAddress userAddress);

        UserAddress UpdateUserAddress(UserAddress userAddress);

        UserAddress DeleteUserAddress(UserAddress userAddress);

        //State Table
        State GetStateByCityName(string cityName);
    }
}
