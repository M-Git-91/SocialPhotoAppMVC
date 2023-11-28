using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<ServiceResponse<IPagedList<Photo>>> GetAllPhotos(int? page);
        Task<ServiceResponse<Photo>> GetPhotoByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<Photo>>> GetFeaturedPhotos();
        Task<ServiceResponse<IEnumerable<Photo>>> GetUserPhotos(string currentUserId);
        Task<ServiceResponse<Photo>> GetPhotoDetail(int id);
        Task<ServiceResponse<bool>> UploadPhoto(UploadPhotoVM photoVM);
        Task<ServiceResponse<bool>> DeletePhotoAsync(int id);
        Task<ServiceResponse<bool>> EditPhotoAsync(EditPhotoVM editPhotoVM);
    }
}
