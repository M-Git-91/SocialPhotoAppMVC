
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.ViewModels;
using System.Security.Claims;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudService _cloudService;
        private readonly IHttpContextAccessor _httpContext;

        public PhotoService(ApplicationDbContext context, ICloudService cloudService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _cloudService = cloudService;
            _httpContext = httpContext;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> GetAllPhotos(int? page)
        {
            var allPhotos = await _context.Photos.OrderByDescending(p => p.DateCreated).ToListAsync();
            var response = new ServiceResponse<IPagedList<Photo>>();

            if (allPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos were found.";
                return response;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var pagedList = await allPhotos.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<Photo>> GetPhotoByIdAsync(int id)
        {
            var photo = await _context.Photos.AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            var response = new ServiceResponse<Photo> { Data = photo };

            if (photo == null)
            {
                response.Success = false;
                response.Message = "No photo found.";
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> GetFeaturedPhotos(int? page)
        {
            var featuredPhotos = await _context.Photos.Where(p => p.IsFeatured == true).ToListAsync();
            var response = new ServiceResponse<IPagedList<Photo>>();

            if (featuredPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No featured photos were found.";
                return response;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var pagedList = await featuredPhotos.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<Photo>> GetPhotoDetail(int id)
        {
            var photo = await _context.Photos.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            var response = new ServiceResponse<Photo> { Data = photo };

            if (response.Data == null) 
            {
                response.Success = false;
                response.Message = "Photo not found.";
                return response;
            }

            return response;
        }


        public async Task<ServiceResponse<IPagedList<Photo>>> GetUserPhotos(string currentUserId, int? page)
        {
            var userPhotos = await _context.Photos.Where(p => p.User.Id == currentUserId).ToListAsync();
            var response = new ServiceResponse<IPagedList<Photo>>();

            if (userPhotos.Count == 0) 
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var pagedList = await userPhotos.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<AddPhotoToAlbumVM>> AddPhotoToAlbumGET(int id)
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var photo = await GetPhotoByIdAsync(id);
            var userAlbums = await _context.Albums.Where(p => p.User.Id == currentUserId).ToListAsync();
            var addPhotoToAlbumVM = new AddPhotoToAlbumVM { Photo = photo.Data, UserAlbums = userAlbums };

            var response = new ServiceResponse<AddPhotoToAlbumVM> { Data = addPhotoToAlbumVM};
            return response;
        }

        public async Task<ServiceResponse<Album>> AddPhotoToAlbumPOST(AddPhotoToAlbumVM photoToAlbumVM)
        {
            var response = new ServiceResponse<Album>();
            var album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == photoToAlbumVM.SelectedAlbumId);
            if (album == null) 
            {
                response.Success = false;
                response.Message = "Album not found.";
                return response;
            }
            
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == photoToAlbumVM.Photo.Id);
            if (photo == null)
            {
                response.Success = false;
                response.Message = "Photo not found.";
                return response;
            }

            album.Photos.Add(photo);
 
            _context.Albums.Update(album);

            var saveCount = Save();
            if ( saveCount == false)
            {
                response.Success = false;
                response.Message = "Photo was not added to the selected album.";
                return response;
            }

            response.Data = album;

            return response;
        }

        public async Task<ServiceResponse<bool>> UploadPhoto(UploadPhotoVM photoVM)
        {
            var response = new ServiceResponse<bool>();
            
            var uploadPhoto = await _cloudService.AddPhotoAsync(photoVM.Image);
            
            if (uploadPhoto.Error != null)
            {
                return NegativeResponse("Photo upload unsuccesful.");
            }

            var photo = new Photo
            {
                Title = photoVM.Title,
                Description = photoVM.Description,
                ImageUrl = uploadPhoto.Url.ToString(),
                Category = photoVM.Category,
                User = await _context.Users.FirstOrDefaultAsync(u => u.Id == photoVM.UserId),
            };

            if (photo.User == null)
            {
                return NegativeResponse("User not found.");
            }

            _context.Photos.Add(photo);
            var saveResult = Save();

            if (saveResult == false) {
                return NegativeResponse("Upload unsuccessful.");
            }
           
            return response;
        }

        public async Task<ServiceResponse<bool>> DeletePhotoAsync(int photoId)
        {
            var response = new ServiceResponse<bool>();
            var photo = await GetPhotoByIdAsync(photoId);
            
            if (photo.Data == null)
            {
                return NegativeResponse("Photo not found.");
            }

            if (!string.IsNullOrEmpty(photo.Data.ImageUrl))
            {
                await _cloudService.DeletePhotoAsync(photo.Data.ImageUrl);
            }

            _context.Photos.Remove(photo.Data);
            var saveResult = Save();

            if (saveResult == false) 
            {
                return NegativeResponse("Photo was not removed");
            }

            response.Data = false;
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<bool>> EditPhotoAsync(EditPhotoVM editPhotoVM)
        {
            var response = new ServiceResponse<bool>();
            var oldPhoto = await GetPhotoByIdAsync(editPhotoVM.PhotoId);
            if (oldPhoto == null)
            {
                return NegativeResponse("Photo not found.");
            }

            var newPhotoUpload = await _cloudService.AddPhotoAsync(editPhotoVM.NewImage);
            if (newPhotoUpload.Error != null)
            {
                return NegativeResponse("Photo upload unsuccessful.");
            }

            if (!string.IsNullOrEmpty(oldPhoto.Data.ImageUrl))
            {
                await _cloudService.DeletePhotoAsync(oldPhoto.Data.ImageUrl);
            }

            var newPhoto = new Photo
            {
                Id = editPhotoVM.PhotoId,
                Title = editPhotoVM.Title,
                Description = editPhotoVM.Description,
                ImageUrl = newPhotoUpload.Url.ToString(),
                Category = editPhotoVM.Category,
                User = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == editPhotoVM.CurrentUserId)

            };

            _context.Photos.Update(newPhoto);
            var saveResult = Save();
            
            if (saveResult == false)
            {
                return NegativeResponse("Photo was not removed.");
            }

            response.Data = true;
            response.Success = true;

            return response;

        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        private ServiceResponse<bool> NegativeResponse(string errorMessage) 
        {
            var response = new ServiceResponse<bool>();
            response.Data = false;
            response.Success = false;
            response.Message = $"{errorMessage}";
            return response;
        }
    }
}
