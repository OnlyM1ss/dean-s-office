using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.Repository.Sql
{
    public class SqlDisciplineRepository : IDisciplineRepository
    {
        private readonly ContosoContext _db;

        public SqlDisciplineRepository(ContosoContext db)
        {
            _db = db;
        }


        public Task<IEnumerable<Discipline>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Discipline> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<Discipline> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Discipline> UpsertAsync(Discipline discipline)
        {
            throw new NotImplementedException();
        }

        public Task<Discipline> DeleteAsync(Discipline disciplineId)
        {
            throw new NotImplementedException();
        }
    }
}