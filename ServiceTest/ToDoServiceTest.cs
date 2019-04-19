using System;
using System.Collections.Immutable;
using System.Data;
using DataAccess.Model;
using DataAccess.Repository;
using Moq;
using Service.ToDos;
using Xunit;

namespace ServiceTest
{
    public class ToDoServiceTest
    {
        private readonly IToDoService _toDoService;

        private readonly Mock<IToDoRepository> _toDoRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;

        private readonly ToDo toDo1 = new ToDo {Id = 1, Name = "t1"};
        private readonly ToDo toDo2 = new ToDo {Id = 2, Name = "t2"};

        private readonly User user = new User
            {Id = 1, Firstname = "f", Lastname = "l", Email = "u@te.st", Password = "123", Level = 1};

        private readonly Project project = new Project {Id = 1, Name = "project"};

        public ToDoServiceTest()
        {
            _toDoRepositoryMock = new Mock<IToDoRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _toDoService = new ToDoService(_toDoRepositoryMock.Object, _userRepositoryMock.Object,
                _projectRepositoryMock.Object);

            _toDoRepositoryMock.Setup(r => r.GetToDo(toDo1.Id)).Returns(toDo1);
            _toDoRepositoryMock.Setup(r => r.GetToDo(toDo2.Id)).Returns(toDo2);
            _userRepositoryMock.Setup(r => r.GetUser(user.Id)).Returns(user);
            _projectRepositoryMock.Setup(r => r.GetProject(project.Id)).Returns(project);
        }

        [Fact]
        public async void ShouldCreateToDo()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.GetToDos()).Returns(new[] {toDo1});
            _toDoRepositoryMock.Setup(r => r.InsertTodo(It.IsAny<ToDo>())).Verifiable();
            // when
            await _toDoService.Create(toDo2);
            // then
            _toDoRepositoryMock.Verify(r => r.InsertTodo(It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async void ShouldSetWorkingPeriod()
        {
            // given
            var start = DateTime.Parse("1/11/2001 13:00:00");
            var end = DateTime.Parse("1/11/2001 14:00:00");
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>())).Verifiable();
            // when
            await _toDoService.SetWorkingTime(toDo1.Id, start, end);
            // then
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotSetWorkingPeriodOnNotExistingToDo()
        {
            // given
            var start = DateTime.Parse("1/11/2001 13:00:00");
            var end = DateTime.Parse("1/11/2001 14:00:00");
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(It.IsAny<ToDo>(), It.IsAny<ToDo>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _toDoService.SetWorkingTime(8, start, end));
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(It.IsAny<ToDo>(), It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotSetInvalidWorkingPeriod()
        {
            // given
            var start = DateTime.Parse("1/11/2001 14:00:00");
            var end = DateTime.Parse("1/11/2001 13:00:00");
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _toDoService.SetWorkingTime(toDo1.Id, start, end));
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async void ShouldSetUser()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>())).Verifiable();
            // when
            await _toDoService.AssignToUser(toDo1.Id, user.Id);
            // then
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotSetUserOnNotExistingToDo()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(It.IsAny<ToDo>(), It.IsAny<ToDo>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _toDoService.AssignToUser(8, user.Id));
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(It.IsAny<ToDo>(), It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotSetNonExistingUser()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _toDoService.AssignToUser(toDo1.Id, 11));
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async void ShouldSetProject()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>())).Verifiable();
            // when
            await _toDoService.AssociateWithProject(toDo1.Id, project.Id);
            // then
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotSetProjectOnNotExistingToDo()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(It.IsAny<ToDo>(), It.IsAny<ToDo>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _toDoService.AssociateWithProject(8, project.Id));
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(It.IsAny<ToDo>(), It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotSetNonExistingProject()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _toDoService.AssociateWithProject(toDo1.Id, 11));
            _toDoRepositoryMock.Verify(r => r.UpdateTodo(toDo1, It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async void ShouldDeleteToDo()
        {
            // given
            _toDoRepositoryMock.Setup(r => r.DeleteTodo(toDo2.Id)).Verifiable();
            // when
            var deleted = await _toDoService.Delete(toDo2.Id);
            // then
            Assert.True(deleted);
            _toDoRepositoryMock.Verify(r => r.DeleteTodo(toDo2.Id), Times.Once);
        }

        [Fact]
        public async void ShouldNotDeleteNonExistingToDo()
        {
            // given
            const int falseId = 88;
            _toDoRepositoryMock.Setup(r => r.DeleteTodo(falseId)).Verifiable();
            // when
            var deleted = await _toDoService.Delete(falseId);
            // then
            Assert.False(deleted);
            _toDoRepositoryMock.Verify(r => r.DeleteTodo(falseId), Times.Never);
        }
    }
}