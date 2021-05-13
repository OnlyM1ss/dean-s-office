using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class SqlTeacherRepository : ITeacherRepository
    {
        private readonly ContosoContext _db;

        public SqlTeacherRepository(ContosoContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Teacher>> GetAsync()
        {
            return await _db.Teachers
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<IEnumerable<Teacher>> GetAsync(string search)
        {
           throw  new NotImplementedException();
        }

        public async Task<Teacher> GetAsync(Guid id)
        {
            return await _db.Teachers
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<Teacher> UpsertAsync(Teacher teacher)
        {
            var current = await _db.Teachers.FirstOrDefaultAsync(_teacher => _teacher.Id == teacher.Id);
            if (null == current)
            {
                await _db.Teachers.AddAsync(teacher);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(teacher);
            }

            await _db.SaveChangesAsync();
            return current;
        }

        public async Task<Teacher> DeleteAsync(Guid teacherId)
        {
            var teacher = await _db.Teachers.FirstOrDefaultAsync(_customer => _customer.Id == teacherId);
            if (null != teacher)
            {
                var teachers = await _db.Teachers.Where(_teacher => teacher.Id == teacherId).ToListAsync();
                _db.Teachers.RemoveRange(teachers);
                _db.Teachers.Remove(teacher);
                await _db.SaveChangesAsync();
            }

            return teacher;
        }
    }
}
