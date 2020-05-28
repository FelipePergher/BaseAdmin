// <copyright file="EmailSender.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BaseAdminProject.Business.Core
{
    public class EmailSender : IEmailSender
    {
        // Our private configuration variables
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSsl;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _emailFrom;

        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSsl, string userName, string password, string emailFrom)
        {
            this._host = host;
            this._port = port;
            this._enableSsl = enableSsl;
            this._userName = userName;
            this._emailFrom = emailFrom;
            this._password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _enableSsl
            };

            return client.SendMailAsync(new MailMessage(_emailFrom, email, subject, htmlMessage) { IsBodyHtml = true });
        }
    }
}
