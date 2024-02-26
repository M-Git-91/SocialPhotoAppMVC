
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Models;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<bool>> CreateCommentPOST(CreateCommentVM commentVM)
        {
            var response = new ServiceResponse<bool>();

            var commentModel = new Comment
            {
                Text = commentVM.Text,
                AppUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == commentVM.UserId),
                Photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == commentVM.PhotoId)
            };

            _context.Comments.Add(commentModel);
            var saveResult = Save();
            if (saveResult == false)
            {
                response.Data = false;
                response.Success = false;
                response.Message = $"Comment was not created";
                return response ;
            }
            return response;

        }

        public async Task<ServiceResponse<IPagedList<Comment>>> GetCommentsByPhotoId(int photoId, int? page, int commentsPerPage)
        {
            var response = new ServiceResponse<IPagedList<Comment>>();
            var comments = await _context.Comments.Where(c => c.Photo.Id == photoId).ToListAsync();

            if (comments.Count == 0)
            {
                response.Success = false;
                response.Message = "No comments.";
                return response;
            }

            IPagedList<Comment> pagedList = await PaginateListOfComments(page, commentsPerPage, comments);
            response.Data = pagedList;

            return response;
        }

        public async Task<IPagedList<Comment>> PaginateListOfComments(int? page, int resultsPerPage, List<Comment> allComments)
        {
            int pageNumber = (page ?? 1);
            var pagedList = await allComments.ToPagedListAsync(pageNumber, resultsPerPage);
            return pagedList;
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
