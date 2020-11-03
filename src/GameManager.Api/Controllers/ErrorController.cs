using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameManager.Data.Dtos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameManager.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling errors on the api calls
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        /// <summary>
        /// Default constuctor
        /// </summary>
        /// <param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the errors on the api
        /// </summary>
        /// <returns></returns>
        [Route("error")]
        public ApiErrorResponseDto Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            Response.StatusCode = (int) StatusCodes.Status400BadRequest;

            // On a improved solution, we should have codes for different types of exceptions and errors

            if (exception != null)
            {
                _logger.LogError($"Returning error: {exception.Message}");

                return new ApiErrorResponseDto(exception.Message, 1);
            }
            else
            {
                return new ApiErrorResponseDto("Unknown error", 1);
            }
        }
    }
}