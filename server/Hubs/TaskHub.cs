using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManager.Api.Hubs
{
    [Authorize]
    public class TaskHub : Hub
    {
        public Task SendTaskUpdate(object payload)
        {
            return Clients.All.SendAsync("TaskUpdated", payload);
        }
    }
}
