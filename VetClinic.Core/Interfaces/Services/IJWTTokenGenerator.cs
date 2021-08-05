using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IJWTTokenGenerator
    {
        public string GenerateToken(User user);
    }
}
