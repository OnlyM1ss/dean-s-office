using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<User> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<User> UpsertAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> DeleteAsync(Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(_user => _user.Id == userId);
            if (null != user)
            {
                var teachers = await _db.Teachers.Where(_user => _user.Id == userId).ToListAsync();
                _db.Users.RemoveRange(user);
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }

            return user;
        }
    }
}