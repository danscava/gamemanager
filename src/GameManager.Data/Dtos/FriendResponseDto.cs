using System;
using System.Collections.Generic;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for friend response
    /// </summary>
    public class FriendResponseDto
    {
        public int id { get; set; }

        public String name { get; set; }

        public String email { get; set; }

        public String telephone { get; set; }

        public List<BorrowedGameMediaResponseDto> games { get; set; }
    }
}