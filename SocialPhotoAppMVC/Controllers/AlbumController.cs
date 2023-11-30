using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Services.AlbumService;

namespace SocialPhotoAppMVC.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        public async Task<IActionResult> RecentAlbums(int? page)
        {
            var recentAlbums = await _albumService.GetAllAlbums(page);

            if (recentAlbums.Success == false)
            {
                var errorMessage = recentAlbums.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(recentAlbums);
        }
    }
}
