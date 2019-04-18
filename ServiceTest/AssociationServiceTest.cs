using System;
using System.Collections.Immutable;
using System.Data;
using DataAccess.Model;
using DataAccess.Repository;
using Moq;
using Service.Associations;
using Xunit;

namespace ServiceTest
{
    public class AssociationServiceTest
    {
        private readonly IAssociationService _associationService;

        private readonly Mock<IAssociationRepository> _associationRepositoryMock;
        private readonly Mock<IToDoRepository> _toDoRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;

        private readonly ToDo toDo1 = new ToDo {Id = 1, Name = "t1"};
        private readonly ToDo toDo2 = new ToDo {Id = 2, Name = "t2"};

        private readonly User user = new User
            {Id = 1, Firstname = "f", Lastname = "l", Email = "u@te.st", Password = "123", Level = 1};

        private readonly Project project = new Project {Id = 1, Name = "project"};

        public AssociationServiceTest()
        {
            _associationRepositoryMock = new Mock<IAssociationRepository>();
            _toDoRepositoryMock = new Mock<IToDoRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _associationService = new AssociationService(_associationRepositoryMock.Object, _toDoRepositoryMock.Object,
                _userRepositoryMock.Object, _projectRepositoryMock.Object);

            _toDoRepositoryMock.Setup(r => r.GetToDo(toDo1.Id)).Returns(toDo1);
            _toDoRepositoryMock.Setup(r => r.GetToDo(toDo2.Id)).Returns(toDo2);
            _userRepositoryMock.Setup(r => r.GetUser(user.Id)).Returns(user);
            _projectRepositoryMock.Setup(r => r.GetProject(project.Id)).Returns(project);
        }

        [Fact]
        public async void ShouldCreateBareAssociation()
        {
            // given
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(ImmutableList<Association>.Empty);
            _associationRepositoryMock.Setup(r => r.InsertAssociation(It.IsAny<Association>())).Verifiable();
            // when
            await _associationService.Create(toDo1.Id, null, null);
            // then
            _associationRepositoryMock.Verify(r => r.InsertAssociation(It.IsAny<Association>()), Times.Once);
        }

        [Fact]
        public async void ShouldCreateAssociationWithOptionalData()
        {
            // given
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(ImmutableList<Association>.Empty);
            _associationRepositoryMock.Setup(r => r.InsertAssociation(It.IsAny<Association>())).Verifiable();
            // when
            await _associationService.Create(toDo1.Id, project.Id, user.Id);
            // then
            _associationRepositoryMock.Verify(r => r.InsertAssociation(It.IsAny<Association>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotCreateDuplicateAssociation()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = project, User = user};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.InsertAssociation(It.IsAny<Association>())).Verifiable();
            // then
            await Assert.ThrowsAsync<DuplicateNameException>(async () =>
                await _associationService.Create(toDo1.Id, null, null));
            _associationRepositoryMock.Verify(r => r.InsertAssociation(It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotCreateAssociationForNonExistingToDo()
        {
            // given
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(ImmutableList<Association>.Empty);
            _associationRepositoryMock.Setup(r => r.InsertAssociation(It.IsAny<Association>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Create(5, null, null));
            _associationRepositoryMock.Verify(r => r.InsertAssociation(It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotCreateAssociationForNonExistingUser()
        {
            // given
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(ImmutableList<Association>.Empty);
            _associationRepositoryMock.Setup(r => r.InsertAssociation(It.IsAny<Association>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Create(toDo1.Id, null, 8));
            _associationRepositoryMock.Verify(r => r.InsertAssociation(It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotCreateAssociationForNonExistingProject()
        {
            // given
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(ImmutableList<Association>.Empty);
            _associationRepositoryMock.Setup(r => r.InsertAssociation(It.IsAny<Association>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Create(toDo1.Id, 42, null));
            _associationRepositoryMock.Verify(r => r.InsertAssociation(It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldUpdateAssociation()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = null, User = null};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()))
                .Verifiable();
            // when
            await _associationService.Update(toDo1.Id, project.Id, user.Id);
            // then
            _associationRepositoryMock.Verify(
                r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotUpdateAssociationForNonExistingToDo()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = null, User = null};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()))
                .Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Update(5, null, null));
            _associationRepositoryMock.Verify(
                r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotUpdateAssociationForNonExistingUser()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = null, User = null};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()))
                .Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Update(toDo1.Id, 8, null));
            _associationRepositoryMock.Verify(
                r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldNotUpdateAssociationForNonExistingProject()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = null, User = null};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()))
                .Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Update(toDo1.Id, null, 7));
            _associationRepositoryMock.Verify(
                r => r.UpdateAssociation(It.IsAny<Association>(), It.IsAny<Association>()), Times.Never);
        }

        [Fact]
        public async void ShouldDeleteAssociation()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = null, User = null};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.DeleteAssociation(It.IsAny<int>())).Verifiable();
            // when
            await _associationService.Delete(toDo1.Id);
            // then
            _associationRepositoryMock.Verify(
                r => r.DeleteAssociation(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotDeleteAssociationForNonExistingToDo()
        {
            // given
            var association = new Association {Id = 1, ToDo = toDo1, Project = null, User = null};
            _associationRepositoryMock.Setup(r => r.GetAssociations()).Returns(new[] {association});
            _associationRepositoryMock.Setup(r => r.DeleteAssociation(It.IsAny<int>())).Verifiable();
            // then
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _associationService.Delete(5));
            _associationRepositoryMock.Verify(
                r => r.DeleteAssociation(It.IsAny<int>()), Times.Never);
        }
    }
}