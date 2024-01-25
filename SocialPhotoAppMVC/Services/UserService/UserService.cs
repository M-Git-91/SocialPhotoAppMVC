using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.Services.AlbumService;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IAlbumService _albumService;

        public UserService(ApplicationDbContext context, IPhotoService photoService, IAlbumService albumService)
        {
            _context = context;
            _photoService = photoService;
            _albumService = albumService;
        }

        public async Task<ServiceResponse<IPagedList<AppUser>>> GetAllUsers(int? page)
        {
            var response = new ServiceResponse<IPagedList<AppUser>>();
            var allUsers = await _context.Users.OrderByDescending(u => u.DateCreated).ToListAsync();

            if (allUsers.Count == 0)
            {
                response.Success = false;
                response.Message = "No users found.";
                return response;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var pagedList = await allUsers.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<AppUserProfileDTO>> GetUserProfile(string id, int? photosPageCount, int? albumsPageCount, int photosPerPage, int albumsPerPage)
        {
            var response = new ServiceResponse<AppUserProfileDTO>();

            AppUser? user = await GetUserById(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            var userPhotos = await _photoService.GetUserPhotos(id, photosPageCount, photosPerPage);
            var userAlbums = await _albumService.GetUserAlbums(id, albumsPageCount, albumsPerPage);

            var userProfileDTO = new AppUserProfileDTO
            {
                UserId = id,
                NickName = user.NickName,
                ProfilePictureURL = user.ProfilePictureURL,
                DateCreated = user.DateCreated,
                Photos = userPhotos.Data,
                Albums = userAlbums.Data
            };

            response.Data = userProfileDTO;

            return response;
        }

        public async Task<AppUser?> GetUserById(string id)
        {
            return await _context.Users
                .Include(u => u.Photos)
                .Include(u => u.Albums)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ServiceResponse<bool>> ChangeNickname(ChangeNicknameVM nicknameVM)
        {
            var response = new ServiceResponse<bool>();
            var userModel = await GetUserById(nicknameVM.CurrentUserId);

            userModel.NickName = nicknameVM.Nickname;
            _context.AppUsers.Update(userModel);
            await _context.SaveChangesAsync();

            response.Data = true;
            return response;
        }
    }
}
