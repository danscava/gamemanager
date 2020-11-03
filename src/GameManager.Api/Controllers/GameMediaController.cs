using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameManager.Api.Extensions;
using GameManager.Data.Commands;
using GameManager.Data.Dtos;
using GameManager.Data.Enums;
using GameManager.Data.Interfaces;
using GameManager.Services;
using GameManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace GameManager.Api.Controllers
{
    /// <summary>
    /// Controlle responsible for game media management
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameMediaController : ControllerBase
    {
        private readonly ILogger<GameMediaController> _logger;
        private readonly IGameMediaService _gameMediaService;
        private readonly IAsyncGameMediaRepository _gameMediaRepository;
        private readonly IMapper _mapper;

        public GameMediaController(ILogger<GameMediaController> logger,
            IGameMediaService gameMediaService,
            IAsyncGameMediaRepository gameMediaRepository,
            IMapper mapper)
        {
            _logger = logger;
            _gameMediaService = gameMediaService;
            _gameMediaRepository = gameMediaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all games
        /// </summary>
        /// <returns>List of games</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(GameMediaResponseDto[]), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var gameMedias = await _gameMediaRepository.GetAllAsync();

            var response = _mapper.Map<List<GameMediaResponseDto>>(gameMedias);

            return Ok(response);
        }

        /// <summary>
        /// Get a specific game by its Id
        /// </summary>
        /// <param name="id">Game's Id</param>
        /// <returns>Game information</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(GameMediaResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var gameMedia = await _gameMediaRepository.GetByIdAsync(id);

            if (gameMedia == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameMediaResponseDto>(gameMedia));
        }

        /// <summary>
        /// Creates a new game 
        /// </summary>
        /// <param name="createDto">Json containing the game information</param>
        /// <returns>Created game information</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(GameMediaResponseDto), 201)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GameMediaCreateDto createDto)
        {
            if (!Enum.IsDefined(typeof(Platform), createDto.platform))
            {
                throw new ArgumentException("Platform value is not valid");
            }

            if (!Enum.IsDefined(typeof(MediaType), createDto.media_type))
            {
                throw new ArgumentException("Game media type is not valid");
            }

            var gameMedia = await _gameMediaService.CreateAsync(
                new GameMediaCreateRequest()
                {
                    Title = createDto.title,
                    Year = createDto.year,
                    MediaType = (MediaType) createDto.media_type,
                    Platform = (Platform) createDto.platform
                });

            if (gameMedia == null)
            {
                throw new Exception("Error creating game media");
            }

            var response = _mapper.Map<GameMediaResponseDto>(gameMedia);

            return CreatedAtAction(nameof(Get), new {response.id}, response);
        }

        /// <summary>
        /// Updates a game information
        /// </summary>
        /// <param name="id">Game's Id</param>
        /// <param name="updateDto">Game with updated information</param>
        /// <returns>Updated game information</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(GameMediaResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GameMediaCreateDto updateDto)
        {
            var gameMedia = await _gameMediaRepository.GetByIdAsync(id);

            if (gameMedia == null)
            {
                return NotFound();
            }

            if (!Enum.IsDefined(typeof(Platform), updateDto.platform))
            {
                throw new ArgumentException("Platform value is not valid");
            }

            if (!Enum.IsDefined(typeof(MediaType), updateDto.media_type))
            {
                throw new ArgumentException("Game media type is not valid");
            }

            var updateRequest = new GameMediaUpdateRequest()
            {
                Id = id,
                Title = updateDto.title,
                Year = updateDto.year,
                MediaType = (MediaType) updateDto.media_type,
                Platform = (Platform) updateDto.platform
            };

            var updatedMedia = await _gameMediaService.UpdateAsync(updateRequest);

            return Ok(_mapper.Map<GameMediaResponseDto>(updatedMedia));
        }

        /// <summary>
        /// Deletes a game
        /// </summary>
        /// <param name="id">Game's Id</param>
        /// <returns></returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gameMedia = await _gameMediaRepository.GetByIdAsync(id);

            if (gameMedia == null)
            {
                return NotFound();
            }

            await _gameMediaService.DeleteAsync(id);

            return Ok();
        }

        /// <summary>
        /// Lends a game to a friend
        /// </summary>
        /// <param name="id">Game's Id</param>
        /// <param name="lendDto">Friend information</param>
        /// <returns>Game with the borrower information</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(GameMediaResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpPost("{id}/lend")]
        public async Task<IActionResult> Post(int id, [FromBody] GameMediaLendDto lendDto)
        {
            var gameMedia = await _gameMediaService.LendGameAsync(
                new GameMediaLendRequest()
                {
                    GameId = id,
                    FriendId = lendDto.friend_id
                });

            if (gameMedia == null)
            {
                throw new Exception("Error lending the game");
            }

            var response = _mapper.Map<GameMediaResponseDto>(gameMedia);

            return Ok(response);
        }

        /// <summary>
        /// Returns a borrowed game
        /// </summary>
        /// <param name="id">Game's Id</param>
        /// <returns>Game information</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(GameMediaResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = Role.Admin)]
        [HttpPost("{id}/return")]
        public async Task<IActionResult> Post(int id)
        {
            var gameMedia = await _gameMediaService.ReturnGameAsync(
                new GameMediaReturnRequest()
                {
                    GameId = id
                });

            if (gameMedia == null)
            {
                throw new Exception("Error returning the game");
            }

            var response = _mapper.Map<GameMediaResponseDto>(gameMedia);

            return Ok(response);
        }
    }
}
