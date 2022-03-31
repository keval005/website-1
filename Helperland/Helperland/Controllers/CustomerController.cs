using Helperland.Core;
using Helperland.Data;
using Helperland.Enums;
using Helperland.Models;
using Helperland.Repository;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    [CookieHelper]
    [SessionHelper(userType: UserTypeEnum.Customer)]
    public class CustomerController : Controller
    {
        private readonly ICustomerControllerRepository _customerControllerRepository;
        private readonly IConfiguration _configuration;
        private readonly HelperlandContext _helperlandContext;

        public CustomerController(ICustomerControllerRepository customerControllerRepository, IConfiguration configuration,
            HelperlandContext helperlandContext)
        {
            this._customerControllerRepository = customerControllerRepository;
            this._configuration = configuration;
            this._helperlandContext = helperlandContext;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetCurrentServiceRequestList()
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

                var serviceRequest = _customerControllerRepository.GetCurrentServiceRequestByCustomerId(Convert.ToInt32(sessionUser.UserID));

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
                    case "ServiceProvider_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.FirstName).ThenBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.LastName);
                        break;
                    case "ServiceProvider_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.FirstName).ThenBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.LastName);
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
                    if (temp.ServiceProviderId != null)
                    {
                        //temp.User = _customerControllerRepository.GetUserByPK(Convert.ToInt32(temp.ServiceProviderId));
                        temp.Ratings = _customerControllerRepository.GetRatingsByServiceProviderId(temp.ServiceProviderId);
                    }
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
        public JsonResult GetServiceRequest(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _customerControllerRepository.GetServiceRequest(serviceRequestId);

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
        public JsonResult UpdateRescheduleServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _customerControllerRepository.GetServiceRequest(model.ServiceRequestId);

            DateTime newServiceRequestStartDateTime = Convert.ToDateTime(model.ServiceStartDate + " " + model.ServiceStartTime);
            DateTime newServiceRequestEndDateTime = newServiceRequestStartDateTime.AddMinutes(serviceRequest.ServiceHours * 60);

            DateTime dateLimit = Convert.ToDateTime(model.ServiceStartDate).AddHours(21);

            if (newServiceRequestEndDateTime > dateLimit)
            {
                return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "Error", ErrorMessage = "Could not completed the service request, because service booking request is must be completed within 21:00 time" });
            }

            if (serviceRequest.ServiceProviderId != null)
            {
                List<ServiceRequest> serviceRequestList = _customerControllerRepository.GetFutureServiceRequestByServiceProviderId(Convert.ToInt32(serviceRequest.ServiceProviderId));

                Boolean serviceRequestConflict = false;

                string errorMessage = "";

                foreach (ServiceRequest temp in serviceRequestList)
                {
                    if (serviceRequest.ServiceRequestId != temp.ServiceRequestId)
                    {
                        DateTime serviceRequestStartDateTime = temp.ServiceStartDate;
                        DateTime serviceRequestEndDateTime = serviceRequestStartDateTime.AddMinutes(temp.ServiceHours * 60);

                        if (serviceRequestStartDateTime <= newServiceRequestEndDateTime && newServiceRequestStartDateTime <= serviceRequestEndDateTime)
                        {
                            serviceRequestConflict = true;
                            errorMessage = "Another service request has been assigned to the service provider on " + serviceRequestStartDateTime.ToShortDateString()
                                + " from " + serviceRequestStartDateTime.ToShortTimeString() + " to " + serviceRequestEndDateTime.ToShortTimeString() + ". Either choose another date or pick up a different time slot";
                            break;
                        }
                    }
                }

                if (serviceRequestConflict == true)
                {
                    return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "Error", ErrorMessage = errorMessage });
                }
            }

            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            serviceRequest.ServiceStartDate = newServiceRequestStartDateTime;
            serviceRequest.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            serviceRequest.ModifiedDate = DateTime.Now;

            _customerControllerRepository.UpdateServiceRequest(serviceRequest);

            if (serviceRequest.ServiceProviderId != null)
            {
                User sp = _customerControllerRepository.GetUserByPK(Convert.ToInt32(serviceRequest.ServiceProviderId));

                MailHelper mailHelper = new MailHelper(_configuration);
                EmailModel emailModel = new EmailModel();

                emailModel.To = sp.Email;
                emailModel.Subject = "Reschedule ServiceRequest";
                emailModel.Body = "Service Request " + serviceRequest.ServiceRequestId + " has been rescheduled by customer. New date and time are " +
                    serviceRequest.ServiceStartDate.ToShortDateString() + " " + serviceRequest.ServiceStartDate.ToShortTimeString() + ".";

                mailHelper.SendMail(emailModel);
            }

            return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult CancelServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _customerControllerRepository.GetServiceRequest(model.ServiceRequestId);

            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            serviceRequest.Status = (int)ServiceRequestStatusEnum.Cancelled;
            serviceRequest.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            serviceRequest.ModifiedDate = DateTime.Now;

            _customerControllerRepository.UpdateServiceRequest(serviceRequest);

            if (serviceRequest.ServiceProviderId != null)
            {
                User sp = _customerControllerRepository.GetUserByPK(Convert.ToInt32(serviceRequest.ServiceProviderId));

                MailHelper mailHelper = new MailHelper(_configuration);
                EmailModel emailModel = new EmailModel();

                emailModel.To = sp.Email;
                emailModel.Subject = "Cancel ServiceRequest";
                emailModel.Body = "Service Request " + serviceRequest.ServiceRequestId + " cancelled by customer." +
                    " Reason for cancel service request : " + model.Comments + ".";

                mailHelper.SendMail(emailModel);
            }

            return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "ok", ErrorMessage = null });
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

                var serviceRequestList = _customerControllerRepository.GetServiceRequestHistoryListByCustomerId(Convert.ToInt32(sessionUser.UserID));

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceDateTime_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceDateTime_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "ServiceProvider_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.FirstName).ThenBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.LastName);  //check once for sorting
                        break;
                    case "ServiceProvider_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.FirstName).ThenBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.LastName);
                        break;
                    case "TotalCost_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.TotalCost);
                        break;
                    case "TotalCost_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.TotalCost);
                        break;
                    default:
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequestList.Count();
                var data = serviceRequestList.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    if (temp.ServiceProviderId != null)
                    {
                        //temp.User = _customerControllerRepository.GetUserByPK(Convert.ToInt32(temp.ServiceProviderId));
                        temp.Ratings = _customerControllerRepository.GetRatingsByServiceProviderId(temp.ServiceProviderId);
                    }
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
        public JsonResult GetServiceProvicerRatingFromServiceRequest(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _customerControllerRepository.GetServiceRequest(serviceRequestId);

            Rating rating = _customerControllerRepository.GetRatingsByServiceRequestId(serviceRequestId);

            RatingViewModel ratingViewModel = new RatingViewModel();

            if (rating != null)
            {
                ratingViewModel.RatingId = rating.RatingId;
                ratingViewModel.RatingFrom = rating.RatingFrom;
                ratingViewModel.RatingTo = rating.RatingTo;
                ratingViewModel.RatingDate = rating.RatingDate;
                ratingViewModel.Ratings = rating.Ratings;
                ratingViewModel.Comments = rating.Comments;
                ratingViewModel.RatingDate = rating.RatingDate;
                ratingViewModel.OnTimeArrival = rating.OnTimeArrival;
                ratingViewModel.Friendly = rating.Friendly;
                ratingViewModel.QualityOfService = rating.QualityOfService;
            }

            List<Rating> serviceProviderRating = _customerControllerRepository.GetRatingsByServiceProviderId(serviceRequest.ServiceProviderId);

            decimal totalRating = 0;
            ratingViewModel.ServiceProviderRating = 0;
            ratingViewModel.ServiceRequestId = serviceRequestId;

            foreach (Rating temp in serviceProviderRating)
            {
                totalRating = totalRating + temp.Ratings;
            }

            if (serviceProviderRating.Any())
            {
                ratingViewModel.ServiceProviderRating = totalRating / serviceProviderRating.Count;
                ratingViewModel.ServiceProviderRating = Math.Round(ratingViewModel.ServiceProviderRating * 10) / 10;
            }

            ratingViewModel.ServiceProvider = _customerControllerRepository.GetUserByPK(Convert.ToInt32(serviceRequest.ServiceProviderId));

            return Json(new SingleEntity<RatingViewModel> { Result = ratingViewModel, Status = "ok", ErrorMessage = null });
        }

        public JsonResult SubmitRatingFromCustomer([FromBody] RatingViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            Rating rating = new Rating
            {
                ServiceRequestId = model.ServiceRequestId,
                RatingFrom = Convert.ToInt32(sessionUser.UserID),
                RatingTo = model.RatingTo,
                Ratings = model.Ratings,
                Comments = model.Comments,
                RatingDate = DateTime.Now,
                OnTimeArrival = model.OnTimeArrival,
                Friendly = model.Friendly,
                QualityOfService = model.QualityOfService
            };

            _customerControllerRepository.AddRating(rating);

            return Json(new SingleEntity<RatingViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public JsonResult GetCustomerDetail()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            User customer = _customerControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

            return Json(new SingleEntity<User> { Result = customer, Status = "ok", ErrorMessage = null });
        }

        public JsonResult GetCustomerAddresses()
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            List<UserAddress> customerAddresses = _customerControllerRepository.GetUserAddressByUserId(Convert.ToInt32(sessionUser.UserID));

            return Json(new SingleEntity<List<UserAddress>> { Result = customerAddresses, Status = "ok", ErrorMessage = null });
        }

        public JsonResult DeleteCustomerAddress(string addressId)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            UserAddress customerAddress = _customerControllerRepository.GetUserAddressByPK(Convert.ToInt32(addressId), Convert.ToInt32(sessionUser.UserID));

            _customerControllerRepository.DeleteUserAddress(customerAddress);

            return Json(new SingleEntity<UserAddress> { Result = customerAddress, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _customerControllerRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }

        [HttpPost]
        public JsonResult GetCustomerAddress(string addressId)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            UserAddress customerAddress = _customerControllerRepository.GetUserAddressByPK(Convert.ToInt32(addressId), Convert.ToInt32(sessionUser.UserID));

            return Json(new SingleEntity<UserAddress> { Result = customerAddress, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult SaveCustomerAddress([FromBody] UserAddressViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            State state = _customerControllerRepository.GetStateByCityName(model.City.ToString().Trim());

            UserAddress userAddress = new UserAddress
            {
                AddressLine1 = model.StreetName.ToString().Trim(),
                AddressLine2 = model.HouseNumber.ToString().Trim(),
                City = model.City.ToString().Trim(),
                State = state.StateName,
                PostalCode = model.PostalCode.ToString().Trim(),
                Mobile = model.PhoneNumber.ToString().Trim(),
                UserId = Convert.ToInt32(sessionUser.UserID)
            };

            if (string.IsNullOrEmpty(model.AddressId))
            {
                userAddress = _customerControllerRepository.AddUserAddress(userAddress);
                model.AddressId = userAddress.AddressId.ToString();
            }
            else
            {
                userAddress.AddressId = Convert.ToInt32(model.AddressId);
                userAddress = _customerControllerRepository.UpdateUserAddress(userAddress);
            }

            return Json(new SingleEntity<UserAddressViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult UpdateCustomerPassword([FromBody] UserViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            User customer = _customerControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

            if(model.Password != customer.Password)
            {
                return Json(new SingleEntity<UserViewModel> { Result = model, Status = "Error", ErrorMessage = "Your current password is wrong!" });
            }

            customer.Password = model.NewPassword.ToString().Trim();

            _customerControllerRepository.UpdateUser(customer);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult UpdateCustomerProfileDetail([FromBody] UserViewModel model)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            User customer = _customerControllerRepository.GetUserByPK(Convert.ToInt32(sessionUser.UserID));

            customer.FirstName = model.FirstName.ToString().Trim();
            customer.LastName = model.LastName.ToString().Trim();
            customer.Mobile = model.Mobile.ToString().Trim();
            customer.LanguageId = model.LanguageId;


            if(model.DateOfBirth != null)
            {
                customer.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
            }

            customer.ModifiedBy = Convert.ToInt32(sessionUser.UserID);
            customer.ModifiedDate = DateTime.Now;

            _customerControllerRepository.UpdateUser(customer);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }
    }
}
