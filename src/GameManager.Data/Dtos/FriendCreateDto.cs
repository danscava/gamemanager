using System;
using System.ComponentModel.DataAnnotations;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for friend creation request
    /// </summary>
    public class FriendCreateDto
    {
        [Required]
        public String name { get; set; }

        [Required]
        public String email { get; set; }

        [Required]
        public String telephone { get; set; }
    }
}