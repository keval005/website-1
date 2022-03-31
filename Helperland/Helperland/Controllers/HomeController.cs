using Helperland.Core;
using Helperland.Data;
using Helperland.Enums;
using Helperland.Models;
using Helperland.Repository;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    [CookieHelper]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHomeControllerRepository _homeControllerRepository;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment, IConfiguration configuration,
            IHomeControllerRepository homeControllerRepository)
        {
            _logger = logger;
            this._hostingEnvironment = hostingEnvironment;
            this._configuration = configuration;
            this._homeControllerRepository = homeControllerRepository;
        }

        public IActionResult Index()
        {
            if (TempData["OpenModel"] != null)
            {
                @ViewBag.OpenModel = TempData["OpenModel"].ToString();
            }
            ViewBag.navbar = "transparentNavbar";
            return View();
        }

        public IActionResult about()
        {
            return View();
        }

        public IActionResult faq()
        {
            return View();
        }

        public IActionResult prices()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                ContactU contactU = new ContactU
                {
                    Name = model.FirstName + " " + model.LastName,
                    PhoneNumber = model.MobileNumber,
                    Email = model.EmailAddress,
                    Subject = model.Subject,
                    Message = model.Message
                };

                string attachmentFilePath = "";

                if (model.Attachment != null)
                {
                    string uniqueAttachmentName = null;
                    string uploadsAttachmentFolder = Path.Combine(_hostingEnvironment.WebRootPath, "upload\\contact-us-attachment");
                    uniqueAttachmentName = Guid.NewGuid().ToString() + "_" + model.Attachment.FileName;
                    attachmentFilePath = Path.Combine(uploadsAttachmentFolder, uniqueAttachmentName);
                    using (var fileStream = new FileStream(attachmentFilePath, FileMode.Create))
                    {
                        model.Attachment.CopyTo(fileStream);
                    }

                    contactU.UploadFileName = uniqueAttachmentName;
                }

                _homeControllerRepository.AddContactUs(contactU);

                //Send email to admin

                EmailModel emailModel = new EmailModel
                {
                    Subject = contactU.Subject,
                    Body = "You have new Query.<br>Name : " + contactU.Name + "<br>Mobile number : " + contactU.PhoneNumber + "<br>Email : " + contactU.Email + "<br>Subject : " + contactU.Subject + "<br>Message : " + contactU.Message,
                    Attachment = attachmentFilePath
                };

                MailHelper mailHelper = new MailHelper(_configuration);
                mailHelper.SendContactUsDetail(emailModel);

                ModelState.Clear();

                return View();
            }

            return View(model);
        }

        [SessionHelper(userType: UserTypeEnum.Customer)]
        public IActionResult BookService()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckPostalCode(string postalCode)
        {
            var user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();

            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            List<User> serviceProvider = _homeControllerRepository.GetUserByPostalCodeAndCustomerId(postalCode, Convert.ToInt32(sessionUser.UserID));
            bool IsServiceProviderAvailable = false;
            if (serviceProvider.Any())
            {
                IsServiceProviderAvailable = true;
            }
            return Json(IsServiceProviderAvailable);
        }

        public IActionResult GetCustomerAddressList(int userId, string postalCode)
        {
            List<UserAddress> userAddresseList = _homeControllerRepository.GetUserAddress(userId, postalCode);
            return View("BookServiceCustomerAddressList", userAddresseList);
        }

        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _homeControllerRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }

        [HttpPost]
        public JsonResult AddCustomerAddress([FromBody] UserAddressViewModel userAddressViewModel)
        {
            string user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();
            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            State state = _homeControllerRepository.GetStateByCityName(userAddressViewModel.City.ToString().Trim());

            UserAddress userAddress = new UserAddress
            {
                AddressLine1 = userAddressViewModel.StreetName.ToString().Trim(),
                AddressLine2 = userAddressViewModel.HouseNumber.ToString().Trim(),
                PostalCode = userAddressViewModel.PostalCode,
                City = userAddressViewModel.City.ToString().Trim(),
                State = state.StateName,
                Mobile = userAddressViewModel.PhoneNumber,
                UserId = Convert.ToInt32(sessionUser.UserID),
                Email = sessionUser.Email.ToString().Trim()
            };
            userAddress = _homeControllerRepository.AddUserAddress(userAddress);
            return Json(userAddress);
        }

        [HttpPost]
        public JsonResult BookCustomerServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = new ServiceRequest
            {
                UserId = model.UserId,
                ServiceId = 0,
                ServiceStartDate = Convert.ToDateTime(model.ServiceStartDate.ToString().Trim() + " " + model.ServiceStartTime.ToString().Trim()),
                ZipCode = model.PostalCode.ToString().Trim(),
                ServiceHourlyRate = model.ServiceHourlyRate,
                ServiceHours = model.ServiceHours,
                ExtraHours = model.ExtraHours,
                SubTotal = Convert.ToDecimal(model.SubTotal),
                Discount = 0,
                TotalCost = Convert.ToDecimal(model.TotalCost),
                Comments = model.Comments.ToString().Trim(),
                PaymentDue = false,
                HasPets = model.HasPets,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Distance = 0,
                Status = (int)ServiceRequestStatusEnum.New,
                RecordVersion = Guid.NewGuid()
            };

            _homeControllerRepository.AddServiceRequest(serviceRequest);

            model.ServiceRequestId = serviceRequest.ServiceRequestId;

            UserAddress userAddress = _homeControllerRepository.SelectUserAddressByPK(Convert.ToInt32(model.UserAddressId));

            ServiceRequestAddress serviceRequestAddress = new ServiceRequestAddress
            {
                ServiceRequestId = serviceRequest.ServiceRequestId,
                AddressLine1 = userAddress.AddressLine1,
                AddressLine2 = userAddress.AddressLine2,
                City = userAddress.City,
                State = userAddress.State,
                PostalCode = userAddress.PostalCode,
                Mobile = userAddress.Mobile,
                Email = userAddress.Email
            };

            _homeControllerRepository.AddServiceRequestAddress(serviceRequestAddress);

            ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra
            {
                ServiceRequestId = serviceRequest.ServiceRequestId
            };

            foreach (string extraService in model.ExtraServicesName)
            {
                serviceRequestExtra.ServiceRequestExtraId = 0;
                serviceRequestExtra.ServiceExtraId = Convert.ToInt32((ExtraServiceEnum)System.Enum.Parse(typeof(ExtraServiceEnum), extraService));
                _homeControllerRepository.AddServiceRequestExtra(serviceRequestExtra);
            }

            List<User> serviceProviders = _homeControllerRepository.GetUserByPostalCodeAndCustomerId(model.PostalCode.ToString().Trim(), Convert.ToInt32(serviceRequest.UserId));

            if (serviceProviders.Any())
            {
                MailHelper mailHelper = new MailHelper(_configuration);
                EmailModel emailModel = new EmailModel();

                emailModel.Subject = "Customer Service Request";
                emailModel.Body = "Hi {{DisplayName}},<br> There is new Service Request in your area.<br> " +
                    "Service Request Id :" + serviceRequest.ServiceRequestId + "<br> " +
                    "Address : " + serviceRequestAddress.AddressLine1 + " " + serviceRequestAddress.AddressLine2 + ", " + serviceRequestAddress.City + ", " + serviceRequestAddress.State + " <br> " +
                    "Postal Code:" + serviceRequestAddress.PostalCode + " <br> " +
                    "Click on Link to accept request : <a href=\"http://" + this.Request.Host.ToString() + "/ServiceProvider/NewServiceRequest\">Accept Now</a>";

                foreach (User user in serviceProviders)
                {
                    if (model.HasPets == true && user.WorksWithPets == false)
                    {
                        continue;
                    }
                    emailModel.To = user.Email;
                    emailModel.Body = emailModel.Body.Replace("{{DisplayName}}", user.FirstName.ToString() + " " + user.LastName.ToString());
                    mailHelper.SendMail(emailModel);
                }
            }

            return Json(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
