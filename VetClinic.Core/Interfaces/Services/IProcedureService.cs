using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services.Base;

namespace VetClinic.Core.Interfaces.Services
{
    public interface IProcedureService : IBaseService<Procedure, int>
    {
        public Task<IList<Procedure>> GetProceduresAsync();
    }
}
