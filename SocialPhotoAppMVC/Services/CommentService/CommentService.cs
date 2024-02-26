
using Microsoft.EntityFrameworkCore;
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
    }
}
