using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAsync();
        Task<User> GetAsync(string search);
        Task<User> GetAsync(Guid id);
        Task<User> UpsertAsync(User user);
        Task<User> DeleteAsync(Guid userId);
    }
}
