using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Contoso.Repository.Rest;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class RestUserRepository : IUserRepository
    {
        private readonly HttpHelper _http;
        public RestUserRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
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