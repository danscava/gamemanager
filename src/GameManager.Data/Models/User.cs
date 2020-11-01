using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
    [Table("user")]
    public class User : Audit
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public byte[] Password { get; set; }

        [Column("salt")]
        public byte[] Salt { get; set; }

        [Column("active")]
        public int Active { get; set; }
    }
}