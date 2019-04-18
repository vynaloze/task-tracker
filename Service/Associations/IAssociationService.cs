using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;

namespace Service.Associations
{
    public interface IAssociationService
    {
        Task<IEnumerable<Association>> Get();
        Task<Association> Get(int todoId);
        Task<Association> Create(int todoId, int? projectId, int? userId);
        Task<bool> Update(int todoId, int? projectId, int? userId);
        Task<bool> Delete(int todoId);

    }
}