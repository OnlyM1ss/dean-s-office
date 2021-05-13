using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository
{
    public interface IDisciplineRepository
    {
        Task<IEnumerable<Discipline>> GetAsync();
        Task<Discipline> GetAsync(string search);
        Task<Discipline> GetAsync(Guid id);
        Task<Discipline> UpsertAsync(Discipline discipline);
        Task<Discipline> DeleteAsync(Discipline disciplineId);
    }
}
