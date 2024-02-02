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
        private readonly IPhotoService _photoService;
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;

        public SearchService(
            ApplicationDbContext context, 
            IPhotoService photoService, 
            IAlbumService albumService, 
            IUserService userService)
        {
            _context = context;
            _photoService = photoService;
            _albumService = albumService;
            _userService = userService;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(SearchPhotoVM? searchInput, int? page)
        {
            var foundPhotos = await _context.Photos
                            .Where(p => (searchInput.Title == null || p.Title.ToLower()
                            .Contains(searchInput.Title.ToLower())) &&
                            (searchInput.Description == null || p.Description.ToLower()
                            .Contains(searchInput.Description.ToLower())) &&
                            (searchInput.Category == null || p.Category == searchInput.Category))
                            .ToListAsync();


            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            var pagedList = await _photoService.PaginateListOfPhotos(page, 1, foundPhotos);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Album>>> SearchAlbums(SearchAlbumVM searchInput, int? page)
        {
            var foundAlbums = await _context.Albums.Include(a => a.User)
                .Where(p => (searchInput.Title == null || p.Title.ToLower()
                .Contains(searchInput.Title.ToLower())) &&
                (searchInput.Description == null || p.Description.ToLower()
                .Contains(searchInput.Description.ToLower()))
                && (searchInput.Username == null || p.User.NickName.ToLower()
                .Contains(searchInput.Username.ToLower())))
                .ToListAsync();


            var response = new ServiceResponse<IPagedList<Album>>();

            if (foundAlbums.Count == 0)
            {
                response.Success = false;
                response.Message = "No albums found.";
                return response;
            }

            var pagedList = await _albumService.PaginateListOfAlbums(page, 6, foundAlbums);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<AppUser>>> SearchUsers(SearchUserVM searchInput, int? page)
        {
            var foundUsers = await _context.Users
                .Where(u => (searchInput.NickName == null || u.NickName.ToLower()
                .Contains(searchInput.NickName.ToLower())))
                .ToListAsync();

            var response = new ServiceResponse<IPagedList<AppUser>>();

            if (foundUsers.Count == 0)
            {
                response.Success = false;
                response.Message = "No users found.";
                return response;
            }

            var pagedList = await _userService.PaginateListOfUsers(page, 6, foundUsers);
            response.Data = pagedList;

            return response;
        }
    }
}
