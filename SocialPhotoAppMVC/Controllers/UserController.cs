using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Services.UserService;
using SocialPhotoAppMVC.ViewModels;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserService _userService;

        public UserController(IHttpContextAccessor httpContext, IUserService userService)
        {
            _httpContext = httpContext;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> ListUsers(int? page)
        {
            var allUsers = await _userService.GetAllUsers(page);

            if (allUsers.Success == false)
            {
                var errorMessage = allUsers.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(allUsers);
        }
        [HttpGet]
        public async Task<IActionResult> UserProfile(string id, int? page)
        {
            var userProfile = await _userService.GetUserProfile(id, page);

            if (userProfile.Success == false)
            {
                var errorMessage = userProfile.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(userProfile);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> ChangeNickname() 
        {
            string currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userModel = await _userService.GetUserById(currentUserId);

            var changeNickVM = new ChangeNicknameVM { CurrentUserId = currentUserId, Nickname = userModel.NickName};

            return View(changeNickVM);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> ChangeNickname(ChangeNicknameVM newNick) 
        {
            await _userService.ChangeNickname(newNick);

            return RedirectToAction("ChangeNickname");
        }
    }
}