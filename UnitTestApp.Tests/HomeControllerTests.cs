using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTestApp.Controllers;
using UnitTestApp.Models;
using Xunit;

namespace UnitTestApp.Tests
{
    public class HomeControllerTests
    {
        //sample1
        //[Fact]
        //public void IndexViewDataMessage()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.Equal("Hello world!", result?.ViewData["Message"]);
        //}

        //[Fact]
        //public void IndexViewResultNotNull()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();
        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;
        //    // Assert
        //    Assert.NotNull(result);
        //}

        //[Fact]
        //public void IndexViewNameEqualIndex()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();
        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;
        //    // Assert
        //    Assert.Equal("Index", result?.ViewName);
        //}

        //[Fact]
        //public void IndexTest()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.Equal("Hello world!", result?.ViewData["Message"]);
        //    Assert.NotNull(result);
        //    Assert.Equal("Index", result?.ViewName);
        //}

        //sample2
        //[Fact]
        //public void IndexReturnsAViewResultWithAListOfUsers()
        //{
        //    // Arrange
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(repo => repo.GetAll()).Returns(GetTestUsers());
        //    var controller = new HomeController(mock.Object);

        //    // Act
        //    var result = controller.Index();

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
        //    Assert.Equal(GetTestUsers().Count, model.Count());
        //}
        //private List<User> GetTestUsers()
        //{
        //    var users = new List<User>
        //    {
        //        new User { Id=1, Name="Tom", Age=35},
        //        new User { Id=2, Name="Alice", Age=29},
        //        new User { Id=3, Name="Sam", Age=32},
        //        new User { Id=4, Name="Kate", Age=30}
        //    };
        //    return users;
        //}

        //sample3
        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestUsers());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(
                viewResult.Model);
            Assert.Equal(GetTestUsers().Count, model.Count());
        }
        private List<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new User { Id=1, Name="Tom", Age=35},
                new User { Id=2, Name="Alice", Age=29},
                new User { Id=3, Name="Sam", Age=32},
                new User { Id=4, Name="Kate", Age=30}
            };
            return users;
        }
        [Fact]
        public void AddUserReturnsViewResultWithUserModel()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            User newUser = new User();

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult?.Model);
        }

        [Fact]
        public void AddUserReturnsARedirectAndAddsUser()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            var newUser = new User()
            {
                Name = "Ben"
            };

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Create(newUser));
        }

        [Fact]
        public void GetUserReturnsBadRequestResultWhenIdIsNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(null);

            // Arrange
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetUserReturnsNotFoundResultWhenUserNotFound()
        {
            // Arrange
            int testUserId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.Get(testUserId))
                .Returns(null as User);
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(testUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUserReturnsViewResultWithUser()
        {
            // Arrange
            int testUserId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.Get(testUserId))
                .Returns(GetTestUsers().FirstOrDefault(p => p.Id == testUserId));
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(testUserId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.ViewData.Model);
            Assert.Equal("Tom", model.Name);
            Assert.Equal(35, model.Age);
            Assert.Equal(testUserId, model.Id);
        }
    }
}
