using SocialPhotoAppMVC.Services.SearchService;
using SocialPhotoAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests.Services.SearchServiceTests
{
    public class SearchServiceTests
    {
        [Fact]
        public async void SearchService_SearchPhotos_ReturnPhotoByExactTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "TestPhotoName1" };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnPhotosByPartialTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "Test" };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>()
                .Subject.Data.Count().Should().Be(3);
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnPhotoByTitleAndDescription()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "Test", Description = "description1" };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnPhotoByCategory()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Category = Enums.Category.Animals };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>()
                .Subject.Data.Count().Should().Be(3);
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnPhotoByCategoryAndTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "1",Category = Enums.Category.Animals };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Photo>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnsSuccessFalseByTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "notFound"};

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnsSuccessFalseByDescription()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "test", Description = "notFound" };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void SearchService_SearchPhotos_ReturnsSuccessFalseByCategory()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchPhotoVM { Title = "test", Description = "notFound", Category = Enums.Category.Aerial };

            //Act
            var result = await service.SearchPhotos(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnAlbumByExactTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "TestAlbumName1" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Album>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnAlbumsByPartialTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "Test" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Album>>>()
                .Subject.Data.Count().Should().Be(3);
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnAlbumsByTitleAndDescription()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "Test", Description = "description1" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Album>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnAlbumByNickName()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Username = "1" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Album>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnAlbumByCategoryAndNickName()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "testalbumname1", Username = "1" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<Album>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnsSuccessFalseByTitle()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "notFound" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnsSuccessFalseByDescription()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "test", Description = "notFound" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void SearchService_SearchAlbums_ReturnsSuccessFalseByNickName()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchAlbumVM { Title = "test", Description = "test", Username = "notFound" };

            //Act
            var result = await service.SearchAlbums(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void SearchService_SearchUsers_ReturnUserByExactNickname()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchUserVM { NickName = "Nick1" };

            //Act
            var result = await service.SearchUsers(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<AppUser>>>()
                .Subject.Data.Count().Should().Be(1);
        }

        [Fact]
        public async void SearchService_SearchUsers_ReturnUsersByPartialNickname()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchUserVM { NickName = "nick" };

            //Act
            var result = await service.SearchUsers(searchInput, page);

            //Assert
            result.Data.Should().NotBeNull();
            result.Should().BeOfType<ServiceResponse<IPagedList<AppUser>>>()
                .Subject.Data.Count().Should().Be(3);
        }

        [Fact]
        public async void SearchService_SearchUsers_ReturnSuccessFalse()
        {
            //Arrange
            var dbContext = await InMemoryDb.GetDbContext();
            var service = new SearchService(dbContext);
            var page = 1;
            var searchInput = new SearchUserVM { NickName = "notFoundNick" };

            //Act
            var result = await service.SearchUsers(searchInput, page);

            //Assert
            result.Data.Should().BeNull();
            result.Success.Should().BeFalse();
        }
    }
}
