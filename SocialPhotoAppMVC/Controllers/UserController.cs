using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SocialPhotoAppMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListUsers()
        {
            List<AppUser> allUsers = await _context.Users.ToListAsync();

            return View(allUsers);
        }
    }
}
