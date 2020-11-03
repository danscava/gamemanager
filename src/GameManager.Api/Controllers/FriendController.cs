using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameManager.Api.Extensions;
using GameManager.Data.Commands;
using GameManager.Data.Dtos;
using GameManager.Data.Interfaces;
using GameManager.Services;
using GameManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameManager.Api.Controllers
{
    /// <summary>
    /// Controller responsible for friend management
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly ILogger<GameMediaController> _logger;
        private readonly IFriendService _friendService;
        private readonly IAsyncFriendRepository _friendRepository;
        private readonly IMapper _mapper;

        public FriendController(ILogger<GameMediaController> logger,
            IFriendService friendService,
            IAsyncFriendRepository friendRepository,
            IMapper mapper)
        {
            _logger = logger;
            _friendService = friendService;
            _friendRepository = friendRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all friends
        /// </summary>
        /// <returns>List of friends</returns>
        [ProducesResponseType(typeof(FriendResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var friends = await _friendRepository.GetAllAsync();

            var response = _mapper.Map<List<FriendResponseDto>>(friends);


            return Ok(response);
        }

        /// <summary>
        /// Get a specific friend by its Id
        /// </summary>
        /// <param name="id">Friend Id</param>
        /// <returns>Friend information</returns>
        [ProducesResponseType(typeof(FriendResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [Authorize(Roles = Role.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var friend = await _friendRepository.GetByIdAsync(id);

            if (friend == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<FriendResponseDto>(friend));
        }

        /// <summary>
        /// Adds a new friend
        /// </summary>
        /// <param name="createDto">Json containing the friend information</param>
        /// <returns>Created friend info</returns>
        [ProducesResponseType(typeof(FriendResponseDto), 201)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FriendCreateDto createDto)
        {
            var friend = await _friendService.CreateAsync(
                new FriendCreateRequest()
                {
                    Email = createDto.email,
                    Name = createDto.name,
                    Telephone = createDto.telephone
                });

            if (friend == null)
            {
                throw new Exception("Error creating friend");
            }

            var response = _mapper.Map<FriendResponseDto>(friend);

            return CreatedAtAction(nameof(Get), new { response.id }, response);
        }

        /// <summary>
        /// Updates a friend information
        /// </summary>
        /// <param name="id">Friend Id</param>
        /// <param name="updateDto">Json with new information</param>
        /// <returns>Updated info</returns>
        [ProducesResponseType(typeof(FriendResponseDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FriendCreateDto updateDto)
        {
            var friend = await _friendRepository.GetByIdAsync(id);

            if (friend == null)
            {
                return NotFound();
            }

            var updateRequest = new FriendUpdateRequest()
            {
                Id = id,
                Email = updateDto.email,
                Telephone = updateDto.telephone,
                Name = updateDto.name
            };

            var updatedMedia = await _friendService.UpdateAsync(updateRequest);

            return Ok(_mapper.Map<FriendResponseDto>(updatedMedia));
        }

        /// <summary>
        /// Removes a friend
        /// </summary>
        /// <param name="id">Friend Id</param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponseDto), 400)]
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var friend = await _friendRepository.GetByIdAsync(id);

            if (friend == null)
            {
                return NotFound();
            }

            await _friendService.DeleteAsync(id);

            return Ok();
        }
    }
}
