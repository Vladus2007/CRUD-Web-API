using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class Note
    {
       
        [Key]
        public int id { get; set; }
        [Required]
        [MaxLength(100)]
        public string title { get; set; }
        public string description { get; set; }
        public DateTime CreateAt { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string UserId { get; set; }
        
       
    }
}
