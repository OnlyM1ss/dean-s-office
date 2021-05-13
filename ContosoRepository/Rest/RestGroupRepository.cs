using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Contoso.Repository.Rest;

namespace Contoso.Repository.Sql
{
    public class RestGroupRepository : IGroupRepository
    {
        private readonly HttpHelper _httpHelper;

        public RestGroupRepository(string baseUrl)
        {
            _httpHelper = new HttpHelper(baseUrl);
        }

        public Task<IEnumerable<Group>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Group> UpsertAsync(Group @group)
        {
            throw new NotImplementedException();
        }

        public Task<Group> DeleteAsync(Guid groupId)
        {
            throw new NotImplementedException();
        }
    }
}
