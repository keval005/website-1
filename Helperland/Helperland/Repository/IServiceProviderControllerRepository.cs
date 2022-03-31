using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public interface IServiceProviderControllerRepository
    {
        //City Table
        List<City> GetCitiesByPostalCode(string postalCode);

        //FavoriteAndBlocked Table
        FavoriteAndBlocked GetFavoriteAndBlockedByUserIdAndTargetUserId(int userId, int targetUserId);
        FavoriteAndBlocked AddFavoriteAndBlocked(FavoriteAndBlocked favoriteAndBlocked);
        FavoriteAndBlocked UpdateFavoriteAndBlocked(FavoriteAndBlocked favoriteAndBlocked);

        //Rating table
        IEnumerable<Rating> GetServiceProviderRatingByServiceProviderId(int serviceProviderId, decimal ratings);

        //ServiceRequest Table
        IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCode(string postalCode);
        IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCodeExcludePetAtHome(string postalCode);
        ServiceRequest GetServiceRequestByPK(int serviceRequestId);
        List<ServiceRequest> GetServiceRequestListByServiceProviderId(int serviceProviderId);
        ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest);
        IEnumerable<ServiceRequest> GetUpcomingServiceRequestsListByServiceProviderId(int serviceProviderId);
        IEnumerable<ServiceRequest> GetServiceRequestsHistoryListByServiceProviderId(int serviceProviderId);


        //ServiceRequestAddress Table
        List<ServiceRequestAddress> ServiceRequestAddressByServiceRequestId(int ServiceRequestId);

        //User Table
        User GetUserByPK(int userId);
        User UpdateUser(User user);
        List<User> GetUserByPostalCodeAndCustomerId(string postalCode, int customerId);

        //UserAddress Table
        UserAddress GetUserAddressByUserId(int userId);
        UserAddress AddUserAddress(UserAddress userAddress);
        UserAddress UpdateUserAddress(UserAddress userAddress);

        //State Table
        State GetStateByCityName(string cityName);
    }
}
