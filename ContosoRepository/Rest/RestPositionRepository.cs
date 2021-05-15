using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;
using Contoso.Repository.Rest;

namespace Contoso.Repository.Sql
{
    public class RestPositionRepository : IPositionRepository
    {
        private readonly HttpHelper _http;

        public RestPositionRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }
        public Task<IEnumerable<Position>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Position> UpsertAsync(Position position)
        {
            throw new NotImplementedException();
        }

        public Task<Position> DeleteAsync(Guid positionId)
        {
            throw new NotImplementedException();
        }
    }
}
