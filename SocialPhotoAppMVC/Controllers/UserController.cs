using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Services.UserService;
using SocialPhotoAppMVC.ViewModels;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _webHost;
        private readonly IUserService _userService;

        public UserController(ApplicationDbContext context, IHttpContextAccessor httpContext, IWebHostEnvironment webHost, IUserService userService)
        {
            _context = context;
            _httpContext = httpContext;
            _webHost = webHost;
            _userService = userService;
        }

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

        public async Task<IActionResult> UserProfile(string id)
        {
            var findUser = await _userService.GetUserById(id);

            if (findUser.Success == false)
            {
                var errorMessage = findUser.Message;
                return View("ErrorPage", errorMessage);
            }

            return View(findUser);
        }
    }
}