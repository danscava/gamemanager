using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GameManager.Data.Enums;

namespace GameManager.Page.Models
{
    public class GameCreateModel
    {
        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 2)]
        public String Title { get; set; }

        [Range(1950, 2020, ErrorMessage = "Invalid year")]
        [Required]
        public int Year { get; set; }

        [Required]
        public Platform Platform { get; set; }

        [Required]
        public MediaType MediaType { get; set; }

    }
}
