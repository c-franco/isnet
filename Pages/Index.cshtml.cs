using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<JsonResult> OnGetSearchSongsAsync(string query)
        {
            var results = await _algoliaService.SearchSongsAsync(query);
            return new JsonResult(results);
        }
    }
}
