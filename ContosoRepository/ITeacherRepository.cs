using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAsync();
        Task<Teacher>  GetAsync(string search);
        Task<Teacher> GetAsync(Guid id);
        Task<Teacher> UpsertAsync(Teacher teacher);
        Task<Teacher> DeleteAsync(Guid teacherId);

    }
}
