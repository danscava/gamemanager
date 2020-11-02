using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
    /// <summary>
    /// Model for the table user
    /// </summary>
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
        public string Password { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("active")]
        public override int Active { get; set; }

        public ICollection<UserHasRole> Roles { get; set; }
    }
}