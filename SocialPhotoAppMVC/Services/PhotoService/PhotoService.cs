
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.ViewModels;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudService _cloudService;

        public PhotoService(ApplicationDbContext context, ICloudService cloudService)
        {
            _context = context;
            _cloudService = cloudService;
        }

        public async Task<ServiceResponse<IEnumerable<Photo>>> GetAllPhotos()
        {
            var photos = await _context.Photos.OrderByDescending(p => p.DateCreated).ToListAsync();
            var response = new ServiceResponse<IEnumerable<Photo>> 
            {
                Data = photos,
            };

            return response;
        }

        public async Task<ServiceResponse<Photo>> GetPhotoByIdAsync(int id)
        {
            var photo = await _context.Photos.AsNoTracking().Include(p => p.User).FirstAsync(p => p.Id == id);
            var response = new ServiceResponse<Photo> { Data = photo };
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<Photo>>> GetFeaturedPhotos()
        {
            var photos = await _context.Photos.Where(p => p.IsFeatured == true).ToListAsync();
            var response = new ServiceResponse<IEnumerable<Photo>> { Data = photos};
            return response;
        }

        public async Task<ServiceResponse<Photo>> GetPhotoDetail(int id)
        {
            var photo = await _context.Photos.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            var response = new ServiceResponse<Photo> { Data = photo };

            if (photo == null) 
            {
                response.Success = false;
                response.Message = "Photo not found.";
                return response;
            }

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
