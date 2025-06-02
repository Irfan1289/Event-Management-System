using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Model
{
    public class ParticipantEventDetails
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("ParticipantEmailId", TypeName = "varchar(100)")]
        public string ParticipantEmailId { get; set; }

        // 👇 Navigation property with [ForeignKey] referring to FK property name
        [ForeignKey(nameof(ParticipantEmailId))]
        public UserInfo? Participant { get; set; }

        [Required]
        [Column("EventId")]
        public int EventId { get; set; }

        // 👇 Navigation property with [ForeignKey] referring to FK property name
        [ForeignKey(nameof(EventId))]
        public EventDetails? Event { get; set; }

        [Required]
        [Column("IsAttended", TypeName = "varchar(3)")]
        [RegularExpression("Yes|No")]
        public string IsAttended { get; set; }
    }
}
