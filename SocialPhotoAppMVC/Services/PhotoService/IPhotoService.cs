using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<ServiceResponse<IPagedList<Photo>>> GetAllPhotos(int? page);
        Task<ServiceResponse<Photo>> GetPhotoByIdAsync(int id);
        Task<ServiceResponse<IPagedList<Photo>>> GetFeaturedPhotos(int? page);
        Task<ServiceResponse<IPagedList<Photo>>> GetUserPhotos(string currentUserId, int? page, int resultsPerPage);
        Task<ServiceResponse<Photo>> GetPhotoDetail(int id);
        Task<ServiceResponse<AddPhotoToAlbumVM>> AddPhotoToAlbumGET(int id);
        Task<ServiceResponse<Album>> AddPhotoToAlbumPOST(AddPhotoToAlbumVM photoToAlbumVM);
        Task<ServiceResponse<AddPhotoToAlbumVM>> RemovePhotoFromAlbumGET(int id);
        Task<ServiceResponse<Album>> RemovePhotoFromAlbumPOST(AddPhotoToAlbumVM photoToAlbumVM);
        Task<ServiceResponse<bool>> UploadPhoto(UploadPhotoVM photoVM);
        Task<ServiceResponse<bool>> DeletePhoto(int id);
        Task<ServiceResponse<bool>> EditPhoto(EditPhotoVM editPhotoVM);
    }
}
