using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository.Sql
{
    public class SqlPositionRepository : IPositionRepository
    {
        private readonly ContosoContext _db;

        public SqlPositionRepository(ContosoContext db)
        {
            _db = db;
        }

        public Task<IEnumerable<Position>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Position> UpsertAsync(User position)
        {
            throw new NotImplementedException();
        }

        public Task<Position> DeleteAsync(Guid positionId)
        {
            throw new NotImplementedException();
        }
    }
}
