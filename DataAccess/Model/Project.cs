using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    public class Project : BaseEntity
    {
        [Required, MaxLength(50)] 
        public string Name { get; set; }
    }
}