using X.PagedList;

namespace SocialPhotoAppMVC.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<IPagedList<Comment>>> GetCommentsByPhotoId(int photoId, int? page, int commentsPerPagee);
    }
}
