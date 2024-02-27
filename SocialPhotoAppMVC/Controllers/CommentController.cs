using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.Services.CommentService;
using System.Security.Claims;

namespace SocialPhotoAppMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IHttpContextAccessor _httpContext;

        public CommentController(ICommentService commentService, IHttpContextAccessor httpContext)
        {
            _commentService = commentService;
            _httpContext = httpContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> CreateComment(int id)
        {
            var currentUserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            CreateCommentVM newCreateCommentVM = new CreateCommentVM { UserId = currentUserId, PhotoId = id };
            return View(newCreateCommentVM);
        }

        [HttpPost, ActionName("CreateComment"), Authorize]
        public async Task<IActionResult> CreateCommentPOST(CreateCommentVM commentVM) 
        {
            if (ModelState.IsValid)
            {
                await _commentService.CreateCommentPOST(commentVM);

                return RedirectToAction();
            }
            else
            {
                ModelState.AddModelError("", "Photo upload unsuccessful.");
                return View(commentVM);
            }

        }
    }
}
