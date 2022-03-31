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
    public class CustomerControllerRepository : ICustomerControllerRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public CustomerControllerRepository(HelperlandContext helperlandContext)
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

        #region ServiceRequest Table

        public IEnumerable<ServiceRequest> GetCurrentServiceRequestByCustomerId(int customerId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Include(x => x.ServiceProvider).Where(x => x.UserId == customerId && x.Status != (int)ServiceRequestStatusEnum.Cancelled
            && x.Status != (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequests;
        }

        public ServiceRequest GetServiceRequest(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _helperlandContext.ServiceRequests.Where(x => x.ServiceRequestId == serviceRequestId).FirstOrDefault();
            serviceRequest.ServiceRequestExtras = _helperlandContext.ServiceRequestExtras.Where(x => x.ServiceRequestId == serviceRequestId).ToList();
            serviceRequest.ServiceRequestAddresses = _helperlandContext.ServiceRequestAddresses.Where(x => x.ServiceRequestId == serviceRequestId).ToList();
            serviceRequest.User = _helperlandContext.Users.Where(x => x.UserId == serviceRequest.UserId).FirstOrDefault();
            return serviceRequest;
        }

        public List<ServiceRequest> GetFutureServiceRequestByServiceProviderId(int serviceProviderId)
        {
            List<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == serviceProviderId && x.ServiceStartDate > DateTime.Now).ToList();
            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetServiceRequestHistoryListByCustomerId(int customerId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequests.Include(x => x.ServiceProvider).Where(x => x.UserId == customerId &&
                (x.Status == (int)ServiceRequestStatusEnum.Cancelled || x.Status == (int)ServiceRequestStatusEnum.Completed)).ToList();
            return serviceRequests;
        }

        public ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            _helperlandContext.ServiceRequests.Update(serviceRequest);
            _helperlandContext.SaveChanges();
            return serviceRequest;
        }

        #endregion ServiceRequest Table

        #region Rating Table

        public List<Rating> GetRatingsByServiceProviderId(int? serviceProviderId)
        {
            return _helperlandContext.Ratings.Where(x => x.RatingTo == serviceProviderId).ToList<Rating>();
        }

        public Rating GetRatingsByServiceRequestId(int? serviceRequestId)
        {
            return _helperlandContext.Ratings.Where(x => x.ServiceRequestId == serviceRequestId).FirstOrDefault();
        }

        public Rating AddRating(Rating rating)
        {
            _helperlandContext.Ratings.Add(rating);
            _helperlandContext.SaveChanges();
            return rating;
        }

        #endregion Rating Table

        #region User Table

        public User GetUserByPK(int userId)
        {
            return _helperlandContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public User UpdateUser(User user)
        {
            _helperlandContext.Users.Update(user);
            _helperlandContext.SaveChanges();
            return user;
        }

        #endregion User Table

        #region UserAddress Table

        public List<UserAddress> GetUserAddressByUserId(int userId)
        {
            return _helperlandContext.UserAddresses.Where(x => x.UserId == userId).ToList();
        }

        public UserAddress GetUserAddressByPK(int AddressId, int userId)
        {
            return _helperlandContext.UserAddresses.Where(x => x.AddressId == AddressId && x.UserId == userId).FirstOrDefault();
        }

        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddresses.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress UpdateUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddresses.Update(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress DeleteUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddresses.Remove(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        #endregion UserAddress Table

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
