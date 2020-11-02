using System;
using System.Collections.Generic;
using System.Text;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for authentication request
    /// </summary>
    public class AuthenticateRequestDto
    {
        public String username { get; set; }
        public String password { get; set; }
    }
}
