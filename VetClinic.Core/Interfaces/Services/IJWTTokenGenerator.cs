using VetClinic.Core.Entities;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IJWTTokenGenerator
    {
        public string GenerateToken(User user, string role);
    }
}
