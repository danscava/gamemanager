using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
    /// <summary>
    /// Model for the table user_has_role
    /// </summary>
    [Table("user_has_role")]
    public class UserHasRole : Audit
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("active")]
        public override int Active { get; set; }
    }
}