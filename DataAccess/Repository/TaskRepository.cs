using System.Collections.Generic;
using System.Linq;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public class TaskRepository: ITaskRepository
    {
        private readonly DataContext _context;

        public TaskRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<Task> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        public Task GetTask(int id)
        {
            return _context.Tasks.Find(id);
        }

        public void InsertTask(Task task)
        {
            _context.Tasks.Add(task);
            Save();
        }

        public void DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            _context.Tasks.Remove(task);
            Save();
        }

        public void UpdateTask(Task oldTask, Task newTask)
        {
            oldTask.Name = newTask.Name;
            oldTask.StartTime = newTask.StartTime;
            oldTask.EndTime = newTask.EndTime;
            
            _context.Tasks.Update(oldTask);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}