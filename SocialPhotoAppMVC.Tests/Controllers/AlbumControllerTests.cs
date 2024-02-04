using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Controllers;
using SocialPhotoAppMVC.Models;
using SocialPhotoAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests.Controllers
{
    public class AlbumControllerTests
    {
        private readonly AlbumController _albumController;
        private readonly IAlbumService _albumService;
        private readonly IHttpContextAccessor _httpContext;

        public AlbumControllerTests()
        {
            //Dependencies
            _albumService = A.Fake<IAlbumService>();
            _httpContext = A.Fake<IHttpContextAccessor>();
            //System under test
            _albumController = new AlbumController(_albumService, _httpContext);
        }

        [Fact]
        public void AlbumController_RecentAlbums_ReturnsSuccessTrue() 
        {
            //Arrange
            var albums = A.Fake<ServiceResponse<IPagedList<Album>>>();
            int page = 1;
            A.CallTo(() => _albumService.GetAllAlbums(page)).Returns(albums);

            //Act
            var result = _albumController.RecentAlbums(page); ;

            //Assert (object and view check)
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AlbumController_AlbumDetail_ReturnsSuccess() 
        {
            //Arrange
            int id = 1;
            var album = A.Fake<ServiceResponse<Album>>();
            A.CallTo(() => _albumService.GetAlbumDetail(id)).Returns(album);

            //Act
            var result = _albumController.AlbumDetail(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();

        }

        [Fact]
        public void AlbumController_UserAlbums_ReturnsSuccess() 
        {
            //Arrange
            var albumsPerPage = 1;
            var page = 1;
            string currentUserId = "userId";
            var userAlbums = A.Fake<ServiceResponse<IPagedList<Album>>>();
            A.CallTo(() => _albumService.GetUserAlbums(currentUserId, page, albumsPerPage)).Returns(userAlbums);

            //Act
            var result =  _albumController.UserAlbums(page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AlbumController_CreateAlbumPost_ReturnsSuccess() 
        {
            //Arrange
            var albumVM = A.Fake<CreateAlbumVM>();
            var createAlbum = A.Fake<ServiceResponse<bool>>();
            A.CallTo(() => _albumService.CreateAlbum(albumVM)).Returns(createAlbum);

            //Act
            var result = _albumController.CreateAlbumPost(albumVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AlbumController_DeleteAlbum_ReturnsSuccess() 
        {
            //Arrange
            var albumId = 1;
            var albumToRemove = A.Fake<ServiceResponse<Album>>();
            A.CallTo(() => _albumService.GetAlbumByIdAsync(albumId)).Returns(albumToRemove);

            //Act
            var result = _albumController.DeleteAlbum(albumId);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AlbumController_DeleteAlbumPost_ReturnsSuccess()
        {
            //Arrange
            int albumId = 1;
            //Act

            var result = _albumController.DeleteAlbum(albumId);
            //Assert

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AlbumController_EditAlbum_ReturnsSuccess() 
        {
            //Arrange
            int albumId = 1;
            var albumToEdit = A.Fake<ServiceResponse<Album>>();
            A.CallTo(() => _albumService.GetAlbumByIdAsync(albumId)).Returns(albumToEdit);

            //Act
            var result = _albumController.EditAlbum(albumId);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AlbumController_EditAlbumPost_ReturnsSuccess() 
        {
            //Arrange
            var editAlbumVM = A.Fake<EditAlbumVM>();
            var isDeleted = A.Fake<ServiceResponse<bool>>();
            A.CallTo(() => _albumService.EditAlbum(editAlbumVM)).Returns(isDeleted);

            //Act
            var result = _albumController.EditAlbumPost(editAlbumVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
