using X.PagedList;

namespace SocialPhotoAppMVC.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<IPagedList<AppUser>>> GetAllUsers(int? page);
        Task<ServiceResponse<AppUser>> GetUserById(string id);
    }
}
