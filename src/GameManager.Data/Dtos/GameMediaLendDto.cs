using System.ComponentModel.DataAnnotations;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO for lending a game media
    /// </summary>
    public class GameMediaLendDto
    {
        [Required] public int friend_id { get; set; }
    }
}