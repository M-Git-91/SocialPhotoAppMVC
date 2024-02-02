using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudService _cloudService;

        public AlbumService(ApplicationDbContext context, ICloudService cloudService)
        {
            _context = context;
            _cloudService = cloudService;
        }

        public async Task<ServiceResponse<IPagedList<Album>>> GetAllAlbums(int? page)
        {
            var allAlbums = await _context.Albums.Include(a => a.User).OrderByDescending(a => a.DateCreated).ToListAsync();
            var response = new ServiceResponse<IPagedList<Album>>();

            if (allAlbums.Count == 0)
            {
                response.Success = false;
                response.Message = "No albums were found.";
                return response;
            }

            IPagedList<Album> pagedList = await PaginateListOfAlbums(page, 6, allAlbums);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<Album>> GetAlbumByIdAsync(int id)
        {
            var album = await _context.Albums.AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            var response = new ServiceResponse<Album> { Data = album };

            if (album == null)
            {
                response.Success = false;
                response.Message = "No album found.";
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Album>>> GetUserAlbums(string currentUserId, int? page, int albumsPerPage)
        {
            var userAlbums = await _context.Albums.Where(p => p.User.Id == currentUserId).ToListAsync();
            var response = new ServiceResponse<IPagedList<Album>>();

            if (userAlbums.Count == 0)
            {
                response.Success = false;
                response.Message = "No albums found.";
                return response;
            }

            IPagedList<Album> pagedList = await PaginateListOfAlbums(page, albumsPerPage, userAlbums);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<Album>> GetAlbumDetail(int id)
        {
            var album = await _context.Albums.Include(p => p.User).Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id);
            var response = new ServiceResponse<Album> { Data = album };

            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Album not found.";
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> CreateAlbum(CreateAlbumVM albumVM)
        {
            var response = new ServiceResponse<bool>();

            var uploadPhoto = await _cloudService.AddPhotoAsync(albumVM.CoverArt);          

            if (uploadPhoto.Error != null)
            {
                return NegativeResponse("CoverArt upload unsuccessful.");
            }

            var album = new Album
            {
                Title = albumVM.Title,
                Description = albumVM.Description,
                CoverArtUrl = uploadPhoto.Url.ToString(),
                User = await _context.Users.FirstOrDefaultAsync(u => u.Id == albumVM.UserId),
            };

            if (album.User == null)
            {
                return NegativeResponse("User not found.");
            }

            _context.Albums.Add(album);
            var saveResult = Save();

            if (saveResult == false)
            {
                return NegativeResponse("Album was not created.");
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteAlbum(int albumId)
        {
            var response = new ServiceResponse<bool>();
            var album = await GetAlbumByIdAsync(albumId);

            if (album.Data == null)
            {
                return NegativeResponse("Album not found.");
            }

            if (!string.IsNullOrEmpty(album.Data.CoverArtUrl))
            {
                await _cloudService.DeletePhotoAsync(album.Data.CoverArtUrl);
            }

            _context.Albums.Remove(album.Data);
            var saveResult = Save();

            if (saveResult == false)
            {
                return NegativeResponse("Album was not removed");
            }

            response.Data = false;
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<bool>> EditAlbum(EditAlbumVM editAlbumVM)
        {
            var response = new ServiceResponse<bool>();
            var oldAlbum = await GetAlbumByIdAsync(editAlbumVM.AlbumId);
            if (oldAlbum == null)
            {
                return NegativeResponse("Album not found.");
            }

            var newCoverUpload = await _cloudService.AddPhotoAsync(editAlbumVM.NewCoverArt);
            if (newCoverUpload.Error != null)
            {
                return NegativeResponse("Cover art upload unsuccessful.");
            }

            if (!string.IsNullOrEmpty(oldAlbum.Data.CoverArtUrl))
            {
                await _cloudService.DeletePhotoAsync(oldAlbum.Data.CoverArtUrl);
            }

            var newAlbum = new Album
            {
                Id = editAlbumVM.AlbumId,
                Title = editAlbumVM.Title,
                Description = editAlbumVM.Description,
                CoverArtUrl = newCoverUpload.Url.ToString(),
                User = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == editAlbumVM.CurrentUserId)

            };

            _context.Albums.Update(newAlbum);
            var saveResult = Save();

            if (saveResult == false)
            {
                return NegativeResponse("Album was not edited.");
            }

            response.Data = true;
            response.Success = true;

            return response;

        }

        public async Task<IPagedList<Album>> PaginateListOfAlbums(int? page, int resultsPerPage, List<Album> allAlbums)
        {
            int pageNumber = (page ?? 1);
            var pagedList = await allAlbums.ToPagedListAsync(pageNumber, resultsPerPage);
            return pagedList;
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        private ServiceResponse<bool> NegativeResponse(string errorMessage)
        {
            var response = new ServiceResponse<bool>();
            response.Data = false;
            response.Success = false;
            response.Message = $"{errorMessage}";
            return response;
        }
    }   
}
