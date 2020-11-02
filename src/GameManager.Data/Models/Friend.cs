using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
    /// <summary>
    /// Model for the table friend
    /// </summary>
    [Table("friend")]
    public class Friend : Audit
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("telephone")]
        public string Telephone { get; set; }

        [Column("active")]
        public override int Active { get; set; }

        public ICollection<GameMedia> BorrowedGames { get; set; }
    }
}