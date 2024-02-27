using X.PagedList;

namespace SocialPhotoAppMVC.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<IPagedList<Comment>>> GetCommentsByPhotoId(int photoId, int? page, int commentsPerPagee);
        Task<ServiceResponse<bool>> CreateCommentPOST(CreateCommentVM commentVM);
        Task<IPagedList<Comment>> PaginateListOfComments(int? page, int resultsPerPage, List<Comment> allComments);
    }
}
