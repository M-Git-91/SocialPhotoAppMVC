using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Enums;
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

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(string searchText, int? page)
        {
            var foundPhotos = await _context.Photos.Where(p => p.Title.Contains(searchText) || p.Description.Contains(searchText)).ToListAsync();
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

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByTitle(string searchText, int? page)
        {
            var foundPhotos = await _context.Photos.Where(p => p.Title.Contains(searchText)).ToListAsync();
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

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotosByDescription(string searchText, int? page)
        {
            var foundPhotos = await _context.Photos.Where(p => p.Description.Contains(searchText)).ToListAsync();
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
            var foundPhotos = await _context.Photos.Include(p => p.Category).Where(p => p.Category == category).ToListAsync();
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
