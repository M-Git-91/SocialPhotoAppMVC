﻿using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.Services.AlbumService;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.ViewModels;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IAlbumService _albumService;
        private readonly ICloudService _cloudService;

        public UserService(ApplicationDbContext context, IPhotoService photoService, IAlbumService albumService, ICloudService cloudService)
        {
            _context = context;
            _photoService = photoService;
            _albumService = albumService;
            _cloudService = cloudService;
        }

        public async Task<ServiceResponse<IPagedList<AppUser>>> GetAllUsers(int? page)
        {
            var response = new ServiceResponse<IPagedList<AppUser>>();
            var allUsers = await _context.Users.OrderByDescending(u => u.DateCreated).ToListAsync();

            if (allUsers.Count == 0)
            {
                response.Success = false;
                response.Message = "No users found.";
                return response;
            }

            IPagedList<AppUser> pagedList = await PaginateListOfUsers(page, 6, allUsers);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<AppUserProfileVM>> GetUserProfile(string id, int? photosPageCount, int? albumsPageCount, int photosPerPage, int albumsPerPage)
        {
            var response = new ServiceResponse<AppUserProfileVM>();

            AppUser? user = await GetUserById(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            var userPhotos = await _photoService.GetUserPhotos(id, photosPageCount, photosPerPage);
            var userAlbums = await _albumService.GetUserAlbums(id, albumsPageCount, albumsPerPage);

            var userProfileVM = new AppUserProfileVM
            {
                UserId = id,
                NickName = user.NickName,
                Email = user.Email,
                ProfilePictureURL = user.ProfilePictureURL,
                DateCreated = user.DateCreated,
                Photos = userPhotos.Data,
                Albums = userAlbums.Data
            };

            response.Data = userProfileVM;

            return response;
        }

        public async Task<AppUser?> GetUserById(string id)
        {
            return await _context.Users
                .Include(u => u.Photos)
                .Include(u => u.Albums)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ServiceResponse<bool>> ChangeNickname(ChangeNicknameVM nicknameVM)
        {
            var response = new ServiceResponse<bool>();
            var userModel = await GetUserById(nicknameVM.CurrentUserId);

            userModel.NickName = nicknameVM.Nickname;
            _context.AppUsers.Update(userModel);
            await _context.SaveChangesAsync();

            response.Data = true;
            return response;
        }
        public async Task<ServiceResponse<bool>> ChangeProfilePhoto(ChangeProfilePhotoVM profilePhotoVM)
        {
            var response = new ServiceResponse<bool>();
            var userModel = await GetUserById(profilePhotoVM.CurrentUserId);

            var newPhotoUpload = await _cloudService.UploadProfilePhoto(profilePhotoVM.NewProfileImage);

            if (!string.IsNullOrEmpty(userModel.ProfilePictureURL))
            {
                await _cloudService.DeletePhotoAsync(userModel.ProfilePictureURL);
            }

            userModel.ProfilePictureURL = newPhotoUpload.Url.ToString();

            _context.AppUsers.Update(userModel); 
            await _context.SaveChangesAsync();

            response.Data = true;
            return response;
        }

        private async Task<IPagedList<AppUser>> PaginateListOfUsers(int? page, int resultsPerPage, List<AppUser> allUsers)
        {
            int pageNumber = (page ?? 1);
            var pagedList = await allUsers.ToPagedListAsync(pageNumber, resultsPerPage);
            return pagedList;
        }
    }
}
