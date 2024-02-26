using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SocialPhotoAppMVC.Services.CommentService;

namespace SocialPhotoAppMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View();
        }

       // public async Task<IActionResult> GetCommentsByPhotoId(int photoId, int? page, int commentsPerPage) 
       // {
       //     var result = _commentService.GetCommentsByPhotoId(photoId, page, commentsPerPage);
       //
//
       // }
    }
}
