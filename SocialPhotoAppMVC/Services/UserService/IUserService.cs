using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<IPagedList<AppUser>>> GetAllUsers(int? page);
        Task<ServiceResponse<AppUserProfileVM>> GetUserProfile(string id, int? photosPageCount, int? albumsPageCount, int photosPerPage, int albumsPerPage);
        Task<ServiceResponse<bool>> ChangeNickname(ChangeNicknameVM nicknameVM);
        Task<ServiceResponse<bool>> ChangeProfilePhoto(ChangeProfilePhotoVM profilePhotoVM);
        Task<AppUser?> GetUserById(string id);
    }
}
