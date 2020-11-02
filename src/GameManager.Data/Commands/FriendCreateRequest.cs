using System;

namespace GameManager.Data.Commands
{
    /// <summary>
    /// Request command for creating a friend
    /// </summary>
    public class FriendCreateRequest
    {
        public String Name { get; set; }

        public String Email { get; set; }

        public String Telephone { get; set; }
    }
}