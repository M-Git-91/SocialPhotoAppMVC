using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public PhotoController(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<IActionResult> Index()
        {
            var recentPhotos = await _context.Photos.OrderByDescending(p => p.DateCreated).ToListAsync();

            return View(recentPhotos);
        }

        public async Task<IActionResult> FeaturedPhotos()
        {
            var featuredPhotos = await _context.Photos.Where(p => p.IsFeatured == true).ToListAsync();

            return View(featuredPhotos);
        }
        
        public async Task<IActionResult> UserPhotos()
        {
            var currentUser = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userPhotos = await _context.Photos.Where(p => p.User.Id == currentUser).ToListAsync();

            return View(userPhotos);
        }

        public async Task<IActionResult> PhotoDetail(int id) 
        {
            var findPhoto = _context.Photos.FirstOrDefault(p => p.Id == id);
            return View(findPhoto);
        }
    }

}
