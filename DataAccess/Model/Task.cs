using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    public class Task : BaseEntity
    {
        [Required, MaxLength(50)] 
        public string Name { get; set; }
        
        public User AssignedUser { get; set; }
        
        public Project AssignedToProject { get; set; }
    }
}