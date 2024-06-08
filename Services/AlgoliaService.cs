using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using sisnet.Models;

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

        public async Task<List<Song>> SearchSongsAsync(string query, int page, int pageSize, int startYear = 2000, int endYear = 2024)
        {
           try
           {
                Validations(startYear, endYear);

                string filters = CreateFilters((int)startYear, (int)endYear);

                var searchResult = await _index.SearchAsync<Song>(new Query(query)
                {
                    Page = page - 1,
                    HitsPerPage = pageSize,
                    Filters = filters
                });

                return searchResult.Hits.ToList();
           }
           catch (Exception)
           {
                return new List<Song>();
           }
        }

        public async Task<List<Song>> GetTrendingSongsAsync(int pageSize)
        {
            var searchParameters = new Query("")
            {
                HitsPerPage = pageSize,
                Page = 0,
            };

            var searchResult = await _index.SearchAsync<Song>(searchParameters);
            return searchResult.Hits.ToList();
        }

        private string CreateFilters(int startYear, int endYear)
        {
            string filters = "";

            for (int i = 0; i <= endYear - startYear; i++)
            {
                filters += $"year:{startYear + i}";
                if (i <= endYear - startYear - 1)
                    filters += " OR ";
            }

            return filters;
        }

        private void Validations(int startYear, int endYear)
        {
            if (startYear.ToString().Length < 4) startYear = 2000;
            if (endYear.ToString().Length < 4) endYear = 2024;

            if (startYear > endYear)
            {
                throw new Exception();
            }
        }
    }
}
