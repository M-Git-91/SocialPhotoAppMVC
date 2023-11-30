using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly ApplicationDbContext _context;

        public AlbumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IPagedList<Album>>> GetAllAlbums(int? page)
        {
            var allAlbums = await _context.Albums.OrderByDescending(p => p.DateCreated).ToListAsync();
            var response = new ServiceResponse<IPagedList<Album>>();

            if (allAlbums.Count == 0)
            {
                response.Success = false;
                response.Message = "No albums were found.";
                return response;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var pagedList = await allAlbums.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        public Task<ServiceResponse<Album>> GetAlbumByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IPagedList<Album>>> GetUserAlbums(string currentUserId, int? page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<Album>> GetAlbumDetail(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> CreateAlbum(CreateAlbumVM albumVM)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> DeleteAlbumAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> EditAlbumAsync(EditAlbumVM editAlbumVM)
        {
            throw new NotImplementedException();
        }
    }
}
