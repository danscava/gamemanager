using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for game media creation
    /// </summary>
    public class GameMediaCreateDto
    {
        [Required] public String title { get; set; }

        [Required] public int year { get; set; }

        [Required] public int platform { get; set; }

        [Required] public int media_type { get; set; }
    }
}