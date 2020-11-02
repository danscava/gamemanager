using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Commands;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using GameManager.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameManager.Services
{
    public class FriendService : IFriendService
    {
        private readonly ILogger<FriendService> _logger;
        private readonly IAsyncFriendRepository _friendRepository;

        public FriendService(ILogger<FriendService> logger,
            IAsyncFriendRepository friendRepository)
        {
            _logger = logger;
            _friendRepository = friendRepository;
        }

        public async Task<Friend> CreateAsync(FriendCreateRequest createRequest)
        {
            if (String.IsNullOrEmpty(createRequest.Name))
            {
                throw new ArgumentNullException(nameof(createRequest.Name));
            }

            if (String.IsNullOrEmpty(createRequest.Email))
            {
                throw new ArgumentNullException(nameof(createRequest.Email));
            }

            if (String.IsNullOrEmpty(createRequest.Telephone))
            {
                throw new ArgumentNullException(nameof(createRequest.Telephone));
            }

            var newFriend = new Friend();
            newFriend.Name = createRequest.Name;
            newFriend.Email = createRequest.Email;
            newFriend.Telephone = createRequest.Telephone;

            var friend = await _friendRepository.AddAsync(newFriend);

            return friend;
        }

        public async Task<Friend> UpdateAsync(FriendUpdateRequest updateRequest)
        {
            var friend = await _friendRepository.GetByIdAsync(updateRequest.Id);

            if (friend == null)
            {
                throw new Exception($"Friend with id {updateRequest.Id} not found");
            }

            if (String.IsNullOrEmpty(updateRequest.Name))
            {
                throw new ArgumentNullException(nameof(updateRequest.Name));
            }

            if (String.IsNullOrEmpty(updateRequest.Email))
            {
                throw new ArgumentNullException(nameof(updateRequest.Email));
            }

            if (String.IsNullOrEmpty(updateRequest.Telephone))
            {
                throw new ArgumentNullException(nameof(updateRequest.Telephone));
            }

            friend.Name = updateRequest.Name;
            friend.Email= updateRequest.Email;
            friend.Telephone= updateRequest.Telephone;

            friend = await _friendRepository.UpdateAsync(friend);

            return friend;
        }

        public async Task<bool> DeleteAsync(int friendId)
        {
            var friend = await _friendRepository.GetByIdAsync(friendId);

            if (friend != null)
            {
                await _friendRepository.DeleteAsync(friend);
                return true;
            }

            return false;
        }
    }
}
