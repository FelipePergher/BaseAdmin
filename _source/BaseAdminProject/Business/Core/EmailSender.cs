// <copyright file="EmailSender.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BaseAdminProject.Business.Core
{
    public class EmailSender : IEmailSender
    {
        // Our private configuration variables
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSSL;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _emailFrom;

        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSSL, string userName, string password, string emailFrom)
        {
            this._host = host;
            this._port = port;
            this._enableSSL = enableSSL;
            this._userName = userName;
            this._emailFrom = emailFrom;
            this._password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Todo send email
            return Task.CompletedTask;
        }
    }
}
