using Microsoft.AspNetCore.Authorization;
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
            UploadPhotoVM createUploadPhotoVM = new UploadPhotoVM { UserId = currentUserId };
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

        [HttpGet, Authorize]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Photo photo = await _photoService.GetPhotoByIdAsync(id);
            if (photo == null)
            {
                return View("Error");
            }

            DeletePhotoVM createDeletePhotoVM = new DeletePhotoVM { 
                UserId = currentUserId, 
                PhotoOwnerId = photo.User.Id,
                PhotoId = id,
                Title = photo.Title,
                Description = photo.Description,
                Category = photo.Category,
            };

            return View(createDeletePhotoVM);
        }

        [HttpPost, ActionName("DeletePhoto"), Authorize]
        public async Task<IActionResult> DeletePhotoPost(DeletePhotoVM deletePhotoVM)
        {
            if (deletePhotoVM.UserId == deletePhotoVM.PhotoOwnerId)
            {
                bool result = await _photoService.DeletePhotoAsync(deletePhotoVM.PhotoId);
                if (result == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ErrorPage");
                }
            }
            return View("ErrorPage");
        }
        /*
        [HttpPost, ActionName("DeletePhoto"), Authorize]
        public async Task<IActionResult> DeletePhotoPost(int photoId)
        {
            var result = await _photoService.DeletePhotoAsync(photoId);

            if (result == true)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }*/
    }

}
