﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialPhotoAppMVC.ViewModels;

namespace SocialPhotoAppMVC.Tests.Services.AlbumServiceTests
{
    public class AlbumServiceTests
    {
        private readonly ICloudService _cloudService;
        public AlbumServiceTests()
        {
            _cloudService = A.Fake<ICloudService>();
        }

        [Fact]
        public async void AlbumService_GetAllAlbums_ReturnAlbums()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new AlbumService(dbContext, _cloudService);
            var page = 1;

            //Act
            var result = await service.GetAllAlbums(page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Album>>>()
                .Subject.Data.Count().Should().Be(3);
        }

        [Fact]
        public async void AlbumService_GetAllAlbums_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var albumsToRemove = await dbContext.Albums.ToListAsync();
            dbContext.RemoveRange(albumsToRemove);
            await dbContext.SaveChangesAsync();

            var service = new AlbumService(dbContext, _cloudService);
            var page = 1;

            //Act
            var result = await service.GetAllAlbums(page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void AlbumService_GetAlbumByIdAsync_ReturnAlbum()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new AlbumService(dbContext, _cloudService);
            var id = 3;

            //Act
            var result = await service.GetAlbumByIdAsync(id);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
            result.Data.User.Id.Should().Be($"{id}");
        }


        [Fact]
        public async void AlbumService_GetUserAlbums_ReturnAlbums()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new AlbumService(dbContext, _cloudService);
            var currentUserId = "1";
            var page = 1;
            var albumsPerPage = 1;

            //Act
            var result = await service.GetUserAlbums(currentUserId, page, albumsPerPage);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.PageNumber.Should().Be(1);
            result.Data.Count.Should().Be(1);
        }

        [Fact]
        public async void AlbumService_GetUserAlbums_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var albums = await dbContext.Albums.ToListAsync();
            dbContext.Albums.RemoveRange(albums);
            await dbContext.SaveChangesAsync();

            var service = new AlbumService(dbContext, _cloudService);
            var currentUserId = "1";
            var page = 1;
            var albumsPerPage = 1;

            //Act
            var result = await service.GetUserAlbums(currentUserId, page, albumsPerPage);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void AlbumService_GetAlbumDetail_ReturnAlbum() 
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            int albumId = 1;
            var service = new AlbumService(dbContext, _cloudService);

            //Act
            var result = await service.GetAlbumDetail(albumId);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(albumId);
            result.Data.User.Id.Should().Be("1");
            result.Data.Photos.Count.Should().Be(0);
        }

        [Fact]
        public async void AlbumService_GetAlbumDetail_ReturnSuccesFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var albums = await dbContext.Albums.ToListAsync();
            dbContext.Albums.RemoveRange(albums);
            await dbContext.SaveChangesAsync();

            int albumId = 1;
            var service = new AlbumService(dbContext, _cloudService);

            //Act
            var result = await service.GetAlbumDetail(albumId);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void AlbumService_DeleteAlbum_ReturnDataFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            int albumId = 1;
            var service = new AlbumService(dbContext, _cloudService);
            dbContext.ChangeTracker.Clear();

            //Act
            var result = await service.DeleteAlbum(albumId);

            //Assert
            result.Data.Should().BeFalse();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void AlbumService_DeleteAlbum_ReturnResultFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            int albumId = 100;
            var service = new AlbumService(dbContext, _cloudService);
            dbContext.ChangeTracker.Clear();

            //Act
            var result = await service.DeleteAlbum(albumId);

            //Assert
            result.Data.Should().BeFalse();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void AlbumService_CreateAlbum_ReturnBool()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new AlbumService(dbContext, _cloudService);
            
            var fakeService = A.Fake<AlbumService>(options =>
                options.WithArgumentsForConstructor(
                    () => new AlbumService(dbContext, _cloudService)));

            var CreateAlbumVM = new CreateAlbumVM { 
                Title = "", 
                Description = "", 
                CoverArt = A.Fake<IFormFile>(), 
                UserId = "1" };

            var albumModel = new Album { User = A.Fake<AppUser>() };
            var imageUploadResult = new ImageUploadResult { Url = A.Fake<Uri>() };
            
            A.CallTo(() => _cloudService.AddPhotoAsync(CreateAlbumVM.CoverArt)).Returns(imageUploadResult);
            A.CallTo(() => fakeService.MapCreateAlbumVMtoAlbum(CreateAlbumVM, imageUploadResult)).Returns(albumModel);

            //Act
            var result = await service.CreateAlbum(CreateAlbumVM);

            //Assert
            result.Should().BeOfType<ServiceResponse<bool>>();
            result.Success.Should().BeTrue();
        }
    }
}
