using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.ViewModels;
using System.Linq;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(SearchPhotoVM searchInput, int? page)
        {
            var foundPhotos = await _context.Photos
                .Include(p => p.Title)
                .Include(p => p.Description)
                .Where(p => p.Title
                .Contains(searchInput.Title) || p.Description.Contains(searchInput.Description))
                .ToListAsync();

            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            var pagedList = await PaginateListOfPhotos(foundPhotos, page, 6);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByTitle(SearchPhotoVM searchInput, int? page)
        {
            var foundPhotos = await _context.Photos.Where(p => p.Title.Contains(searchInput.Title)).ToListAsync();
            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            var pagedList = await PaginateListOfPhotos(foundPhotos, page, 6);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByDescription(SearchPhotoVM searchInput, int? page)
        {
            var foundPhotos = await _context.Photos.Where(p => p.Description.Contains(searchInput.Description)).ToListAsync();
            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            var pagedList = await PaginateListOfPhotos(foundPhotos, page, 6);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByCategory(Category category, int? page)
        {
            var foundPhotos = await _context.Photos.Where(p => p.Category == category).ToListAsync();
            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            var pagedList = await PaginateListOfPhotos(foundPhotos, page, 6);
            response.Data = pagedList;

            return response;
        }

        private async Task<IPagedList<Photo>> PaginateListOfPhotos(List<Photo> listOfPhotos, int? page, int pageSize) 
        {
            var pageNumber = (page ?? 1);
            var pagedList = await listOfPhotos.ToPagedListAsync(pageNumber, pageSize);
            return pagedList;
        }
    }
}
