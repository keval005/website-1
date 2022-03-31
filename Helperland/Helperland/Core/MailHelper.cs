using Helperland.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Helperland.Core
{
    public class MailHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MailHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public MailHelper(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this._configuration = configuration;
            this._hostingEnvironment = hostingEnvironment;
        }

        public bool SendResetPasswordLink(EmailModel model)
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);

                model.From = _configuration["Gmail:Username"];

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };

                var emailTemplatePath = _hostingEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" + Path.DirectorySeparatorChar.ToString() + "ForgotPasswordEmailTemplate.html";

                string mailBody = string.Empty;

                using (StreamReader SourceReader = System.IO.File.OpenText(emailTemplatePath))
                {
                    mailBody = SourceReader.ReadToEnd();
                }

                mailBody = mailBody.Replace("{DisplayName}", model.DisplayName.ToString());
                mailBody = mailBody.Replace("{ResetPasswordUrl}", model.Body.ToString());

                var mailMessage = new MailMessage(model.From, model.To, model.Subject, mailBody);
                mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendContactUsDetail(EmailModel model)
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);

                model.From = _configuration["Gmail:Username"];
                model.To = _configuration["Gmail:adminEmail"];

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };

                var mailMessage = new MailMessage(model.From, model.To, model.Subject, model.Body);
                
                mailMessage.IsBodyHtml = true;

                if (model.Attachment != null)
                {
                    mailMessage.Attachments.Add(new Attachment(model.Attachment));
                }

                smtpClient.Send(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendMail(EmailModel model)
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);

                model.From = _configuration["Gmail:Username"];
                model.To = model.To;

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };

                var mailMessage = new MailMessage(model.From, model.To, model.Subject, model.Body);

                mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
