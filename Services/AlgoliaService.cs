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

        public async Task<List<Song>> SearchSongsAsync(string query, int page, int pageSize, string genres, int startYear = 2000, int endYear = 2024)
        {
           try
           {
                Validations(startYear, endYear);

                string filters = CreateFilters((int)startYear, (int)endYear, genres);

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

        public async Task<List<Genre>> GetGenresAsync()
        {
            var results = await _index.SearchAsync<Song>(new Query("")
            {
                Facets = new List<string> { "genre" },
                HitsPerPage = 0
            });

            var genres = results.Facets
                .SelectMany(f => f.Value.Keys)
                .Distinct()
                .Select(g => new Genre
                {
                    Key = g,
                    Value = FormatGenre(g)
                })
                .OrderBy(g => g.Value)
                .ToList();

            return genres;
        }

        private string FormatGenre(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return genre;
            }

            return string.Join(" ", genre.Split('-').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }

        private string CreateFilters(int startYear, int endYear, string genres)
        {
            string filters = "";

            // Years
            for (int i = 0; i <= endYear - startYear; i++)
            {
                filters += $"year:{startYear + i}";
                if (i <= endYear - startYear - 1)
                {
                    filters += " OR ";
                }
            }

            // Genres
            if(!string.IsNullOrEmpty(genres))
            {
                filters += " AND ";

                string[] splitGenres = genres.Split(',');
                for (int i = 0; i < splitGenres.Length; i++)
                {
                    filters += $"genre:{splitGenres[i]}";
                    if (i < splitGenres.Length - 1)
                    {
                        filters += " OR ";
                    }
                }
            }

            return filters;
        }

        private void Validations(int startYear, int endYear)
        {
            if (startYear.ToString().Length < 4)
            {
                startYear = 2000;
            }

            if (endYear.ToString().Length < 4)
            {
                endYear = 2024;
            }

            if (startYear > endYear)
            {
                throw new Exception();
            }
        }
    }
}
