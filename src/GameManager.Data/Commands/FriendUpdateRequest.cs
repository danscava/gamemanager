using System;
using System.ComponentModel.DataAnnotations;

namespace GameManager.Data.Commands
{
    /// <summary>
    /// Request command for updating a friend
    /// </summary>
    public class FriendUpdateRequest
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String Telephone { get; set; }
    }
}