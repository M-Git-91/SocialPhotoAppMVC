using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.Services.SearchService;
using SocialPhotoAppMVC.ViewModels;

namespace SocialPhotoAppMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IActionResult SearchPhotoIndex() 
        {
            return View();
        }

        public async Task<IActionResult> SearchPhotos(SearchPhotoVM searchInput, int? page)
        {
            var response = await _searchService.SearchPhotos(searchInput, page);

            if (response.Success == false)
            {
                var errorMessage = response.Message;
                return View("ErrorPage", errorMessage);
            }

            return View("PhotoSearchResult", response);
        }

        public IActionResult SearchAlbumIndex()
        {
            return View();
        }

        public async Task<IActionResult> SearchAlbums(SearchAlbumVM searchInput, int? page)
        {
            var response = await _searchService.SearchAlbums(searchInput, page);

            if (response.Success == false)
            {
                var errorMessage = response.Message;
                return View("ErrorPage", errorMessage);
            }

            return View("AlbumsSearchResult", response);
        }
    }
}