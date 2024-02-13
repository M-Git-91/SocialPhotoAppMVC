using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.Services.AlbumService;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.Services.UserService;
using SocialPhotoAppMVC.ViewModels;
using System.Linq;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(SearchPhotoVM? searchInput, int? page)
        {
            var foundPhotos = await _context.Photos
                            .Where(p => (searchInput.Title == null || p.Title.ToLower().Trim()
                            .Contains(searchInput.Title.ToLower().Trim())) &&
                            (searchInput.Description == null || p.Description.ToLower().Trim()
                            .Contains(searchInput.Description.ToLower().Trim())) &&
                            (searchInput.Category == null || p.Category == searchInput.Category))
                            .ToListAsync();


            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            var pagedList = await PaginateListOfPhotos(page, 6 ,foundPhotos);

            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Album>>> SearchAlbums(SearchAlbumVM searchInput, int? page)
        {
            var foundAlbums = await _context.Albums.Include(a => a.User)
                .Where(p => (searchInput.Title == null || p.Title.ToLower().Trim()
                .Contains(searchInput.Title.ToLower().Trim())) &&
                    (searchInput.Description == null || p.Description.ToLower().Trim()
                .Contains(searchInput.Description.ToLower().Trim())) && 
                    (searchInput.Username == null || p.User.NickName.ToLower().Trim()
                .Contains(searchInput.Username.ToLower().Trim())))
                .ToListAsync();


            var response = new ServiceResponse<IPagedList<Album>>();

            if (foundAlbums.Count == 0)
            {
                response.Success = false;
                response.Message = "No albums found.";
                return response;
            }

            var pagedList = await PaginateListOfAlbums(page, 6, foundAlbums);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<AppUser>>> SearchUsers(SearchUserVM searchInput, int? page)
        {
            var foundUsers = await _context.Users
                .Where(u => (searchInput.NickName == null || u.NickName.ToLower().Trim()
                .Contains(searchInput.NickName.ToLower().Trim())))
                .ToListAsync();

            var response = new ServiceResponse<IPagedList<AppUser>>();

            if (foundUsers.Count == 0)
            {
                response.Success = false;
                response.Message = "No users found.";
                return response;
            }

            var pagedList = await PaginateListOfUsers(page, 6, foundUsers);
            response.Data = pagedList;

            return response;
        }

        private async Task<IPagedList<Photo>> PaginateListOfPhotos(int? page, int resultsPerPage, List<Photo> allPhotos)
        {
            int pageNumber = (page ?? 1);
            var pagedList = await allPhotos.ToPagedListAsync(pageNumber, resultsPerPage);
            return pagedList;
        }

        private async Task<IPagedList<Album>> PaginateListOfAlbums(int? page, int resultsPerPage, List<Album> allAlbums)
        {
            int pageNumber = (page ?? 1);
            var pagedList = await allAlbums.ToPagedListAsync(pageNumber, resultsPerPage);
            return pagedList;
        }

        private async Task<IPagedList<AppUser>> PaginateListOfUsers(int? page, int resultsPerPage, List<AppUser> allUsers)
        {
            int pageNumber = (page ?? 1);
            var pagedList = await allUsers.ToPagedListAsync(pageNumber, resultsPerPage);
            return pagedList;
        }
    }
}
