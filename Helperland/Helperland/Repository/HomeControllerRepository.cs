using Helperland.Data;
using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public class HomeControllerRepository : IHomeControllerRepository
    {
        private readonly HelperlandContext _helperlandContext;

        #region Constructor
        public HomeControllerRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }
        #endregion Constructor

        #region User Table

        public List<User> GetUserByPostalCodeAndCustomerId(string postalCode, int customerId)
        {
            var users = from sp in _helperlandContext.Users
                        join favoriteAndBlocked in _helperlandContext.FavoriteAndBlockeds on sp.UserId equals favoriteAndBlocked.UserId into spList
                        from favoriteAndBlocked in spList.DefaultIfEmpty()
                        join cust in _helperlandContext.Users on favoriteAndBlocked.TargetUserId equals cust.UserId into CustList
                        from tempCust in CustList.DefaultIfEmpty()
                        where sp.ZipCode == postalCode 
                        && (favoriteAndBlocked.TargetUserId == customerId || favoriteAndBlocked.Equals(null))
                        && (favoriteAndBlocked.IsBlocked == false || favoriteAndBlocked.Equals(null))
                        select sp;
            
            return users.ToList();
        }

        #endregion User Table

        #region UserAddress Table

        public List<UserAddress> GetUserAddress(int userId, string postalCode)
        {
            List<UserAddress> userAddressList = _helperlandContext.UserAddresses.Where(x => x.UserId == userId && x.PostalCode == postalCode).ToList();
            return userAddressList;
        }

        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddresses.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress SelectUserAddressByPK(int addressId)
        {
            UserAddress userAddress = _helperlandContext.UserAddresses.Where(x => x.AddressId == addressId).FirstOrDefault();
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        #endregion UserAddress Table

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

        #region ServiceRequest Table

        public ServiceRequest AddServiceRequest(ServiceRequest serviceRequest)
        {
            _helperlandContext.ServiceRequests.Add(serviceRequest);
            _helperlandContext.SaveChanges();
            return serviceRequest;
        }

        #endregion ServiceRequest Table

        #region ServiceRequestAddress Table

        public ServiceRequestAddress AddServiceRequestAddress(ServiceRequestAddress serviceRequestAddress)
        {
            _helperlandContext.ServiceRequestAddresses.Add(serviceRequestAddress);
            _helperlandContext.SaveChanges();
            return serviceRequestAddress;
        }

        #endregion ServiceRequestAddress Table

        #region ServiceRequestExtra Table

        public ServiceRequestExtra AddServiceRequestExtra(ServiceRequestExtra serviceRequestExtra)
        {
            _helperlandContext.ServiceRequestExtras.Add(serviceRequestExtra);
            _helperlandContext.SaveChanges();
            return serviceRequestExtra;
        }

        #endregion ServiceRequestExtra Table

        #region
        #endregion

        #region
        #endregion

        #region Contact Us Table

        public ContactU AddContactUs(ContactU contactU)
        {
            _helperlandContext.ContactUs.Add(contactU);
            _helperlandContext.SaveChanges();
            return contactU;
        }

        #endregion Contact Us Table

    }
}
