using SocialPhotoAppMVC.ViewModels;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetAllPhotos();
        Task<Photo> GetPhotoByIdAsync(int id);
        Task<IEnumerable<Photo>> GetFeaturedPhotos();
        Task<Photo> GetPhotoDetail(int id);
        Task<bool> UploadPhoto(UploadPhotoVM photoVM);
        Task<bool> DeletePhotoAsync(int id);
    }
}
