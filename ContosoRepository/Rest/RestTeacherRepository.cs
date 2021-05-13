using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Contoso.Repository.Rest;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class RestTeacherRepository : ITeacherRepository
    {
        private readonly HttpHelper _http;

        public RestTeacherRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public Task<IEnumerable<Teacher>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Teacher>> GetAsync(string search)
        {
           throw  new NotImplementedException();
        }

        public async Task<Teacher> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Teacher> UpsertAsync(Teacher teacher)
        {
            throw new NotImplementedException();
        }

        public async Task<Teacher> DeleteAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }
    }
}
