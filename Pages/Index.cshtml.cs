using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sisnet.Models;
using sisnet.Services;

namespace sisnet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AlgoliaService _algoliaService;

        public IndexModel(AlgoliaService algoliaService)
        {
            _algoliaService = algoliaService;
        }

        public async Task<JsonResult> OnGetSearchSongsAsync(string query, int currentPage, int pageSize)
        {
            var results = await _algoliaService.SearchSongsAsync(query, currentPage, pageSize);

            foreach(var result in results)
            {
                result.Duration_ms = FormatDuration(result.Duration_ms);
            }

            return new JsonResult(results);
        }

        public async Task<JsonResult> OnGetTrendingSongsAsync(int pageSize)
        {
            var results = await _algoliaService.GetTrendingSongsAsync(pageSize);
            foreach (var song in results)
            {
                song.Duration_ms = FormatDuration(song.Duration_ms);
            }
            return new JsonResult(results);
        }

        private string FormatDuration(string durationMs)
        {
            TimeSpan duration = TimeSpan.FromMilliseconds(Convert.ToDouble(durationMs));
            return $"{duration.Minutes}:{duration.Seconds:D2}";
        }
    }
}
