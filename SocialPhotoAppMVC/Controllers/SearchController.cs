using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SocialPhotoAppMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SearchAll(string searchInput)
        {
            var photos = await _context.Photos.Where(p => p.Title.Contains(searchInput)|| p.Description.Contains(searchInput)).ToListAsync();
            var users = await _context.AppUsers.Where(u => u.NickName.Contains(searchInput)).ToListAsync();

            return View();
        }


    }
}
