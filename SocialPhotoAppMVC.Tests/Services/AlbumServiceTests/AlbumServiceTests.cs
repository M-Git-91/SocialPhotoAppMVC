using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;

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

            if (await dbContext.Albums.CountAsync() <= 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    dbContext.Albums.Add(
                    new Album
                    {
                        Title = $"TestAlbumName{i}",
                        Description = $"TestDescription{i}",
                        User = new AppUser { Id = $"{i+1}"},
                    });
                }
                await dbContext.SaveChangesAsync();
            }
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
    }
}
