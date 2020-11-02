using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for authentication response
    /// </summary>
    public class AuthenticateResponseDto
    {
        public String name { get; set; }

        public String username { get; set; }

        public String token { get; set; }
    }
}
