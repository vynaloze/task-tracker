using System;
using DataAccess.Model;
using DataAccess.Repository;
using Moq;
using Service.Users;
using Xunit;

namespace ServiceTest
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserService _userService;
        private readonly User u1 = new User
            {Id=1, Firstname = "u1", Lastname = "l1", Email = "u1@te.st", Password = "123", Level = 1};
        private readonly User  u2 = new User
            {Id=2, Firstname = "u2", Lastname = "l2", Email = "u2@te.st", Password = "123", Level = 1};
        private readonly User  u3 = new User
            {Id=3, Firstname = "u3", Lastname = "l3", Email = "u3@te.st", Password = "123", Level = 1};

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async void ShouldAuthenticateCorrectUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(() => new[] {u1, u2, u3});
            // when
            var result = await _userService.Authenticate(u2.Email, u2.Password);
            // then
            Assert.NotNull(result);
        }
        
        [Fact]
        public async void ShouldNotAuthenticateWrongUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(() => new[] {u1, u2, u3});
            // when
            var result = await _userService.Authenticate("foo@b.ar", "321");
            // then
            Assert.Null(result);
        }
        
        [Fact]
        public async void ShouldRegisterUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(() => new[] {u1, u2});
            _userRepositoryMock.Setup(r => r.InsertUser(u3)).Verifiable();
            // when
            var result = await _userService.RegisterUser(u3);
            // then
            Assert.NotNull(result);
            _userRepositoryMock.Verify(r => r.InsertUser(u3), Times.Once);
        }
        
        [Fact]
        public async void ShouldNotRegisterDuplicateUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(() => new[] {u1, u2});
            _userRepositoryMock.Setup(r => r.InsertUser(u2)).Verifiable();
            // when
            var result = await _userService.RegisterUser(u2);
            // then
            Assert.Null(result);
            _userRepositoryMock.Verify(r => r.InsertUser(u2), Times.Never);
        }
        
        [Fact]
        public async void ShouldUpdateUserData()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUser(u1.Id)).Returns(() => u1);
            _userRepositoryMock.Setup(r => r.GetUser(u2.Id)).Returns(() => null);
            _userRepositoryMock.Setup(r => r.UpdateUser(u1, u1)).Verifiable();
            // when
            var result = await _userService.SaveUserData(u1.Id, u1);
            // then
            Assert.True(result);
            _userRepositoryMock.Verify(r => r.UpdateUser(u1, u1), Times.Once);
        }
        
        [Fact]
        public async void ShouldNotUpdateNotExistentUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUser(u1.Id)).Returns(() => u1);
            _userRepositoryMock.Setup(r => r.GetUser(u2.Id)).Returns(() => null);
            _userRepositoryMock.Setup(r => r.UpdateUser(u2, u2)).Verifiable();
            // when
            var result = await _userService.SaveUserData(u2.Id, u2);
            // then
            Assert.False(result);
            _userRepositoryMock.Verify(r => r.UpdateUser(u2, u2), Times.Never);
        }
        [Fact]
        public async void ShouldDeleteUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUser(u1.Id)).Returns(() => u1);
            _userRepositoryMock.Setup(r => r.GetUser(u2.Id)).Returns(() => null);
            _userRepositoryMock.Setup(r => r.DeleteUser(u1.Id)).Verifiable();
            // when
            var result = await _userService.DeleteUser(u1.Id);
            // then
            Assert.True(result);
            _userRepositoryMock.Verify(r => r.DeleteUser(u1.Id), Times.Once);
        }
        
        [Fact]
        public async void ShouldNotDeleteNotExistentUser()
        {
            // given
            _userRepositoryMock.Setup(r => r.GetUser(u1.Id)).Returns(() => u1);
            _userRepositoryMock.Setup(r => r.GetUser(u2.Id)).Returns(() => null);
            _userRepositoryMock.Setup(r => r.DeleteUser(u2.Id)).Verifiable();
            // when
            var result = await _userService.DeleteUser(u2.Id);
            // then
            Assert.False(result);
            _userRepositoryMock.Verify(r => r.DeleteUser(u2.Id), Times.Never);
        }
    }
}