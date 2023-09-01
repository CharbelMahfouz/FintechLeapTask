using BLL.ViewModels;
using DAL.Data;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Xunit;
using BLL.Service;
using Moq;
using BLL.Utilities.Extensions;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Castle.DynamicProxy;
using DAL.Models;
using DAL.Services;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    public class UnitTest1
    {
        #region Auth

       
        [Fact]
        public async Task EmailSignIn_CorrectCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Secret"]).Returns("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var signInManager = new SignInManager<ApplicationUser>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
               Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object,configurationMock.Object);

            var model = new EmailSignIn_VM
            {
                Email = "test@example.com",
                Password = "password"
            };
            var request = new Mock<HttpRequest>().Object;

            // Act
            var result = await authBO.EmailSignIn(model, request);

            // Assert
            Assert.Equal(1, result.Result);
            Assert.Equal("Sign in successful", result.Message);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task EmailSignIn_IncorrectCredentials_ReturnsFailedResponse()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Secret"]).Returns("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var signInManager = new SignInManager<ApplicationUser>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
               Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object,configurationMock.Object);

            var model = new EmailSignIn_VM
            {
                Email = "test@example.com",
                Password = "password"
            };
            var request = new Mock<HttpRequest>().Object;

            // Act
            var result = await authBO.EmailSignIn(model, request);

            // Assert
            Assert.Equal(0, result.Result);
            Assert.Equal("Incorrect email/password combination", result.Message);
        }

        [Fact]
        public async Task EmailSignUp_UsedEmail_ReturnsFailedResponse()
        {
            // Arrange
           
            var model = new EmailSignUp_VM
            {
                Email = "test@example.com",
                Password = "password",
                CountryCode = "+961",
                PhoneNumber = "12345678",
                FullName = "Charbel Mahofouz",

            };
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var signInManager = new SignInManager<ApplicationUser>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
               Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success); // Simulate successful user creation
            userManagerMock.Setup(um => um.FindByEmailAsync(model.Email))
                          .ReturnsAsync(new ApplicationUser());
            var clientProfileRepositoryMock = new Mock<IClientProfileRepository>();
            clientProfileRepositoryMock.Setup(repo => repo.Create(It.IsAny<ClientProfile>()))
                                      .ReturnsAsync((ClientProfile profile) => profile);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.ClientProfileRepository)
                          .Returns(clientProfileRepositoryMock.Object);
            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object);

           
        

            // Act
            var result = await authBO.EmailSignUp(model);

            // Assert
            Assert.Equal(0, result.Result);
            Assert.Equal("Email is already taken", result.Message);
        }

        [Fact]
        public async Task EmailSignUp_ValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                         .ReturnsAsync(IdentityResult.Success);
            var signInManager = new SignInManager<ApplicationUser>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
               Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var clientProfileRepositoryMock = new Mock<IClientProfileRepository>();
            clientProfileRepositoryMock.Setup(repo => repo.Create(It.IsAny<ClientProfile>()))
                                      .ReturnsAsync((ClientProfile profile) => profile);
            unitOfWorkMock.Setup(uow => uow.ClientProfileRepository)
                         .Returns(clientProfileRepositoryMock.Object);
            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object);

            var model = new EmailSignUp_VM
            {
                Email = "test@example.com",
                Password = "password",
                CountryCode = "+961",
                PhoneNumber = "12345678",
                FullName = "Charbel Mahofouz",

            };
            var request = new Mock<HttpRequest>().Object;

            // Act
            var result = await authBO.EmailSignUp(model);

            // Assert
            Assert.Equal(1, result.Result);
            Assert.Equal("Account created successfully", result.Message);
        }

        #endregion
        #region UpdateProfile
        [Fact]
        public async Task UpdateProfile_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
           
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var signInManager = new SignInManager<ApplicationUser>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
               Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync((ApplicationUser)null); // Return null for user
            var userRepositoryMock = new Mock<IUserRepos>();
            userRepositoryMock.Setup(repo => repo.CheckIfExists(It.IsAny<Expression<Func<AspNetUser, bool>>>()))
                              .ReturnsAsync(false);
            unitOfWorkMock.Setup(uow => uow.UserRepos)
                 .Returns(userRepositoryMock.Object);
            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object);

            var model = new UpdateProfile_VM();
            var uid = ""; // Non-existent user ID
            var request = new Mock<HttpRequest>().Object;

            // Act
            var result = await authBO.UpdateProfile(model, uid, request);

            // Assert
            Assert.Equal(0, result.Result);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task UpdateProfile_EmailExists_ReturnsNewEmailTakenResponse()
        {
            // Arrange

            var model = new UpdateProfile_VM
            {
                Email = "existing@example.com", // Existing email
                // Set other properties as needed
            };
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
              Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync(new ApplicationUser()); // Return a user for the specified UID

            userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                           .ReturnsAsync(new ApplicationUser()); // Return a user for the specified email
            var signInManager = new SignInManager<ApplicationUser>(
               userManagerMock.Object,
               Mock.Of<IHttpContextAccessor>(),
               Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
               null, null, null, null);
            var userRepositoryMock = new Mock<IUserRepos>();
            userRepositoryMock.Setup(repo => repo.CheckIfExists(It.IsAny<Expression<Func<AspNetUser, bool>>>()))
                              .ReturnsAsync(true);
            unitOfWorkMock.Setup(uow => uow.UserRepos)
                 .Returns(userRepositoryMock.Object);

            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object);


            var uid = "user-id"; // Existing user ID
            var request = new Mock<HttpRequest>().Object;

            // Act
            var result = await authBO.UpdateProfile(model, uid, request);

            // Assert
            Assert.Equal(0, result.Result);
            Assert.Equal("New email is taken", result.Message);
        }

        [Fact]
        public async Task UpdateProfile_ValidModel_ReturnsSuccessResponse()
        {
            // Arrange
            var userManagerMock = new Mock<CustomUserStore>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManager = new SignInManager<ApplicationUser>(
              userManagerMock.Object,
              Mock.Of<IHttpContextAccessor>(),
              Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
              null, null, null, null);
            userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync(new ApplicationUser()); // Return a user for the specified UID
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepos>();
            userRepositoryMock.Setup(repo => repo.CheckIfExists(It.IsAny<Expression<Func<AspNetUser, bool>>>()))
                              .ReturnsAsync(true);
            unitOfWorkMock.Setup(uow => uow.UserRepos)
                 .Returns(userRepositoryMock.Object);
            var authBO = new AuthBO(unitOfWorkMock.Object, userManagerMock.Object, signInManager, null, roleManagerMock.Object);

            var model = new UpdateProfile_VM
            {
                Email = "newemail@example.com", // New email
                // Set other properties as needed
            };
            var uid = "user-id"; // Existing user ID
            var request = new Mock<HttpRequest>().Object;

            // Act
            var result = await authBO.UpdateProfile(model, uid, request);

            // Assert
            Assert.Equal(1, result.Result);
            Assert.Equal("Profile updated successfully", result.Message);
        }
        #endregion
      

    }
}