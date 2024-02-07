using CloudinaryDotNet.Actions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http;
using NuGet.DependencyResolver;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests.Services.PhotoServiceTests
{
    public class PhotoServiceTests
    {
        private readonly ICloudService _cloudService;
        private readonly IHttpContextAccessor _httpContext;

        public PhotoServiceTests()
        {
            _cloudService = A.Fake<ICloudService>();
            _httpContext = A.Fake<IHttpContextAccessor>();
        }

        [Fact]
        public async void PhotoService_GetAllPhotos_ReturnPhotos()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var page = 1;

            //Act
            var result = await service.GetAllPhotos(page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>()
                .Subject.Data.Count().Should().Be(3);
        }

        [Fact]
        public async void PhotoService_GetAllPhotos_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var photos = await dbContext.Photos.ToListAsync();
            dbContext.Photos.RemoveRange(photos);
            await dbContext.SaveChangesAsync();

            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var page = 1;

            //Act
            var result = await service.GetAllPhotos(page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void PhotoService_GetPhotoByIdAsync_ReturnPhoto() 
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            int photoId = 1;
            var service = new PhotoService(dbContext, _cloudService, _httpContext);

            //Act
            var result = await service.GetPhotoByIdAsync(photoId);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<Photo>>();
            result.Data.Id.Should().Be(photoId);
            result.Data.User.Id.Should().Be("1");
        }

        [Fact]
        public async void PhotoService_GetPhotoByIdAsync_ReturnFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            int photoId = 100;
            var service = new PhotoService(dbContext, _cloudService, _httpContext);

            //Act
            var result = await service.GetPhotoByIdAsync(photoId);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void PhotoService_GetFeaturedPhotos_ReturnFeaturedPhotos() 
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var featuredPhoto = await dbContext.Photos.FindAsync(1);
            featuredPhoto.IsFeatured = true;
            await dbContext.SaveChangesAsync();

            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var page = 1;

            //Act
            var result = await service.GetFeaturedPhotos(page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>();
        }

        [Fact]
        public async void PhotoService_GetFeaturedPhotos_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var page = 1;

            //Act
            var result = await service.GetFeaturedPhotos(page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void PhotoService_GetUserPhotos_ReturnUserPhotos()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var currentUserId = "1";
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var page = 1;
            var resultsPerPage = 6;

            //Act
            var result = await service.GetUserPhotos(currentUserId, page, resultsPerPage);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>();
            result.Data.Should().HaveCount(1);
        }

        [Fact]
        public async void PhotoService_GetUserPhotos_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var currentUserId = "100";
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var page = 1;
            var resultsPerPage = 6;

            //Act
            var result = await service.GetUserPhotos(currentUserId, page, resultsPerPage);

            //Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>();
        }

        [Fact]
        public async void PhotoService_AddPhotoToAlbumGET_ReturnAddPhotoToAlbumVM() 
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var photoId = 1;

            //Act
            var result = await service.AddPhotoToAlbumGET(photoId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<AddPhotoToAlbumVM>>();
            result.Data.Photo.Id.Should().Be(photoId);
        }

        [Fact]
        public async void PhotoService_AddPhotoToAlbumGET_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var albums = await dbContext.Albums.ToListAsync();
            dbContext.RemoveRange(albums);
            await dbContext.SaveChangesAsync();

            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var photoId = 1;

            //Act
            var result = await service.AddPhotoToAlbumGET(photoId);

            //Assert
            result.Data.Should().BeOfType<AddPhotoToAlbumVM>();
            result.Data.Photo.Id.Should().Be(photoId);
            result.Data.UserAlbums.Should().HaveCount(0);
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void PhotoService_RemovePhotoFromAlbumGET_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var photoId = 1;

            //Act
            var result = await service.RemovePhotoFromAlbumGET(photoId);

            //Assert
            result.Success.Should().BeFalse();
            result.Data.Photo.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<AddPhotoToAlbumVM>>();
        }

        [Fact]
        public async void PhotoService_UploadPhoto_ReturnSuccessTrue()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var photoVM = new UploadPhotoVM { 
                Title = "", 
                Description = "", 
                Category = Enums.Category.Abstract, 
                Image = A.Fake<IFormFile>(),
                UserId = "1"
            };
            var imageUploadResult = new ImageUploadResult { Url = A.Fake<Uri>() };

            A.CallTo(() => _cloudService.AddPhotoAsync(photoVM.Image)).Returns(imageUploadResult);

            //Act
            var result = await service.UploadPhoto(photoVM);

            //Assert
            result.Should().BeOfType<ServiceResponse<bool>>();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void PhotoService_EditPhoto_ReturnSuccessTrue()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            dbContext.ChangeTracker.Clear();
            var service = new PhotoService(dbContext, _cloudService, _httpContext);
            var fakeService = A.Fake<PhotoService>(options =>
                options.WithArgumentsForConstructor(
                    () => new PhotoService(dbContext, _cloudService, _httpContext)));

            var editPhotoVM = new EditPhotoVM { 
                Title = "", 
                Description = "", 
                Category = Enums.Category.Abstract, 
                ImageUrl = "", 
                NewImage = A.Fake<IFormFile>(),
                AuthorId = "1",
                CurrentUserId = "1",
                PhotoId = 1
            };
            var newPhotoUpload = new ImageUploadResult { Url = A.Fake<Uri>() };
            A.CallTo(() => _cloudService.AddPhotoAsync(editPhotoVM.NewImage)).Returns(newPhotoUpload);

            //Act
            var result = await service.EditPhoto(editPhotoVM);

            //Assert
            result.Should().BeOfType<ServiceResponse<bool>>();
            result.Success.Should().BeTrue();
        }
    }
}
