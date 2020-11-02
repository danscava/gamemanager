
using System;
using System.Collections.Generic;
using System.Text;

namespace GameManager.Data.Dtos
{
    /// <summary>
    /// DTO responsible for retuning errors
    /// </summary>
    public class ApiErrorResponseDto
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="code">Error code</param>
        public ApiErrorResponseDto(string message, int code)
        {
            this.message = message;
            this.code = code;
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public int code { get; set; }
    }
}
