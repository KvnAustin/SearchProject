using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchProject.Core.Interfaces;
using SearchProject.Models;
using SearchProject.UI.ViewModels;
using System.Text.RegularExpressions;
using System.Web;

namespace SearchProject.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearch _searchService;

        public SearchController(ISearch searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSearches()
        {
            try
            {
                var searches = await _searchService.GetAll();

                return Ok(MapSearchesToViewModel(searches));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSearch(Guid id)
        {
            try
            {
                var search = await _searchService.GetById(id);
                if (search == null)
                    return NotFound();

                return Ok(MapSearchToViewModel(search));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(SearchViewModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var endpoint = string.Format("https://www.google.com/search?num=100&q={0}", HttpUtility.UrlEncode(model.Keywords));
                var response = await GetEndpointResponse(endpoint);
                var indexes = GetIndexesFromResponse(response, model.Url);

                var search = MapViewModelToSearch(model, indexes);
                search = await _searchService.Save(search);

                return CreatedAtAction(nameof(GetSearch), new { id = search.Id }, search);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }
        }

        #region [ Helper Methods ]
        private IEnumerable<SearchViewModel> MapSearchesToViewModel(IEnumerable<Search> searches)
        {
            return (searches ?? Enumerable.Empty<Search>())
                .Select(search => MapSearchToViewModel(search));
        }

        private SearchViewModel MapSearchToViewModel(Search search)
        {
            if (search == null)
                return null;

            return new SearchViewModel
            {
                Url = search.Url,
                Keywords = search.Keywords,
                CreatedOnUtc = search.CreatedOnUtc,
                Indexes = search.Results?
                    .Select(result => result.Index)
                    .ToList()

            };
        }

        private Search MapViewModelToSearch(SearchViewModel model, IEnumerable<int> indexes)
        {
            return new Search
            {
                Id = model.Id,
                Url = model.Url,
                Keywords = model.Keywords,
                Results = indexes
                    .Select(index => new SearchResult { Index = index })
                    .ToList(),
                CreatedOnUtc = model.CreatedOnUtc
            };
        }

        private async Task<string> GetEndpointResponse(string endpoint)
        {
            var client = new HttpClient();

            var response = await client.GetStringAsync(endpoint);

            return response;
        }

        private List<int> GetIndexesFromResponse(string response, string url)
        {
            var indexes = new List<int>();

            if (!string.IsNullOrWhiteSpace(response))
            {
                // get anchor tags that have an 'href' that begins with '/url?q='
                var pattern = @"<a\s+[^>]*?href\s*=\s*[""']\/url\?q=[^""']*[""'][^>]*?>.*?<\/a>";

                var index = 0;
                var regex = new Regex(pattern);
                var matches = regex.Matches(response);
                foreach (var match in matches)
                {
                    var matchStr = match?.ToString();
                    if (matchStr?.Contains(url) == true)
                        indexes.Add(index + 1);

                    index++;
                }
            }

            return indexes;
        }
        #endregion
    }
}
