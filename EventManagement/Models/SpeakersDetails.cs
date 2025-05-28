using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Model
{
    public class SpeakersDetails
    {
        [Key]
        [Column("SpeakerId")]
        public int SpeakerId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("SpeakerName", TypeName = "varchar(50)")]
        public string SpeakerName { get; set; }
    }
}