using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace School_API.Hubs
{
    public class CommunicationHub : Hub
    {
        // Tracks active connections: UserId -> ConnectionId
        private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                OnlineUsers[userId] = Context.ConnectionId;
                await Clients.All.SendAsync("UserStatusChanged", userId, true);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string? disconnectedUserId = null;
            foreach (var kvp in OnlineUsers)
            {
                if (kvp.Value == Context.ConnectionId)
                {
                    disconnectedUserId = kvp.Key;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(disconnectedUserId))
            {
                OnlineUsers.TryRemove(disconnectedUserId, out _);
                await Clients.All.SendAsync("UserStatusChanged", disconnectedUserId, false);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendDirectMessage(string receiverUserId, object message)
        {
            if (OnlineUsers.TryGetValue(receiverUserId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveDirectMessage", message);
            }
        }

        public async Task SendGroupMessage(string roomId, object message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveGroupMessage", roomId, message);
        }

        public async Task SendTypingIndicator(string receiverUserId, string senderUserId, bool isTyping)
        {
            if (OnlineUsers.TryGetValue(receiverUserId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveTypingIndicator", senderUserId, isTyping);
            }
        }

        public async Task BroadcastSystemNotification(object notification)
        {
            await Clients.All.SendAsync("ReceiveSystemNotification", notification);
        }

        public static bool IsUserOnline(string userId)
        {
            return OnlineUsers.ContainsKey(userId);
        }
    }
}
