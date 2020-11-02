using System;
using System.Collections.Generic;
using System.Text;
using GameManager.Data.Enums;

namespace GameManager.Data.Commands
{
    /// <summary>
    /// Request command for creating a game media
    /// </summary>
    public class GameMediaCreateRequest
    {
        public String Title { get; set; }

        public int Year { get; set; }

        public Platform Platform { get; set; }

        public MediaType MediaType { get; set; }
    }
}
