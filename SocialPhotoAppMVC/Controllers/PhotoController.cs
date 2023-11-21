using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SocialPhotoAppMVC.Services;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.ViewModels;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContext;

        public PhotoController(IPhotoService photoService, IHttpContextAccessor httpContext)
        {
            _photoService = photoService;
            _httpContext = httpContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var recentPhotos = await _photoService.GetAllPhotos();

            return View(recentPhotos);
        }

        [HttpGet]
        public async Task<IActionResult> FeaturedPhotos()
        {
            var featuredPhotos = await _photoService.GetFeaturedPhotos();

            return View(featuredPhotos);
        }
        
        [HttpGet]
        public async Task<IActionResult> PhotoDetail(int id) 
        {
            var findPhoto = await _photoService.GetPhotoDetail(id);
            return View(findPhoto);
        }

        [HttpGet]
        public IActionResult UploadPhoto()
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var createUploadPhotoVM = new UploadPhotoVM { UserId = currentUserId };
            return View(createUploadPhotoVM);
        }


        [HttpPost, ActionName("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(UploadPhotoVM photoVM)
        {
            if (ModelState.IsValid)
            {
                bool uploadPhoto = await _photoService.UploadPhoto(photoVM);
                if (uploadPhoto == true)
                {
                    return RedirectToAction("Index");
                }
                return View(photoVM);
            }
            else
            {
                ModelState.AddModelError("", "Photo upload unsuccessful.");
            }
            return View(photoVM);
        }
    }

}
