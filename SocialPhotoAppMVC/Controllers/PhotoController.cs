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
                return View("ErrorPage");
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
            else
            {
                return View("ErrorPage");
            }            
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> EditPhoto(int id)
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Photo photo = await _photoService.GetPhotoByIdAsync(id);
            if (photo == null)
            {
                return View("ErrorPage");
            }

            var photoVM = new EditPhotoVM
            {
                PhotoId = photo.Id,
                Title = photo.Title,
                Description = photo.Description,
                ImageUrl = photo.ImageUrl,
                Category = photo.Category,
                CurrentUserId = currentUserId,
                AuthorId = photo.User.Id,
            };
            return View(photoVM);
        }


        [HttpPost, ActionName("EditPhoto"), Authorize]
        public async Task<IActionResult> EditPhotoPost(EditPhotoVM editPhotoVM)
        {
            if (editPhotoVM.CurrentUserId == editPhotoVM.AuthorId)
            {
                var oldPhoto = await _photoService.GetPhotoByIdAsync(editPhotoVM.PhotoId);
                if (oldPhoto == null)
                {
                    TempData["Error"] = "Photo was not found.";
                    return View("ErrorPage");
                }

                var newPhotoUpload = await _cloudService.AddPhotoAsync(editPhotoVM.NewImage);
                if (newPhotoUpload.Error != null)
                {
                    TempData["Error"] = "New photo was not uploaded";
                    return View(editPhotoVM);
                }

                if (!string.IsNullOrEmpty(oldPhoto.ImageUrl))
                {
                    await _cloudService.DeletePhotoAsync(oldPhoto.ImageUrl);
                }

                var newPhoto = new Photo
                {
                    Id = editPhotoVM.PhotoId,
                    Title = editPhotoVM.Title,
                    Description = editPhotoVM.Description,
                    ImageUrl = newPhotoUpload.Uri.ToString(),
                    Category = editPhotoVM.Category,
                    User = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == editPhotoVM.CurrentUserId)

                };

                _context.Photos.Update(newPhoto);
                _context.SaveChanges();
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
