using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.Services;
using SocialPhotoAppMVC.Services.AlbumService;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.ViewModels;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using X.PagedList;

namespace SocialPhotoAppMVC.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApplicationDbContext _context;
        private readonly IAlbumService _albumService;

        public PhotoController(IPhotoService photoService, IHttpContextAccessor httpContext, ApplicationDbContext context)
        {
            _photoService = photoService;
            _httpContext = httpContext;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var recentPhotos = await _photoService.GetAllPhotos(page);

            if (recentPhotos.Success == false)
            {
                var errorMessage = recentPhotos.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(recentPhotos);
        }

        [HttpGet]
        public async Task<IActionResult> FeaturedPhotos(int? page)
        {
            var featuredPhotos = await _photoService.GetFeaturedPhotos(page);

            if (featuredPhotos.Success == false)
            {
                var errorMessage = featuredPhotos.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(featuredPhotos);
        }

        [HttpGet]
        public async Task<IActionResult> PhotoDetail(int id)
        {
            var findPhoto = await _photoService.GetPhotoDetail(id);
            if (findPhoto.Success == false)
            {
                var errorMessage = findPhoto.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(findPhoto);
        }

        [HttpGet]
        public async Task<IActionResult> UserPhotos(int? page)
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userPhotos = await _photoService.GetUserPhotos(currentUserId, page);

            if (userPhotos.Success == false)
            {
                var errorMessage = userPhotos.Message;
                return View("ErrorPage", errorMessage);
            }
            return View(userPhotos);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> AddPhotoToAlbum(int id)
        {
            var response = await _photoService.AddPhotoToAlbumGET(id);
            return View(response.Data);
        }

        [HttpPost, ActionName("AddPhotoToAlbum"), Authorize]
        public async Task<IActionResult> AddPhotoToAlbum(AddPhotoToAlbumVM photoToAlbumVM) 
        {         
            var addPhotoToAlbum = await _photoService.AddPhotoToAlbumPOST(photoToAlbumVM);
            if (addPhotoToAlbum.Success == false)
            {
                TempData["Error"] = $"{addPhotoToAlbum.Message}";
                return View(photoToAlbumVM);
            }
            return RedirectToAction("UserPhotos");
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
                if (uploadPhoto.Success == false)
                {
                    TempData["Error"] = $"{uploadPhoto.Message}";
                    return View(photoVM);
                }
                return RedirectToAction("UserPhotos");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload unsuccessful.");
                return View(photoVM);
            }           
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var photoToRemove = await _photoService.GetPhotoByIdAsync(id);
            if (photoToRemove.Success == false)
            {
                var errorMessage = photoToRemove.Message;
                return View("ErrorPage", errorMessage);
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
                    return RedirectToAction("UserPhotos");
                }
                else
                {
                    var errorMessage = result.Message;
                    return View("ErrorPage", errorMessage);
                }
            }
            else
            {
                var errorMessage = "You are not authorized to delete this photo.";
                return View("ErrorPage", errorMessage);
            }            
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> EditPhoto(int id)
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var photoToEdit = await _photoService.GetPhotoByIdAsync(id);
            if (photoToEdit.Data == null)
            {
                var errorMessage = photoToEdit.Message;
                return View("ErrorPage", errorMessage);
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
                    var errorMessage = result.Message;
                    return View("ErrorPage", errorMessage);
                }
                return RedirectToAction("UserPhotos");
  
            }
            else
            {
                var errorMessage = "You are not authorized to edit this photo.";
                return View("ErrorPage", errorMessage);
            }
        }
    }
}
