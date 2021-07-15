using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IPositionService : IBaseService<Position, int>
    {
        public Task<IList<Position>> GetPositionsAsync();
    }
}
