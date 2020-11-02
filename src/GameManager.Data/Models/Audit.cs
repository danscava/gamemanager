using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Data.Models
{
    /// <summary>
    /// Represents the base information for database models
    /// </summary>
    public abstract class Audit
    {
        public virtual int Id { get; set; }

        public virtual int Active { get; set; }

        #region Audit fields

        [Column("create_by", Order = 101)]
        public string CreateBy { get; set; } = "root@localhost";

        [Column("update_by", Order = 102)]
        public string UpdateBy { get; set; } = "root@localhost";

        [Column("create_time", Order = 103)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Column("update_time", Order = 104)]
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        #endregion
    }
}