using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
    public class ToDo : BaseEntity
    {
        [Required, MaxLength(50)] 
        public string Name { get; set; }
        
        public DateTime ?StartTime { get; set; }
        
        public DateTime ?EndTime { get; set; }
        
        public User User { get; set; }
        
        public Project Project { get; set; }

        public ToDo Clone()
        {
            return new ToDo
            {
                Id = this.Id, Name = this.Name, User = this.User, Project = this.Project, StartTime = this.StartTime,
                EndTime = this.EndTime
            };
        }
    }
}