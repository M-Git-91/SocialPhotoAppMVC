using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Services.AlbumService;
using SocialPhotoAppMVC.ViewModels;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IHttpContextAccessor _httpContext;

        public AlbumController(IAlbumService albumService, IHttpContextAccessor httpContext)
        {
            _albumService = albumService;
            _httpContext = httpContext;
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> AlbumDetail(int id)
        {
            var findAlbum = await _albumService.GetAlbumDetail(id);
            
            if (findAlbum.Success == false)
            {
                var errorMessage = findAlbum.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(findAlbum);
        }

        [HttpGet]
        public async Task<IActionResult> UserAlbums(int? page)
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userPhotos = await _albumService.GetUserAlbums(currentUserId, page);

            if (userPhotos.Success == false)
            {
                var errorMessage = userPhotos.Message;
                return View("ErrorPage", errorMessage);
            }
            return View(userPhotos);
        }

        [HttpGet, Authorize]
        public IActionResult CreateAlbum()
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CreateAlbumVM newCreateAlbumVM = new CreateAlbumVM { UserId = currentUserId };
            return View(newCreateAlbumVM);
        }


        [HttpPost, ActionName("CreateAlbum"), Authorize]
        public async Task<IActionResult> CreateAlbum(CreateAlbumVM albumVM)
        {
            if (ModelState.IsValid)
            {
                var uploadPhoto = await _albumService.CreateAlbum(albumVM);
                if (uploadPhoto.Success == false)
                {
                    TempData["Error"] = $"{uploadPhoto.Message}";
                    return View(albumVM);
                }
                return RedirectToAction("RecentAlbums");
            }
            else
            {
                ModelState.AddModelError("", "Album was not created.");
                return View(albumVM);
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> DeleteAlbum(int id) 
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var albumToRemove = await _albumService.GetAlbumByIdAsync(id);
            if (albumToRemove.Success == false)
            {
                var errorMessage = albumToRemove.Message;
                return View("ErrorPage", errorMessage);
            }

            DeleteAlbumVM deleteAlbumVM = new DeleteAlbumVM
            {
                UserId = currentUserId,
                AlbumOwnerId = albumToRemove.Data.User.Id,
                AlbumId = albumToRemove.Data.Id,
                Title = albumToRemove.Data.Title,
                Description = albumToRemove.Data.Description
            };

            return View(deleteAlbumVM);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> DeleteAlbum(DeleteAlbumVM deleteAlbumVM) 
        {
            if (deleteAlbumVM.UserId == deleteAlbumVM.AlbumOwnerId)
            {
                var result = await _albumService.DeleteAlbum(deleteAlbumVM.AlbumId);
                if (result.Success == true)
                {
                    return RedirectToAction("UserAlbums");
                }
                else
                {
                    var errorMessage = result.Message;
                    return View("ErrorPage", errorMessage);
                }
            }
            else
            {
                var errorMessage = "You are not authorized to delete this album.";
                return View("ErrorPage", errorMessage);
            }
        }
        
    }
}
