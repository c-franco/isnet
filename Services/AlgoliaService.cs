using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using sisnet.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sisnet.Services
{
    public class AlgoliaService
    {
        private readonly ISearchClient _client;
        private readonly ISearchIndex _index;

        public AlgoliaService(string applicationId, string apiKey, string indexName)
        {
            _client = new SearchClient(applicationId, apiKey);
            _index = _client.InitIndex(indexName);
        }

        public async Task<List<Song>> SearchSongsAsync(string query)
        {
            var searchResult = await _index.SearchAsync<Song>(new Query(query));
            return searchResult.Hits;
        }
    }
}
