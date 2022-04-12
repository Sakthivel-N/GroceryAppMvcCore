﻿using System.ComponentModel.DataAnnotations;

namespace GroceryAppMvcCore.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [Required,MaxLength(20)]
        public string AdminName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required, MaxLength(20)]
        public string EmailId { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
        

    }
}
