using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
    [Table("lend_history")]
    public class LendHistory : Audit
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }

        [Column("game_media_id")]
        public int GameMediaId { get; set; }

        [ForeignKey("GameMediaId")]
        public virtual GameMedia GameMedia { get; set; }

        [Column("borrower_id")]
        public int? BorrowerId { get; set; }

        [ForeignKey("BorrowerId")]
        public virtual Friend Borrower { get; set; }
    }
}