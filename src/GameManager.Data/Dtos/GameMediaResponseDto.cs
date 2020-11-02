using System;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for game media response
    /// </summary>
    public class GameMediaResponseDto
    {
        public int id { get; set; }

        public String title { get; set; }

        public int year { get; set; }

        public int platform { get; set; }

        public int media { get; set; }

        public BorrowerResponseDto borrower { get; set; }
    }
}