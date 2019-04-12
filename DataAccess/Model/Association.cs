using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    public class Association : BaseEntity
    {
        [Required]
        public Task Task { get; set; }
        
        public Project Project { get; set; }
        
        public User User { get; set; }
        
    }
}