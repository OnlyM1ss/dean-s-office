using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class SqlDisciplineRepository : IDisciplineRepository
    {
        private readonly ContosoContext _db;

        public SqlDisciplineRepository(ContosoContext db)
        {
            _db = db;
        }


        public async Task<IEnumerable<Discipline>> GetAsync()
        {
            return await _db.Disciplines
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Discipline>> GetAsync(string search)
        {
            string[] parameters = search.Split(' ');
            return await _db.Disciplines
                .Where(discipline =>
                    parameters.Any(parameter =>
                        discipline.Name.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Discipline> GetAsync(Guid id)
        {
            return await _db.Disciplines
                .AsNoTracking()
                .FirstOrDefaultAsync(disc => disc.Id == id);
        }

        public async Task<Discipline> UpsertAsync(Discipline discipline)
        {
            var current = await _db.Disciplines.FirstOrDefaultAsync(_discipline => _discipline.Id == discipline.Id);
            if (null == current)
            {
                await _db.Disciplines.AddAsync(discipline);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(discipline);
            }

            return discipline;
        }

        public async Task<Discipline> DeleteAsync(Guid disciplineId)
        {
            var current = await _db.Disciplines.FirstOrDefaultAsync(_disc => _disc.Id == disciplineId);
            if (null != current)
            {
                var teachers = await _db.Disciplines.Where(_discipline => _discipline.Id == disciplineId).ToListAsync();
                _db.Disciplines.RemoveRange(current);
                _db.Disciplines.Remove(current);
                await _db.SaveChangesAsync();
            }

            return current;
        }
    }
}