using System.Collections.Generic;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public interface IAssociationRepository
    {
        IEnumerable<Association> GetAssociations();
        Association GetAssociation(int id);
        void InsertAssociation(Association association);
        void DeleteAssociation(int id);
        void UpdateAssociation(Association oldAssociation, Association newAssociation);
        void Save();
    }
}