using System.Collections.Generic;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetTasks();
        Task GetTask(int id);
        void InsertTask(Task task);
        void DeleteTask(int id);
        void UpdateTask(Task oldTask, Task newTask);
        void Save();
    }
}