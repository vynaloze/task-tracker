using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    public class User : BaseEntity
    {
        [Required, MaxLength(50)] 
        public string Firstname { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string Lastname { get; set; }

        [Required, EmailAddress] 
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }
        
        [Range(1, 5)] 
        public int Level { get; set; }
    }
}