using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string body);
    }
}
