using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class SqlPositionRepository : IPositionRepository
    {
        private readonly ContosoContext _db;

        public SqlPositionRepository(ContosoContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Position>> GetAsync()
        {
            return await _db.Positions
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Position> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public async Task<Position> GetAsync(Guid id)
        {
            return await _db.Positions
                .AsNoTracking()
                .FirstOrDefaultAsync(pos => pos.Id == id);
        }

        public async Task<Position> UpsertAsync(Position position)
        {
            var current = await _db.Positions.FirstOrDefaultAsync(_pos => _pos.Id == position.Id);
            if (null == current)
            {
                await _db.Positions.AddAsync(position);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(position);
            }

            return position;
        }

        public async Task<Position> DeleteAsync(Guid positionId)
        {
            var current = await _db.Positions.FirstOrDefaultAsync(_pos => _pos.Id == positionId);
            if (null != current)
            {
                var teachers = await _db.Disciplines.Where(_pos => _pos.Id == positionId).ToListAsync();
                _db.Positions.RemoveRange(current);
                _db.Positions.Remove(current);
                await _db.SaveChangesAsync();
            }

            return current;
        }
    }
}
