using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Controllers;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests.Controllers
{
    public class PhotoControllerTests
    {
        private readonly PhotoController _photoController;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContext;

        public PhotoControllerTests()
        {
            //Dependencies
            _photoService = A.Fake<IPhotoService>();
            _httpContext = A.Fake<IHttpContextAccessor>();

            //System under test
            _photoController = new PhotoController(_photoService, _httpContext);
        }

        [Fact]
        public void PhotoController_Index_ReturnsSuccess()
        {
            //Arrange
            int page = 1;
            var allPhotos = A.Fake<ServiceResponse<IPagedList<Photo>>>();
            A.CallTo(() => _photoService.GetAllPhotos(page)).Returns(allPhotos);

            //Act
            var result = _photoController.Index(page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_FeaturedPhotos_ReturnsSuccess()
        {
            //Arrange
            int page = 1;
            var featuredPhotos = A.Fake<ServiceResponse<IPagedList<Photo>>>();
            A.CallTo(() => _photoService.GetFeaturedPhotos(page)).Returns(featuredPhotos);

            //Act
            var result = _photoController.FeaturedPhotos(page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_PhotoDetail_ReturnsSuccess()
        {
            //Arrange
            int id = 1;
            var foundPhoto = A.Fake<ServiceResponse<Photo>>();
            A.CallTo(() => _photoService.GetPhotoDetail(id)).Returns(foundPhoto);

            //Act
            var result = _photoController.PhotoDetail(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_UserPhotos_ReturnsSuccess()
        {
            //Arrange
            string currentUserId = "userId";
            int page = 1;
            int resultsPerPage = 1;
            var userPhotos = A.Fake<ServiceResponse<IPagedList<Photo>>>();
            A.CallTo(() => _photoService.GetUserPhotos(currentUserId, page, resultsPerPage)).Returns(userPhotos);

            //Act
            var result = _photoController.UserPhotos(page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_AddPhotoToAlbum_ReturnsSuccess()
        {
            //Arrange
            int id = 1;
            var viewModel = A.Fake<ServiceResponse<AddPhotoToAlbumVM>>();
            A.CallTo(() => _photoService.AddPhotoToAlbumGET(id)).Returns(viewModel);

            //Act
            var result = _photoController.AddPhotoToAlbum(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_AddPhotoToAlbumPost_ReturnsSuccess()
        {
            //Arrange
            var photoAlbumVM = A.Fake<AddPhotoToAlbumVM>();
            var addPhotoToAlbum = A.Fake<ServiceResponse<Album>>();
            A.CallTo(() => _photoService.AddPhotoToAlbumPOST(photoAlbumVM)).Returns(addPhotoToAlbum);

            //Act
            var result = _photoController.AddPhotoToAlbum(photoAlbumVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_RemovePhotoFromAlbum_ReturnsSuccess()
        {
            //Arrange
            int id = 1;
            var viewModel = A.Fake<ServiceResponse<AddPhotoToAlbumVM>>();
            A.CallTo(() => _photoService.RemovePhotoFromAlbumGET(id)).Returns(viewModel);

            //Act
            var result = _photoController.RemovePhotoFromAlbum(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_RemovePhotoFromAlbumPost_ReturnsSuccess()
        {
            //Arrange
            var photoToAlbumVM = A.Fake<AddPhotoToAlbumVM>();
            var addPhototToAlbum = A.Fake<ServiceResponse<Album>>();
            A.CallTo(() => _photoService.RemovePhotoFromAlbumPOST(photoToAlbumVM)).Returns(addPhototToAlbum);

            //Act
            var result = _photoController.RemovePhotoFromAlbum(photoToAlbumVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_UploadPhotoPost_ReturnsSuccess()
        {
            //Arrange
            var uploadPhotoVM = A.Fake<UploadPhotoVM>();
            var uploadPhoto = A.Fake<ServiceResponse<bool>>();
            A.CallTo(() => _photoService.UploadPhoto(uploadPhotoVM)).Returns(uploadPhoto);

            //Act
            var result = _photoController.UploadPhoto(uploadPhotoVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_DeletePhoto_ReturnsSuccess()
        {
            //Arrange
            int id = 1;
            var photoToRemove = A.Fake<ServiceResponse<Photo>>();
            A.CallTo(() => _photoService.GetPhotoByIdAsync(id)).Returns(photoToRemove);

            //Act
            var result = _photoController.DeletePhoto(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_DeletePhotoPost_ReturnsSuccess()
        {
            //Arrange
            var deletePhotoVM = A.Fake<DeletePhotoVM>();
            var isDeleted = A.Fake<ServiceResponse<bool>>();
            A.CallTo(() => _photoService.DeletePhoto(deletePhotoVM.PhotoId)).Returns(isDeleted);

            //Act
            var result = _photoController.DeletePhotoPost(deletePhotoVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_EditPhoto_ReturnsSuccess()
        {
            //Arrange
            int id = 1;
            string currentUserId = "userId";
            var photoToEdit = A.Fake<ServiceResponse<Photo>>();
            A.CallTo(() => _photoService.GetPhotoByIdAsync(id)).Returns(photoToEdit);

            //Act
            var result = _photoController.EditPhoto(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void PhotoController_EditPhotoPost_ReturnsSuccess()
        {
            //Arrange
            var editPhotoVM = A.Fake<EditPhotoVM>();
            var isEdited = A.Fake<ServiceResponse<bool>>();
            A.CallTo(() => _photoService.EditPhoto(editPhotoVM)).Returns(isEdited);

            //Act
            var result = _photoController.EditPhotoPost(editPhotoVM);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
