using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Controllers;
using SocialPhotoAppMVC.Services.SearchService;
using SocialPhotoAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests.Controllers
{
    public class SearchControllerTests
    {
        private readonly ISearchService _searchService;
        private readonly SearchController _searchController;

        public SearchControllerTests()
        {
            //Dependencies
            _searchService = A.Fake<ISearchService>();

            //System under test
            _searchController = new SearchController(_searchService);
        }

        [Fact]
        public void SearchController_SearchPhotos_ReturnSuccess() 
        {
            //Arrange
            var searchInput = A.Fake<SearchPhotoVM>();
            int page = 1;
            var foundPhotos = A.Fake<ServiceResponse<IPagedList<Photo>>>();
            A.CallTo(() => _searchService.SearchPhotos(searchInput, page)).Returns(foundPhotos);

            //Act
            var result = _searchController.SearchPhotos(searchInput, page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void SearchController_SearchAlbums_ReturnSuccess()
        {
            //Arrange
            var searchInput = A.Fake<SearchAlbumVM>();
            int page = 1;
            var foundAlbums = A.Fake<ServiceResponse<IPagedList<Album>>>();
            A.CallTo(() => _searchService.SearchAlbums(searchInput, page)).Returns(foundAlbums);

            //Act
            var result = _searchController.SearchAlbums(searchInput, page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void SearchController_SearchUsers_ReturnSuccess()
        {
            //Arrange
            var searchInput = A.Fake<SearchUserVM>();
            int page = 1;
            var foundUsers = A.Fake<ServiceResponse<IPagedList<AppUser>>>();
            A.CallTo(() => _searchService.SearchUsers(searchInput, page)).Returns(foundUsers);

            //Act
            var result = _searchController.SearchUsers(searchInput, page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
