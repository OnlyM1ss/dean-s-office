using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class SqlGroupRepository : IGroupRepository
    {
        private readonly ContosoContext _db;

        public SqlGroupRepository(ContosoContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Group>> GetAsync()
        {
            return await _db.Groups
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Group> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public async Task<Group> GetAsync(Guid id)
        {
            return await _db.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(group => group.Id == id);
        }

        public async Task<Group> UpsertAsync(Group group)
        {
            var current = await _db.Groups.FirstOrDefaultAsync(_group => _group.Id == group.Id);
            if (null == current)
            {
                await _db.Groups.AddAsync(group);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(group);
            }

            return group;
        }

        public async Task<Group> DeleteAsync(Guid groupId)
        {
            var current = await _db.Groups.FirstOrDefaultAsync(_group => _group.Id == groupId);
            if (null != current)
            {
                var teachers = await _db.Groups.Where(_group => _group.Id == groupId).ToListAsync();
                _db.Groups.RemoveRange(current);
                _db.Groups.Remove(current);
                await _db.SaveChangesAsync();
            }

            return current;
        }
    }
}
