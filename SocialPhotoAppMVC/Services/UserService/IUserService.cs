using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<IPagedList<AppUser>>> GetAllUsers(int? page);
        Task<ServiceResponse<AppUserProfileDTO>> GetUserProfile(string id, int? photosPage, int? albumsPage);
        Task<ServiceResponse<bool>> ChangeNickname(ChangeNicknameVM nicknameVM);
        Task<AppUser?> GetUserById(string id);
    }
}
