   using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventManagement.Model
{

    public class SessionInfo
    {
        [Key]
        [Column("SessionId")]
        public int SessionId { get; set; }

        [Required]
        [Column("EventId")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        
        public EventDetails? Event { get; set; }

        [Required]
        [StringLength(50)]
        [Column("SessionTitle", TypeName = "varchar(50)")]
        public string SessionTitle { get; set; }

        [Column("SpeakerId")]
        public int? SpeakerId { get; set; }

        [ForeignKey("SpeakerId")]
        public SpeakersDetails? Speaker { get; set; }

        [Column("Description", TypeName = "varchar(max)")]
        public string? Description { get; set; }

        [Required]
        [Column("SessionStart", TypeName = "datetime")]
        public DateTime SessionStart { get; set; }

        [Required]
        [Column("SessionEnd", TypeName = "datetime")]
        public DateTime SessionEnd { get; set; }

        [Column("SessionUrl", TypeName = "varchar(max)")]
        public string? SessionUrl { get; set; }
    }

}