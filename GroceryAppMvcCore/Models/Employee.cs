using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required,MaxLength(20)]
        public string EmployeeName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required,MaxLength(20)]
        public string EmaiLId { get; set; }
        [Required, MaxLength(13)]
        public string Phone { get; set; }
        [Required,MaxLength(20)]
        public string City { get; set; }

        [DataType(DataType.Password)]
        [Required,MaxLength(20)]
        public string Password { get; set; }
     
    }
}
