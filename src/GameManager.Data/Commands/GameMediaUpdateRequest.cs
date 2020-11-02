using System;
using GameManager.Data.Enums;

namespace GameManager.Data.Commands
{
    /// <summary>
    /// Request command for updating a game media
    /// </summary>
    public class GameMediaUpdateRequest
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public int Year { get; set; }

        public Platform Platform { get; set; }

        public MediaType MediaType { get; set; }
    }
}