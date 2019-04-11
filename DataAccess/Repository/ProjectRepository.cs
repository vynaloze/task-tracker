using System.Collections.Generic;
using System.Linq;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;

        public ProjectRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects.ToList();
        }

        public Project GetProject(int id)
        {
            return _context.Projects.Find(id);
        }

        public void InsertProject(Project project)
        {
            _context.Projects.Add(project);
            Save();
        }

        public void DeleteProject(int id)
        {
            var project = _context.Projects.Find(id);
            _context.Projects.Remove(project);
            Save();
        }

        public void UpdateProject(Project oldProject, Project newProject)
        {
            oldProject.Name = newProject.Name;
            
            _context.Projects.Update(oldProject);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}