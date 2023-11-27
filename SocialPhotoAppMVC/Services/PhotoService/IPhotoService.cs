using SocialPhotoAppMVC.ViewModels;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<ServiceResponse<IEnumerable<Photo>>> GetAllPhotos();
        Task<ServiceResponse<Photo>> GetPhotoByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<Photo>>> GetFeaturedPhotos();
        Task<ServiceResponse<Photo>> GetPhotoDetail(int id);
        Task<ServiceResponse<bool>> UploadPhoto(UploadPhotoVM photoVM);
        Task<ServiceResponse<bool>> DeletePhotoAsync(int id);
        Task<ServiceResponse<bool>> EditPhotoAsync(EditPhotoVM editPhotoVM);
    }
}
