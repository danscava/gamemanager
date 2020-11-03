using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for authentication request
    /// </summary>
    public class AuthenticateRequestDto
    {
        [Required]
        public String username { get; set; }
        [Required]
        public String password { get; set; }
    }
}
