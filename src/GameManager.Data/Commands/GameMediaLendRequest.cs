namespace GameManager.Data.Commands
{
    /// <summary>
    /// Request command for lending a game media
    /// </summary>
    public class GameMediaLendRequest
    {
        public int GameId { get; set; }

        public int FriendId { get; set; }
    }
}