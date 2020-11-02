using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GameManager.Data.Enums;

namespace GameManager.Data.Models
{
    /// <summary>
    /// Model for the table game_media
    /// </summary>
    [Table("game_media")]
    public class GameMedia : Audit
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }

        [Column("borrower_id")]
        public int? BorrowerId { get; set; }

        [ForeignKey("BorrowerId")]
        public virtual Friend Borrower { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [Column("platform")]
        public Platform Platform { get; set; }

        [Column("media_type")]
        public MediaType MediaType { get; set; }

        [Column("active")]
        public override int Active { get; set; }
    }
}
