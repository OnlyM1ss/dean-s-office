using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetAsync();
        Task<Position> GetAsync(string search);
        Task<Position> GetAsync(Guid id);
        Task<Position> UpsertAsync(User position);
        Task<Position> DeleteAsync(Guid positionId);
    }
}
