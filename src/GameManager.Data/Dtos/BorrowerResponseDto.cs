using System;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for borrower information
    /// </summary>
    public class BorrowerResponseDto
    {
        public int id { get; set; }

        public String name { get; set; }

        public String email { get; set; }

        public String telephone { get; set; }
    }
}