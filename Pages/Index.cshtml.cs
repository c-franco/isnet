using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using isnet.Models;
using isnet.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace isnet.Pages
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
