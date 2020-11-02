using System;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for friend creation request
    /// </summary>
    public class FriendCreateDto
    {
        public String name { get; set; }

        public String email { get; set; }

        public String telephone { get; set; }
    }
}