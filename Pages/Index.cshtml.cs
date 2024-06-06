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

        public JsonResult OnGetTrendingSongsAsync(int pageSize)
        {
            List<Song> trendingSongs = GetTrendingSongs();

            return new JsonResult(trendingSongs.Take(pageSize));
        }

        public List<Song> GetTrendingSongs()
        {

            List<Song> trendingSongs = new List<Song>
            {
                new Song
                {
                    Track_Id = "5XeFesFbtLpXzIVDNQP22n",
                    Artist_Name = "Arctic Monkeys",
                    Track_Name = "I Wanna Be Yours",
                    Popularity = 91,
                    Year = "2013",
                    Genre = "garage",
                    Duration_ms = FormatDuration("183956")
                },
                new Song
                {
                    Track_Id = "1Qrg8KqiBpW07V7PNxwwwL",
                    Artist_Name = "SZA",
                    Track_Name = "Kill Bill",
                    Popularity = 94,
                    Year = "2022",
                    Genre = "pop",
                    Duration_ms = FormatDuration("153947")
                },
                new Song
                {
                    Track_Id = "3w3y8KPTfNeOKPiqUTakBh",
                    Artist_Name = "Bruno Mars",
                    Track_Name = "Locked out of Heaven",
                    Popularity = 85,
                    Year = "2012",
                    Genre = "dance",
                    Duration_ms = FormatDuration("233478")
                },
                new Song
                {
                    Track_Id = "4pGqFOfzvfe6avb9kbZicC",
                    Artist_Name = "The Neighbourhood",
                    Track_Name = "Sweater Weather",
                    Popularity = 60,
                    Year = "2012",
                    Genre = "alt-rock",
                    Duration_ms = FormatDuration("240040")
                },
                new Song
                {
                    Track_Id = "4LRPiXqCikLlN15c3yImP7",
                    Artist_Name = "Harry Styles",
                    Track_Name = "As It Was",
                    Popularity = 90,
                    Year = "2022",
                    Genre = "pop",
                    Duration_ms = FormatDuration("167303")
                },
                new Song
                {
                    Track_Id = "1BxfuPKGuaTgP7aM0Bbdwr",
                    Artist_Name = "Taylor Swift",
                    Track_Name = "Cruel Summer",
                    Popularity = 83,
                    Year = "2019",
                    Genre = "pop",
                    Duration_ms = FormatDuration("178427")
                },
                new Song
                {
                    Track_Id = "0mflMxspEfB0VbI1kyLiAv",
                    Artist_Name = "Stick Season",
                    Track_Name = "Noah Kahan",
                    Popularity = 75,
                    Year = "2022",
                    Genre = "pop",
                    Duration_ms = FormatDuration("182347")
                },
                new Song
                {
                    Track_Id = "7KA4W4McWYRpgf0fWsJZWB",
                    Artist_Name = "Tyler, The Creator",
                    Track_Name = "See You Again (feat. Kali Uchis)",
                    Popularity = 83,
                    Year = "2017",
                    Genre = "hip-hop",
                    Duration_ms = FormatDuration("180387")
                },
                new Song
                {
                    Track_Id = "1uMHCAyGmHqyygoNRuo7MV",
                    Artist_Name = "bbno$",
                    Track_Name = "edamame (feat. Rich Brian)",
                    Popularity = 72,
                    Year = "2021",
                    Genre = "hip-hop",
                    Duration_ms = FormatDuration("133707")
                }
                ,
                new Song
                {
                    Track_Id = "561jH07mF1jHuk7KlaeF0s",
                    Artist_Name = "Eminem",
                    Track_Name = "Mockingbird",
                    Popularity = 90,
                    Year = "2004",
                    Genre = "hip-hop",
                    Duration_ms = FormatDuration("250760")
                }
            };

            return trendingSongs;
        }

        private string FormatDuration(string durationMs)
        {
            int duration = Convert.ToInt32(durationMs);
            var totalSeconds = duration / 1000;
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;
            return $"{minutes}:{seconds:D2}";
        }
    }
}
