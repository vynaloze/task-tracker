using System.Collections.Generic;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects();
        Project GetProject(int id);
        void InsertProject(Project project);
        void DeleteProject(int id);
        void UpdateProject(Project oldProject, Project newProject);
        void Save();
    }
}