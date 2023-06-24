using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Models
{
    public class Users
    {
        [Key]
        public int User_id { get; set; }
        [Required]
        [Column(TypeName = "TEXT")]
        public string? UserName { get; set; }

        [Column(TypeName = "TEXT")]
        public string? PasswordHash { get; set; }

        [Column(TypeName = "TEXT")]
        public string? PasswordSalt { get; set; }

        [Column(TypeName = "BOOLEAN")]
        public bool? Status { get; set; }
    }
}