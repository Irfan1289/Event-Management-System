using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Model
{


    public class UserInfo
    {
        [Key]
        [Column("EmailId", TypeName = "varchar(100)")]
        public string EmailId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Column("UserName", TypeName = "varchar(50)")]
        public string UserName { get; set; }

        [Required]
        [Column("Role", TypeName = "varchar(20)")]
        [RegularExpression("Admin|Participant")]
        public string Role { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [Column("Password", TypeName = "varchar(20)")]
        public string Password { get; set; }


    }
}
