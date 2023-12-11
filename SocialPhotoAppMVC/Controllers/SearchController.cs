using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.Services.SearchService;

namespace SocialPhotoAppMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<IActionResult> SearchPhotos(string searchInput, int? page)
        {
            var response = await _searchService.SearchPhotos(searchInput, page);

            if (response.Success == false)
            {
                var errorMessage = response.Message;
                return View("ErrorPage", errorMessage);
            }

            return View();
        }

        public async Task<IActionResult> SearchPhotosByTitle(string searchInput, int? page) 
        {
            var response = await _searchService.SearchPhotosByTitle(searchInput, page);

            if (response.Success == false)
            {
                var errorMessage = response.Message;
                return View("ErrorPage", errorMessage);
            }

            return View();
        }

        public async Task<IActionResult> SearchPhotosByDescription(string searchInput, int? page)
        {
            var response = await _searchService.SearchPhotosByDescription(searchInput, page);

            if (response.Success == false)
            {
                var errorMessage = response.Message;
                return View("ErrorPage", errorMessage);
            }

            return View();
        }

        public async Task<IActionResult> SearchPhotosByCategory(Category category, int? page)
        {
            var response = await _searchService.SearchPhotosByCategory(category, page);

            if (response.Success == false)
            {
                var errorMessage = response.Message;
                return View("ErrorPage", errorMessage);
            }

            return View();
        }
    }
}
