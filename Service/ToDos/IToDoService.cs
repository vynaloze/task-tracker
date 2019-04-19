using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;

namespace Service.ToDos
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDo>> Get();
        Task<ToDo> Get(int id);
        Task<ToDo> Create(ToDo toDo);
        Task<bool> Update(ToDo oldToDo, ToDo newToDo);
        Task<bool> Delete(int id);
        Task<bool> SetWorkingTime(int toDoId, DateTime start, DateTime end);
        Task<bool> AssociateWithProject(int toDoId, int? projectId);
        Task<bool> AssignToUser(int toDoId, int? userId);
    }
}