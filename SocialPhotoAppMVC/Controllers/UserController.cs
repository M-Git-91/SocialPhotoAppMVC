using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.ViewModels;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _webHost;

        public UserController(ApplicationDbContext context, IHttpContextAccessor httpContext, IWebHostEnvironment webHost)
        {
            _context = context;
            _httpContext = httpContext;
            _webHost = webHost;
        }

        public async Task<IActionResult> ListUsers()
        {
            List<AppUser> allUsers = await _context.Users.ToListAsync();

            return View(allUsers);
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            var findUser = await _context.Users
                .Include(u => u.Photos)
                .Include(u =>u.Albums)
                .FirstOrDefaultAsync(p => p.Id == id);
            return View(findUser);
        }

        public async Task<IActionResult> UserPhotos()
        {
            var currentUser = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userPhotos = await _context.Photos.Where(p => p.User.Id == currentUser).ToListAsync();

            return View(userPhotos);
        }
    }
}