using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.SearchService
{
    public interface ISearchService
    {
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(SearchPhotoVM searchInput, int? page);
        Task<ServiceResponse<IPagedList<Album>>> SearchAlbums(SearchAlbumVM searchInput, int? page);
        Task<ServiceResponse<IPagedList<AppUser>>> SearchUsers(SearchUserVM searchInput, int? page);
    }
}
