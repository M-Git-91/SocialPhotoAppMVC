﻿
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.ViewModels;

namespace SocialPhotoAppMVC.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudService _cloudService;

        public PhotoService(ApplicationDbContext context, ICloudService cloudService)
        {
            _context = context;
            _cloudService = cloudService;
        }

        public async Task<IEnumerable<Photo>> GetAllPhotos()
        {
            IEnumerable<Photo> result = await _context.Photos.OrderByDescending(p => p.DateCreated).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Photo>> GetFeaturedPhotos()
        {
            IEnumerable<Photo> result = await _context.Photos.Where(p => p.IsFeatured == true).ToListAsync();
            return result;
        }

        public async Task<Photo> GetPhotoDetail(int id)
        {
            Photo result = await _context.Photos.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task<bool> UploadPhoto(UploadPhotoVM photoVM)
        {
            var result = _cloudService.AddPhoto(photoVM.Image);
            var photo = new Photo
            {
                Title = photoVM.Title,
                Description = photoVM.Description,
                ImageUrl = result.Url.ToString(),
                Category = photoVM.Category,
                User = await _context.Users.FirstOrDefaultAsync(u => u.Id == photoVM.UserId),
            };
            _context.Photos.Add(photo);
            _context.SaveChanges();
            return true;
        }
    }
}
