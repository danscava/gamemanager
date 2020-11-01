using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
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
        public int Active { get; set; }
    }
}