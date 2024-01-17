using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<ServiceResponse<AppUser>> GetUserById(string id)
        {
            var response = new ServiceResponse<AppUser>();

            var findUser = await _context.Users
                .Include(u => u.Photos)
                .Include(u => u.Albums)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (findUser == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            response.Data = findUser;

            return response;
        }

        public async Task<ServiceResponse<bool>> ChangeNickname(ChangeNicknameVM nicknameVM)
        {
            var response = new ServiceResponse<bool>();
            var userModel = await GetUserById(nicknameVM.CurrentUserId);

            userModel.Data.NickName = nicknameVM.Nickname;
            _context.AppUsers.Update(userModel.Data);
            await _context.SaveChangesAsync();

            response.Data = true;
            return response;
        }
    }
}
