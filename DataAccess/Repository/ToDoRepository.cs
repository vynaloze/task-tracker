using System.Collections.Generic;
using System.Linq;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public class ToDoRepository: IToDoRepository
    {
        private readonly DataContext _context;

        public ToDoRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<ToDo> GetToDos()
        {
            return _context.Tasks.ToList();
        }

        public ToDo GetToDo(int id)
        {
            return _context.Tasks.Find(id);
        }

        public void InsertTodo(ToDo toDo)
        {
            _context.Tasks.Add(toDo);
            Save();
        }

        public void DeleteTodo(int id)
        {
            var task = _context.Tasks.Find(id);
            _context.Tasks.Remove(task);
            Save();
        }

        public void UpdateTodo(ToDo oldToDo, ToDo newToDo)
        {
            oldToDo.Name = newToDo.Name;
            oldToDo.StartTime = newToDo.StartTime;
            oldToDo.EndTime = newToDo.EndTime;
            
            _context.Tasks.Update(oldToDo);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}