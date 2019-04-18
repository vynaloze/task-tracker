using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;

namespace Service.Associations
{
    public class AssociationService : IAssociationService
    {
        private readonly IAssociationRepository _associationRepository;
        private readonly IToDoRepository _toDoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public AssociationService(IAssociationRepository associationRepository, IToDoRepository doRepository,
            IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _associationRepository = associationRepository;
            _toDoRepository = doRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Association>> Get()
        {
            return await Task.Run(() => _associationRepository.GetAssociations());
        }

        public async Task<Association> Get(int todoId)
        {
            return await Task.Run(() => _associationRepository.GetAssociation(todoId));
        }

        public async Task<Association> Create(int todoId, int? projectId, int? userId)
        {
            return await Task.Run(() =>
            {
                var existsDuplicate = _associationRepository.GetAssociations().Any(a => a.ToDo.Id == todoId);
                if (existsDuplicate)
                {
                    throw new DuplicateNameException("Such ToDo is already assigned.");
                }

                var newAssociation = new Association();
                var task = _toDoRepository.GetToDo(todoId);
                if (task == null)
                {
                    throw new ArgumentException("ToDo not found");
                }

                newAssociation.ToDo = task;

                if (userId.HasValue)
                {
                    var dbUser = _userRepository.GetUser(userId.Value);
                    if (dbUser == null)
                    {
                        throw new ArgumentException("User not found");
                    }

                    newAssociation.User = dbUser;
                }

                if (projectId.HasValue)
                {
                    var dbProject = _projectRepository.GetProject(projectId.Value);
                    if (dbProject == null)
                    {
                        throw new ArgumentException("Project not found");
                    }

                    newAssociation.Project = dbProject;
                }

                _associationRepository.InsertAssociation(newAssociation);
                return newAssociation;
            });
        }

        public async Task<bool> Update(int todoId, int? projectId, int? userId)
        {
            return await Task.Run(() =>
            {
                var oldAssociation = _associationRepository.GetAssociations().FirstOrDefault(a => a.ToDo.Id == todoId);
                if (oldAssociation == null)
                {
                    throw new ArgumentException("Association with such toDo not found");
                }

                var newAssociation = new Association{ToDo = oldAssociation.ToDo};
                if (userId.HasValue)
                {
                    var dbUser = _userRepository.GetUser(userId.Value);
                    if (dbUser == null)
                    {
                        throw new ArgumentException("User not found");
                    }

                    newAssociation.User = dbUser;
                }

                if (projectId.HasValue)
                {
                    var dbProject = _projectRepository.GetProject(projectId.Value);
                    if (dbProject == null)
                    {
                        throw new ArgumentException("Project not found");
                    }

                    newAssociation.Project = dbProject;
                }

                _associationRepository.UpdateAssociation(oldAssociation, newAssociation);

                return true;
            });
        }

        public async Task<bool> Delete(int todoId)
        {
            return await Task.Run(() =>
            {
                var oldAssociation = _associationRepository.GetAssociations().FirstOrDefault(a => a.ToDo.Id == todoId);
                if (oldAssociation == null)
                {
                    throw new ArgumentException();
                }

                _associationRepository.DeleteAssociation(oldAssociation.Id);
                return true;
            });
        }
    }
}