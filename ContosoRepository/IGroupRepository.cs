using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAsync();
        Task<Group> GetAsync(string search);
        Task<Group> GetAsync(Guid id);
        Task<Group> UpsertAsync(Group group);
        Task<Group> DeleteAsync(Guid groupId);
    }
}
