using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly ContosoContext _db;
        public SqlUserRepository(ContosoContext db)
        {
            _db = db;
        }

        public Task<IEnumerable<User>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpsertAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}