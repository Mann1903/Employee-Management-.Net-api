using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EmployeeCrud_API.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public string? Gender { get; set; }
        public string? Position { get; set;} 
        public int? IsActive { get; set; }
        public string? ImageName { get; set; }
        //[NotMapped]
        //public IFormFile? ProfileImage { get; set; }
    }
}
