using System.ComponentModel.DataAnnotations;
namespace GroceryAppMvcCore.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [MaxLength(255)]
        public string FeedbackMsg { get; set; }
    }
}
