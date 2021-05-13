using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Contoso.Repository.Rest;

namespace Contoso.Repository.Sql
{
    public class RestDisciplineRepository : IDisciplineRepository
    {
        private readonly HttpHelper _httpHelper;

        public RestDisciplineRepository(string baseUrl)
        {
            _httpHelper = new HttpHelper(baseUrl);
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