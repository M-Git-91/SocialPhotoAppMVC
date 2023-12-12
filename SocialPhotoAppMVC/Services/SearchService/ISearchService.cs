using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.SearchService
{
    public interface ISearchService
    {
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(SearchPhotoVM searchInput, int? page);
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByTitle(SearchPhotoVM searchInput, int? page);
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByDescription(SearchPhotoVM searchInput, int? page);
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByCategory(Category category, int? page);
    }
}
