using System.Collections.Generic;
using System.Linq;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class AssociationRepository : IAssociationRepository
    {
        private readonly DataContext _context;

        public AssociationRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<Association> GetAssociations()
        {
            return _context.Associations
                .Include(a => a.Task)
                .Include(a => a.Project)
                .Include(a => a.User)
                .ToList();
        }

        public Association GetAssociation(int id)
        {
            return _context.Associations
                .Include(a => a.Task)
                .Include(a => a.Project)
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);
        }

        public void InsertAssociation(Association association)
        {
            _context.Associations.Add(association);
            Save();
        }

        public void DeleteAssociation(int id)
        {
            var association = _context.Associations.Find(id);
            _context.Associations.Remove(association);
            Save();
        }

        public void UpdateAssociation(Association oldAssociation, Association newAssociation)
        {
            oldAssociation.Task = newAssociation.Task;
            oldAssociation.Project = newAssociation.Project;
            oldAssociation.User = newAssociation.User;
            
            _context.Associations.Update(oldAssociation);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}