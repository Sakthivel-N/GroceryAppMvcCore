using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(20)]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required, MaxLength(20)]
        public string EmailId { get; set; }
        [Required, MaxLength(13)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(20)]
        public string Area { get; set; }

        [Required, MaxLength(20)]
        public string City { get; set; }
        [Required, MaxLength(20)]
        public string State { get; set; }
        [Required, MaxLength(20)]
        public string Country { get; set; }
        [Required, MaxLength(10)]
        public string Pincode { get; set; }


        [DataType(DataType.Password)]
        [Required, MaxLength(20)]
        public string Password { get; set; }

    }
}
