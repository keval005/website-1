using Helperland.Data;
using Helperland.Enums;
using Helperland.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public class ServiceProviderControllerRepository : IServiceProviderControllerRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public ServiceProviderControllerRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        #region City Table

        public List<City> GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = (from city in _helperlandContext.Cities
                                 join zipcode in _helperlandContext.Zipcodes on city.Id equals zipcode.CityId
                                 where zipcode.ZipcodeValue == postalCode
                                 select new City
                                 {
                                     Id = city.Id,
                                     CityName = city.CityName
                                 }).ToList();
            return cities;
        }

        #endregion City Table

        #region FavoriteAndBlocked Table

        public FavoriteAndBlocked GetFavoriteAndBlockedByUserIdAndTargetUserId(int userId, int targetUserId)
        {
            return _helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == userId && x.TargetUserId == targetUserId).FirstOrDefault();
        }

        public FavoriteAndBlocked AddFavoriteAndBlocked(FavoriteAndBlocked favoriteAndBlocked)
        {
            _helperlandContext.Add(favoriteAndBlocked);
            _helperlandContext.SaveChanges();
            return favoriteAndBlocked;
        }

        public FavoriteAndBlocked UpdateFavoriteAndBlocked(FavoriteAndBlocked favoriteAndBlocked)
        {
            _helperlandContext.Update(favoriteAndBlocked);
            _helperlandContext.SaveChanges();
            return favoriteAndBlocked;
        }

        #endregion FavoriteAndBlocked Table

        #region Rating Table

        public IEnumerable<Rating> GetServiceProviderRatingByServiceProviderId(int serviceProviderId, decimal ratings)
        {
            return _helperlandContext.Ratings.Where(x => x.RatingTo == serviceProviderId && x.Ratings > (ratings == 5 ? 0 : ratings) && x.Ratings <= (ratings == 5 ? 5 : (ratings + 1))).Include(x => x.RatingFromNavigation).Include(x => x.ServiceRequest);
        }

        #endregion Rating Table

        #region ServiceRequest Table

        public IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCode(string postalCode)
        {
            //IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Where(x => x.ZipCode == postalCode
            //&& x.Status != (int)ServiceRequestStatusEnum.Accepted && x.Status != (int)ServiceRequestStatusEnum.Cancelled
            //&& x.Status != (int)ServiceRequestStatusEnum.Completed).ToList();
            IEnumerable<ServiceRequest> serviceRequests = from serviceRequest in _helperlandContext.ServiceRequests.Include(x => x.User)
                                                          join favoriteAndBlocked in _helperlandContext.FavoriteAndBlockeds
                                                          on serviceRequest.UserId equals favoriteAndBlocked.TargetUserId into blockedCustomer
                                                          from blocked in blockedCustomer.DefaultIfEmpty()
                                                          where serviceRequest.ZipCode == postalCode
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Pending
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Cancelled
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Completed
                                                            && blocked.IsBlocked != true
                                                          select serviceRequest;
            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCodeExcludePetAtHome(string postalCode)
        {
            //IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Where(x => x.ZipCode == postalCode
            //&& x.Status != (int)ServiceRequestStatusEnum.Accepted && x.Status != (int)ServiceRequestStatusEnum.Cancelled
            //&& x.Status != (int)ServiceRequestStatusEnum.Completed && x.HasPets == false).ToList();
            IEnumerable<ServiceRequest> serviceRequests = from serviceRequest in _helperlandContext.ServiceRequests.Include(x => x.User)
                                                          join favoriteAndBlocked in _helperlandContext.FavoriteAndBlockeds
                                                          on serviceRequest.UserId equals favoriteAndBlocked.TargetUserId into blockedCustomer
                                                          from blocked in blockedCustomer.DefaultIfEmpty()
                                                          where serviceRequest.ZipCode == postalCode
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Pending
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Cancelled
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Completed
                                                            && blocked.IsBlocked != true && serviceRequest.HasPets == false
                                                          select serviceRequest;
            return serviceRequests;
        }

        public ServiceRequest GetServiceRequestByPK(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == serviceRequestId).Include(c => c.User).Include(c => c.ServiceRequestAddresses).Include(c => c.ServiceRequestExtras).FirstOrDefault();
            //serviceRequest.ServiceRequestExtras = _helperlandContext.ServiceRequestExtras.Where(x => x.ServiceRequestId == serviceRequestId).ToList();
            //serviceRequest.ServiceRequestAddresses = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == serviceRequestId).ToList();
            //serviceRequest.User = _helperlandContext.Users.Where(x => x.UserId == serviceRequest.UserId).FirstOrDefault();
            return serviceRequest;
        }

        public List<ServiceRequest> GetServiceRequestListByServiceProviderId(int serviceProviderId)
        {
            List<ServiceRequest> serviceRequestList = _helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == serviceProviderId
            && x.Status == (int)ServiceRequestStatusEnum.Pending
            && x.Status != (int)ServiceRequestStatusEnum.Cancelled
            && x.Status != (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequestList;
        }

        public ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            _helperlandContext.ServiceRequests.Update(serviceRequest);
            _helperlandContext.SaveChanges();
            return serviceRequest;
        }

        public IEnumerable<ServiceRequest> GetUpcomingServiceRequestsListByServiceProviderId(int serviceProviderId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Include(x => x.User).Where(x => x.ServiceProviderId == serviceProviderId
            && x.Status == (int)ServiceRequestStatusEnum.Pending && x.Status != (int)ServiceRequestStatusEnum.Cancelled && x.Status != (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetServiceRequestsHistoryListByServiceProviderId(int serviceProviderId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Include(x => x.User).Where(x => x.ServiceProviderId == serviceProviderId
            && x.Status == (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequests;
        }

        #endregion ServiceRequest Table

        #region User Table

        public List<ServiceRequestAddress> ServiceRequestAddressByServiceRequestId(int ServiceRequestId)
        {
            return _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == ServiceRequestId).ToList();
        }

        #endregion User Table

        #region User Table

        public User GetUserByPK(int userId)
        {
            return _helperlandContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public List<User> GetUserByPostalCodeAndCustomerId(string postalCode, int customerId)
        {
            return _helperlandContext.Users.Where(x => x.ZipCode == postalCode).Include(x => x.FavoriteAndBlockedUsers.Where(c => c.TargetUserId == customerId)).ToList();
        }

        public User UpdateUser(User user)
        {
            _helperlandContext.Users.Update(user);
            _helperlandContext.SaveChanges();
            return user;
        }

        #endregion User Table

        #region UserAddress

        public UserAddress GetUserAddressByUserId(int userId)
        {
            return _helperlandContext.UserAddresses.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress UpdateUserAddress(UserAddress userAddress)
        {
            _helperlandContext.Update(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        #endregion UserAddress

        #region State Table

        public State GetStateByCityName(string cityName)
        {
            State objState = (from state in _helperlandContext.States
                              join city in _helperlandContext.Cities on state.Id equals city.StateId
                              where city.CityName == cityName
                              select new State
                              {
                                  Id = state.Id,
                                  StateName = state.StateName
                              }).FirstOrDefault();
            return objState;
        }

        #endregion State Table
    }
}
