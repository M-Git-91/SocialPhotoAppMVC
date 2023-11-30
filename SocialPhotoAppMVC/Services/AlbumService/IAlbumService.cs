using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.AlbumService
{
    public interface IAlbumService
    {
        Task<ServiceResponse<IPagedList<Album>>> GetAllAlbums(int? page);
        Task<ServiceResponse<Album>> GetAlbumByIdAsync(int id);
        Task<ServiceResponse<IPagedList<Album>>> GetUserAlbums(string currentUserId, int? page);
        Task<ServiceResponse<Album>> GetAlbumDetail(int id);
        Task<ServiceResponse<bool>> CreateAlbum(CreateAlbumVM albumVM);
        Task<ServiceResponse<bool>> DeleteAlbumAsync(int id);
        Task<ServiceResponse<bool>> EditAlbumAsync(EditAlbumVM editAlbumVM);
    }
}
