using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialPhotoAppMVC.Controllers;
using SocialPhotoAppMVC.Services.UserService;
using SocialPhotoAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserService _userService;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            //Dependencies
            _httpContext = A.Fake<IHttpContextAccessor>();
            _userService = A.Fake<IUserService>();

            //System under test
            _userController = new UserController(_httpContext, _userService);
        }

        [Fact]
        public void UserController_ListUsers_ReturnsSuccess()
        {
            //Arrange
            int page = 1;
            var allUsers = A.Fake<ServiceResponse<IPagedList<AppUser>>>();
            A.CallTo(() => _userService.GetAllUsers(page)).Returns(allUsers);

            //Act
            var result = _userController.ListUsers(page);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void UserController_UserProfile_ReturnsSuccess()
        {
            //Arrange
            string userId = "";
            var photosPage = 1;
            var albumsPage = 1;
            var photosPerPage = 1;
            var albumsPerPage = 1;
            var userProfile = A.Fake<ServiceResponse<AppUserProfileDTO>>();
            A.CallTo(() => _userService.GetUserProfile(userId, photosPage, albumsPage, photosPerPage, albumsPerPage))
                .Returns(userProfile);

            //Act
            var result = _userController.UserProfile(userId, photosPage, albumsPage);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void UserController_ChangeNickName_ReturnsSuccess()
        {
            //Arrange

            //Act
            var result = _userController.ChangeNickname();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void UserController_ChangeNickNamePost_ReturnsSuccess()
        {
            //Arrange
            var newNick = A.Fake<ChangeNicknameVM>();

            //Act
            var result = _userController.ChangeNickname(newNick);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
