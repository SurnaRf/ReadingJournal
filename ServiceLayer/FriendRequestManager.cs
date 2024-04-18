using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
	public class FriendRequestManager
	{
		private readonly FriendRequestContext friendRequestContext;
		private readonly UserManager userManager;

		public FriendRequestManager(FriendRequestContext friendRequestContext, UserManager userManager)
		{
			this.friendRequestContext = friendRequestContext;
			this.userManager = userManager;
		}

		public async Task SendFriendRequestAsync(string senderId, string receiverId)
		{
			var friendRequest = new FriendRequest
			{
				SenderId = senderId,
				ReceiverId = receiverId,
				IsAccepted = false
			};

			await friendRequestContext.CreateAsync(friendRequest);
		}

		public async Task AcceptFriendRequestAsync(string requestId)
		{
			var friendRequest = await friendRequestContext.ReadAsync(requestId, true);
			if (friendRequest != null)
			{
				friendRequest.IsAccepted = true;
				await friendRequestContext.UpdateAsync(friendRequest);

				var sender = await userManager.ReadUserAsync(friendRequest.SenderId);
				var receiver = await userManager.ReadUserAsync(friendRequest.ReceiverId);
				sender.Friends.Add(receiver);
				receiver.Friends.Add(sender);
				await userManager.UpdateUserAsync(sender, true);
				await userManager.UpdateUserAsync(receiver, true);
			}
		}

        public async Task<List<FriendRequest>> GetFriendRequestsForUserAsync(string userId, bool useNavigationalProperties = false)
        {
			return await friendRequestContext.GetFriendRequestsForUserAsync(userId, useNavigationalProperties);
        }

		public async Task DeleteFriendRequestAsync(string requestId)
		{
			await friendRequestContext.DeleteAsync(requestId);
		}
    }

}
