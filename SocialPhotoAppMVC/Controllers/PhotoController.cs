using Azure.Core;
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
        private readonly ICloudService _cloudService;
        private readonly ApplicationDbContext _context;

        public PhotoController(IPhotoService photoService, IHttpContextAccessor httpContext, ICloudService cloudService, ApplicationDbContext context)
        {
            _photoService = photoService;
            _httpContext = httpContext;
            _cloudService = cloudService;
            _context = context;
        }

        [HttpGet, ActionName("Index")]
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

        [HttpGet, Authorize]
        public IActionResult UploadPhoto()
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UploadPhotoVM createUploadPhotoVM = new UploadPhotoVM { UserId = currentUserId };
            return View(createUploadPhotoVM);
        }


        [HttpPost, ActionName("UploadPhoto"), Authorize]
        public async Task<IActionResult> UploadPhoto(UploadPhotoVM photoVM)
        {
            if (ModelState.IsValid)
            {
                var uploadPhoto = await _photoService.UploadPhoto(photoVM);
                if (uploadPhoto.Data == false)
                {
                    TempData["Error"] = $"{uploadPhoto.Message}";
                    return View(photoVM);
                }
                return RedirectToAction("Index");
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
            var photoToRemove = await _photoService.GetPhotoByIdAsync(id);
            if (photoToRemove.Data == null)
            {
                TempData["Error"] = $"{photoToRemove.Message}";
                return View("ErrorPage");
            }

            DeletePhotoVM createDeletePhotoVM = new DeletePhotoVM { 
                UserId = currentUserId, 
                PhotoOwnerId = photoToRemove.Data.User.Id,
                PhotoId = id,
                Title = photoToRemove.Data.Title,
                Description = photoToRemove.Data.Description,
                Category = photoToRemove.Data.Category,
            };

            return View(createDeletePhotoVM);
        }

        [HttpPost, ActionName("DeletePhoto"), Authorize]
        public async Task<IActionResult> DeletePhotoPost(DeletePhotoVM deletePhotoVM)
        {
            if (deletePhotoVM.UserId == deletePhotoVM.PhotoOwnerId)
            {
                var result = await _photoService.DeletePhotoAsync(deletePhotoVM.PhotoId);
                if (result.Success == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = $"{result.Message}";
                    return View("ErrorPage");
                }
            }
            else
            {
                return View("ErrorPage");
            }            
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> EditPhoto(int id)
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var photoToEdit = await _photoService.GetPhotoByIdAsync(id);
            if (photoToEdit.Data == null)
            {
                TempData["Error"] = $"{photoToEdit.Message}";
                return View("ErrorPage");
            }

            var photoVM = new EditPhotoVM
            {
                PhotoId = photoToEdit.Data.Id,
                Title = photoToEdit.Data.Title,
                Description = photoToEdit.Data.Description,
                ImageUrl = photoToEdit.Data.ImageUrl,
                Category = photoToEdit.Data.Category,
                CurrentUserId = currentUserId,
                AuthorId = photoToEdit.Data.User.Id,
            };
            return View(photoVM);
        }


        [HttpPost, ActionName("EditPhoto"), Authorize]
        public async Task<IActionResult> EditPhotoPost(EditPhotoVM editPhotoVM)
        {
            if (editPhotoVM.CurrentUserId == editPhotoVM.AuthorId)
            {

                var result = await _photoService.EditPhotoAsync(editPhotoVM);
                if (result.Success == false) 
                {
                    return View("ErrorPage");
                }
                return RedirectToAction("Index");
  
            }
            else
            {
                TempData["Error"] = "You are not authorized to edit this photo.";
                return View("ErrorPage");
            }
        }
    }
}
