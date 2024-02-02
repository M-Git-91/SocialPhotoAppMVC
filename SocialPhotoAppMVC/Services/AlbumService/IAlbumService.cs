using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.AlbumService
{
    public interface IAlbumService
    {
        Task<ServiceResponse<IPagedList<Album>>> GetAllAlbums(int? page);
        Task<ServiceResponse<Album>> GetAlbumByIdAsync(int id);
        Task<ServiceResponse<IPagedList<Album>>> GetUserAlbums(string currentUserId, int? page, int albumsPerPage);
        Task<ServiceResponse<Album>> GetAlbumDetail(int id);
        Task<ServiceResponse<bool>> CreateAlbum(CreateAlbumVM albumVM);
        Task<ServiceResponse<bool>> DeleteAlbum(int albumId); 
        Task<ServiceResponse<bool>> EditAlbum(EditAlbumVM editAlbumVM);
        Task<IPagedList<Album>> PaginateListOfAlbums(int? page, int resultsPerPage, List<Album> allAlbums);
    }
}
