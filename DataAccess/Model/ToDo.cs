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
    }
}