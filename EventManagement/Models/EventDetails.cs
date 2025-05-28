using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Model
{
    public class EventDetails
    {
        [Key]
        [Column("EventId")]
        public int EventId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("EventName", TypeName = "varchar(50)")]
        public string EventName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("EventCategory", TypeName = "varchar(50)")]
        public string EventCategory { get; set; }

        [Required]
        [Column("EventDate", TypeName = "datetime")]
        public DateTime EventDate { get; set; }

        [Column("Description", TypeName = "varchar(max)")]
        public string? Description { get; set; }

        [Required]
        [Column("Status", TypeName = "varchar(20)")]
        [RegularExpression("Active|In-Active")]
        public string Status { get; set; }
    

}
}