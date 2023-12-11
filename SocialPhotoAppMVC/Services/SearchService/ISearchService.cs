using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Enums;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.SearchService
{
    public interface ISearchService
    {
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(string searchText, int? page);
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByTitle(string searchText, int? page);
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByDescription(string searchText, int? page);
        Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByCategory(Category category, int? page);
    }
}
