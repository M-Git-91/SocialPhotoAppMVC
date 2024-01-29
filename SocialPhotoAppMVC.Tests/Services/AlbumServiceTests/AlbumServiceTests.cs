using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace SocialPhotoAppMVC.Tests.Services.AlbumServiceTests
{
    public class AlbumServiceTests
    {
        private readonly ICloudService _cloudService;
        public AlbumServiceTests()
        {
            _cloudService = A.Fake<ICloudService>();
        }

        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            var dbContext = new ApplicationDbContext(options);

            dbContext.Database.EnsureCreated();

                for (int i = 1; i < 4; i++)
                {
                    dbContext.Users.Add(
                    new AppUser
                    {
                        Id = $"{i}",
                    });

                    dbContext.Albums.Add(
                    new Album
                    {
                        Title = $"TestAlbumName{i}",
                        Description = $"TestDescription{i}",
                        User = dbContext.AppUsers.Find($"{i}"),
                    });  
                }
                await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void AlbumService_GetAllAlbums_ReturnAlbums()
        {
            //Arrange
            var dbContext = await GetDbContext();
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
            var dbContext = await GetDbContext();
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
        public async void AlbumService_GetAllAlbums_GetAlbumByIdAsync()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var service = new AlbumService(dbContext, _cloudService);
            var id = 3;

            //Act
            var result = await service.GetAlbumByIdAsync(id);

            //Assert
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
            result.Data.User.Id.Should().Be($"{id}");
        }


    }
}
