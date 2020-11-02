using System;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for borrowed game media
    /// </summary>
    public class BorrowedGameMediaResponseDto
    {
        public int id { get; set; }

        public String title { get; set; }

        public int year { get; set; }

        public int platform { get; set; }

        public int media { get; set; }
    }
}