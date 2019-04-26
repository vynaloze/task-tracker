using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;

namespace Service.ToDos
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public ToDoService(IToDoRepository doRepository, IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _toDoRepository = doRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ToDo>> Get()
        {
            return await Task.Run(() =>
                _toDoRepository.GetToDos()
                    .Select(toDo =>
                    {
                        if (toDo?.User != null)
                        {
                            toDo.User = HideSensitiveData(toDo.User);
                        }

                        return toDo;
                    })
                    .ToList()
            );
        }

        public async Task<ToDo> Get(int id)
        {
            return await Task.Run(() =>
            {
                var toDo = _toDoRepository.GetToDo(id);
                if (toDo?.User != null)
                {
                    toDo.User = HideSensitiveData(toDo.User);
                }

                return toDo;
            });
        }

        public async Task<ToDo> Create(ToDo toDo)
        {
            await Task.Run(() => _toDoRepository.InsertTodo(toDo));
            return toDo;
        }

        public async Task<bool> Update(ToDo oldToDo, ToDo newToDo)
        {
            await Task.Run(() => _toDoRepository.UpdateTodo(oldToDo, newToDo));
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var toDo = await Get(id);
            if (toDo == null)
            {
                return false;
            }

            await Task.Run(() => _toDoRepository.DeleteTodo(id));
            return true;
        }

        public async Task<bool> SetWorkingTime(int toDoId, DateTime start, DateTime end)
        {
            var toDo = await Get(toDoId);
            if (toDo == null)
            {
                throw new KeyNotFoundException("ToDo with Id " + toDoId + " not found");
            }

            if (start > end)
            {
                throw new ArgumentException("End is before start");
            }

            var newToDo = toDo.Clone();
            newToDo.StartTime = start;
            newToDo.EndTime = end;

            await Task.Run(() => _toDoRepository.UpdateTodo(toDo, newToDo));
            return true;
        }

        public async Task<bool> AssociateWithProject(int toDoId, int? projectId)
        {
            var toDo = await Get(toDoId);
            if (toDo == null)
            {
                throw new KeyNotFoundException("ToDo with Id " + toDoId + " not found");
            }

            var newToDo = toDo.Clone();
            if (projectId.HasValue)
            {
                var dbProject = _projectRepository.GetProject(projectId.Value);
                if (dbProject == null)
                {
                    throw new KeyNotFoundException("Project with Id " + projectId.Value + " not found");
                }

                newToDo.Project = dbProject;
            }
            else
            {
                newToDo.Project = null;
            }

            await Task.Run(() => _toDoRepository.UpdateTodo(toDo, newToDo));
            return true;
        }

        public async Task<bool> AssignToUser(int toDoId, int? userId)
        {
            var toDo = await Get(toDoId);
            if (toDo == null)
            {
                throw new KeyNotFoundException("ToDo with Id " + toDoId + " not found");
            }

            var newToDo = toDo.Clone();
            if (userId.HasValue)
            {
                var dbUser = _userRepository.GetUser(userId.Value);
                if (dbUser == null)
                {
                    throw new KeyNotFoundException("User with Id " + userId.Value + " not found");
                }

                newToDo.User = dbUser;
            }
            else
            {
                newToDo.User = null;
            }

            await Task.Run(() => _toDoRepository.UpdateTodo(toDo, newToDo));
            return true;
        }

        private User HideSensitiveData(User user)
        {
            var result = user.Clone();
            result.Password = "#####";
            result.Level = -999;
            result.ResetPasswordToken = "#####";
            return result;
        }
    }
}