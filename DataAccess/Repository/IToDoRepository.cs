using System.Collections.Generic;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public interface IToDoRepository
    {
        IEnumerable<ToDo> GetToDos();
        ToDo GetToDo(int id);
        void InsertTodo(ToDo toDo);
        void DeleteTodo(int id);
        void UpdateTodo(ToDo oldToDo, ToDo newToDo);
        void Save();
    }
}