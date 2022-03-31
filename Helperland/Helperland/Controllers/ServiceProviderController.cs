using Helperland.Core;
using Helperland.Data;
using Helperland.Enums;
using Helperland.Models;
using Helperland.Repository;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    [CookieHelper]
    [SessionHelper(userType: UserTypeEnum.ServiceProvider)]
    public class ServiceProviderController : Controller
    {
        private readonly IServiceProviderControllerRepository _serviceProviderControllerRepository;
        private readonly IConfiguration _configuration;
        private readonly HelperlandContext _helperlandContext;

        public ServiceProviderController(IServiceProviderControllerRepository serviceProviderControllerRepository, IConfiguration configuration, HelperlandContext helperlandContext)
        {
            this._serviceProviderControllerRepository = serviceProviderControllerRepository;
            this._configuration = configuration;
            this._helperlandContext = helperlandContext;
        }

        public IActionResult NewServiceRequests()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetNewServiceRequestsList()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var includePetatHome = Request.Form["includePetatHome"].FirstOrDefault();

                User serviceProvider = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

                IEnumerable<ServiceRequest> serviceRequest;

                if (includePetatHome.ToString().Trim() == "true")
                {
                    serviceRequest = _serviceProviderControllerRepository.GetNewServiceRequestsListByPostalCode(serviceProvider.ZipCode);
                }
                else
                {
                    serviceRequest = _serviceProviderControllerRepository.GetNewServiceRequestsListByPostalCodeExcludePetAtHome(serviceProvider.ZipCode);
                }

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceDateTime_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceDateTime_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "CustomerName_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User.FirstName).ThenBy(s => s.User.LastName);  //check once for sorting
                        break;
                    case "CustomerName_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.User.FirstName).ThenBy(s => s.User.LastName);
                        break;
                    case "TotalCost_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.TotalCost);
                        break;
                    case "TotalCost_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.TotalCost);
                        break;
                    default:
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequest.Count();
                var data = serviceRequest.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    //temp.User = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(temp.UserId));
                    temp.ServiceRequestAddresses = _serviceProviderControllerRepository.ServiceRequestAddressByServiceRequestId(temp.ServiceRequestId);
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetServiceRequestWithCustomerDetails(string serviceRequestId)
        {
            ServiceRequest serviceRequest = _serviceProviderControllerRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId));

            serviceRequest.User = _serviceProviderControllerRepository.GetUserByPK(serviceRequest.UserId);

            String serviceRequestExtraName = "";

            foreach (ServiceRequestExtra serviceRequestExtra in serviceRequest.ServiceRequestExtras)
            {
                serviceRequestExtraName = serviceRequestExtraName + ", " + Enum.GetName(typeof(ExtraServiceEnum), serviceRequestExtra.ServiceExtraId);
            }

            if (serviceRequestExtraName.Length > 2)
            {
                serviceRequestExtraName = serviceRequestExtraName.Remove(0, 2);
            }
            else
            {
                serviceRequestExtraName = "-";
            }

            var jsonData = new { data = serviceRequest, extraServiceRequest = serviceRequestExtraName };
            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult AcceptServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _serviceProviderControllerRepository.GetServiceRequestByPK(Convert.ToInt32(model.ServiceRequestId));

            if (serviceRequest.RecordVersion.ToString() != model.RecordVersion)
            {
                return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "Error", ErrorMessage = "This service request is no more available. It has been assigned to another provider" });
            }

            DateTime newServiceRequestStartDateTime = serviceRequest.ServiceStartDate.AddMinutes(-60);
            DateTime newServiceRequestEndDateTime = serviceRequest.ServiceStartDate.AddMinutes((serviceRequest.ServiceHours * 60) + 60);

            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            List<ServiceRequest> serviceRequestList = _serviceProviderControllerRepository.GetServiceRequestListByServiceProviderId(Convert.ToInt32(sessionUser.UserID));
            Boolean serviceRequestConflict = false;
            string errorMessage = "";

            foreach (ServiceRequest temp in serviceRequestList)
            {
                DateTime serviceRequestStartDateTime = temp.ServiceStartDate;
                DateTime serviceRequestEndDateTime = serviceRequestStartDateTime.AddMinutes(temp.ServiceHours * 60);

                if (serviceRequestStartDateTime <= newServiceRequestEndDateTime && newServiceRequestStartDateTime <= serviceRequestEndDateTime)
                {
                    serviceRequestConflict = true;
                    errorMessage = "Another service request " + temp.ServiceRequestId + " has already been assigned which has time overlap with this service request. You can’t pick this one!";
                    break;
                }
            }

            if (serviceRequestConflict == true)
            {
                return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "Error", ErrorMessage = errorMessage });
            }

            serviceRequest.ServiceProviderId = Convert.ToInt32(sessionUser.UserID);
            serviceRequest.SpacceptedDate = DateTime.Now;
            serviceRequest.Status = (int)ServiceRequestStatusEnum.Pending;

            serviceRequest.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest.RecordVersion = Guid.NewGuid();

            _serviceProviderControllerRepository.UpdateServiceRequest(serviceRequest);

            List<User> userList = _serviceProviderControllerRepository.GetUserByPostalCodeAndCustomerId(serviceRequest.ZipCode, serviceRequest.UserId);

            foreach (User temp in userList)
            {
                if (temp.FavoriteAndBlockedUsers.Count > 0)
                {
                    if (temp.FavoriteAndBlockedUsers.ToArray()[0].IsBlocked)
                    {
                        break;
                    }
                }
                if (temp.UserId != serviceRequest.ServiceProviderId)
                {
                    MailHelper mailHelper = new MailHelper(_configuration);
                    EmailModel emailModel = new EmailModel();

                    emailModel.To = temp.Email;
                    emailModel.Subject = "Service Request no more available";
                    emailModel.Body = "Service request " + serviceRequest.ServiceRequestId + " is no more available. It has been assigned to another provider.";

                    mailHelper.SendMail(emailModel);
                }
            }

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        public IActionResult UpcomingService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUpcomingServiceRequestsList()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IEnumerable<ServiceRequest> serviceRequest = _serviceProviderControllerRepository.GetUpcomingServiceRequestsListByServiceProviderId(Convert.ToInt32(sessionUser.UserID));

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceDateTime_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceDateTime_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "CustomerName_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);  //check once for sorting
                        break;
                    case "CustomerName_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);
                        break;
                    case "TotalCost_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.TotalCost);
                        break;
                    case "TotalCost_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.TotalCost);
                        break;
                    default:
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequest.Count();
                var data = serviceRequest.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    //temp.User = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(temp.UserId));
                    temp.ServiceRequestAddresses = _serviceProviderControllerRepository.ServiceRequestAddressByServiceRequestId(temp.ServiceRequestId);
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult CompeleteServiceRequest(string serviceRequestId)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            ServiceRequest serviceRequest = _serviceProviderControllerRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId.ToString().Trim()));

            serviceRequest.Status = (int)ServiceRequestStatusEnum.Completed;

            serviceRequest.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _serviceProviderControllerRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        [HttpPost]
        public JsonResult CancelServiceRequest(string serviceRequestId)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            ServiceRequest serviceRequest = _serviceProviderControllerRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId.ToString().Trim()));

            serviceRequest.Status = (int)ServiceRequestStatusEnum.Cancelled;

            serviceRequest.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _serviceProviderControllerRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        public IActionResult ServiceHistory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetServiceRequestHistoryList()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IEnumerable<ServiceRequest> serviceRequest = _serviceProviderControllerRepository.GetServiceRequestsHistoryListByServiceProviderId(Convert.ToInt32(sessionUser.UserID));

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceDateTime_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceDateTime_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "CustomerName_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);  //check once for sorting
                        break;
                    case "CustomerName_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);
                        break;
                    default:
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequest.Count();
                var data = serviceRequest.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    //temp.User = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(temp.UserId));
                    temp.ServiceRequestAddresses = _serviceProviderControllerRepository.ServiceRequestAddressByServiceRequestId(temp.ServiceRequestId);
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult MyRatings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetCustomerRating()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                decimal ddlRating = Convert.ToDecimal(Request.Form["ratings"].FirstOrDefault());

                IEnumerable<Rating> ratings = _serviceProviderControllerRepository.GetServiceProviderRatingByServiceProviderId(Convert.ToInt32(sessionUser.UserID), ddlRating);

                recordsTotal = ratings.Count();
                var data = ratings.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult BlockCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBlockCustomerList()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var includePetatHome = Request.Form["includePetatHome"].FirstOrDefault();

                //IEnumerable<User> customerList = (from customer in _helperlandContext.Users
                //                    join serviceRequest in _helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == Convert.ToInt32(sessionUser.UserID))
                //                    on customer.UserId equals serviceRequest.UserId
                //                    //join favoriteAndBlockeds in _helperlandContext.FavoriteAndBlockeds
                //                    //on customer.UserId equals favoriteAndBlockeds.UserId
                //                    select new User
                //                    {
                //                        UserId = customer.UserId,
                //                        //FavoriteAndBlockedUsers = _helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == 1).ToList()
                //                    }).Include(x => x.FavoriteAndBlockedTargetUsers).Distinct();

                var customerList = _helperlandContext.Users.Join(_helperlandContext.ServiceRequests.Where(x => x.ServiceProviderId == Convert.ToInt32(sessionUser.UserID)),
                                                                         u => u.UserId,
                                                                         s => s.UserId,
                                                                         (user, serviceRequest) => user).Distinct();

                //List<User> customerList = _helperlandContext.Users.Join(_helperlandContext.ServiceRequests
                //                                                        .Where(x => x.ServiceProviderId == Convert.ToInt32(sessionUser.UserID)
                //                                                                && x.Status == (int)ServiceRequestStatusEnum.Completed),
                //                                                        u => u.UserId,
                //                                                        s => s.UserId,
                //                                                        (user, serviceRequest) => new User
                //                                                        {
                //                                                            UserId = user.UserId,
                //                                                            FirstName = user.FirstName
                //                                                        }).Join(_helperlandContext.FavoriteAndBlockeds
                //                                                                .Where(x => x.UserId == Convert.ToInt32(sessionUser.UserID)),
                //                                                                s => s.UserId,
                //                                                                f => f.UserId,
                //                                                                (user, favourite) => new {user, favourite }).SelectMany(
                //                                                                                            x => x.favourite.DefaultIfEmpty()
                //    ).Distinct().ToList();

                //var customerList = (from sr in _helperlandContext.ServiceRequests
                //                       join
                //                       usr in _helperlandContext.Users
                //                       on
                //                       sr.UserId equals usr.UserId
                //                       where sr.ServiceProviderId == Convert.ToInt32(sessionUser.UserID) && sr.Status == (int)ServiceRequestStatusEnum.Completed
                //                       select new
                //                       {
                //                           serviceproviderid = sr.ServiceProviderId,
                //                           customername = usr.FirstName,
                //                           customername2 = usr.LastName,
                //                           customerprofile = usr.UserProfilePicture,
                //                           customeruserid = usr.UserId,
                //                           blockeduser = _helperlandContext.FavoriteAndBlockeds.Where(x => x.TargetUserId == usr.UserId).FirstOrDefault()
                //                       }).Distinct();

                //return Json(serviceRequests);

                //List<User> customerList = _serviceProviderControllerRepository.GetUnblockedCustomerListByServiceProviderId(Convert.ToInt32(sessionUser.UserID));

                recordsTotal = customerList.Count();
                var data = customerList.Skip(skip).Take(pageSize).ToList();

                foreach (User temp in data)
                {
                    temp.FavoriteAndBlockedUsers = _helperlandContext.FavoriteAndBlockeds.Where(x => x.UserId == Convert.ToInt32(sessionUser.UserID) && x.TargetUserId == temp.UserId).ToList();
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateCustomerBlockStatus([FromBody] FavoriteAndBlockedViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            FavoriteAndBlocked favoriteAndBlocked = _serviceProviderControllerRepository.GetFavoriteAndBlockedByUserIdAndTargetUserId(Convert.ToInt32(sessionUser.UserID), model.TargetUserId);

            if (favoriteAndBlocked == null)
            {
                favoriteAndBlocked = new FavoriteAndBlocked
                {
                    UserId = Convert.ToInt32(sessionUser.UserID),
                    TargetUserId = model.TargetUserId,
                    IsFavorite = false,
                    IsBlocked = model.IsBlocked
                };

                _serviceProviderControllerRepository.AddFavoriteAndBlocked(favoriteAndBlocked);
            }
            else
            {
                favoriteAndBlocked.IsBlocked = model.IsBlocked;

                _serviceProviderControllerRepository.UpdateFavoriteAndBlocked(favoriteAndBlocked);
            }

            return Json(new SingleEntity<FavoriteAndBlockedViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetServiceProviderDetail()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            User customer = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

            customer.Password = null;

            return Json(new SingleEntity<User> { Result = customer, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult GetServiceProviderAddress()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            UserAddress userAddress = _serviceProviderControllerRepository.GetUserAddressByUserId(Convert.ToInt32(sessionUser.UserID));

            return Json(new SingleEntity<UserAddress> { Result = userAddress, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _serviceProviderControllerRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }

        [HttpPost]
        public JsonResult UpdateServiceProviderProfileDetails([FromBody] UserViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            User serviceProvider = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

            serviceProvider.FirstName = model.FirstName;
            serviceProvider.LastName = model.LastName;
            serviceProvider.Mobile = model.Mobile;
            serviceProvider.NationalityId = model.NationalityId;
            serviceProvider.Gender = model.Gender;

            if(model.DateOfBirth != null)
            {
                serviceProvider.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
            }
            
            serviceProvider.ZipCode = model.ZipCode;
            serviceProvider.WorksWithPets = model.WorksWithPets;
            serviceProvider.UserProfilePicture = model.UserProfilePicture;

            serviceProvider.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            serviceProvider.ModifiedDate = DateTime.Now;

            _serviceProviderControllerRepository.UpdateUser(serviceProvider);

            UserAddress userAddress = _serviceProviderControllerRepository.GetUserAddressByUserId(Convert.ToInt32(sessionUser.UserID));

            if (userAddress == null)
            {
                userAddress = new UserAddress();
            }

            userAddress.UserId = Convert.ToInt32(sessionUser.UserID);
            userAddress.AddressLine1 = model.UserAddress.StreetName;
            userAddress.AddressLine2 = model.UserAddress.HouseNumber;
            userAddress.City = model.UserAddress.City;
            userAddress.PostalCode = model.UserAddress.PostalCode;

            State state = _serviceProviderControllerRepository.GetStateByCityName(userAddress.City);

            userAddress.State = state.StateName;

            if (userAddress.AddressId == 0)
            {
                _serviceProviderControllerRepository.AddUserAddress(userAddress);
            }
            else
            {
                _serviceProviderControllerRepository.UpdateUserAddress(userAddress);
            }

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult UpdateServiceProviderPassword([FromBody] UserViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            User customer = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

            if (model.Password != customer.Password)
            {
                return Json(new SingleEntity<UserViewModel> { Result = model, Status = "Error", ErrorMessage = "Your current password is wrong!" });
            }

            customer.Password = model.NewPassword.ToString().Trim();

            _serviceProviderControllerRepository.UpdateUser(customer);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }
    }
}
