using Microsoft.AspNetCore.SignalR;

namespace TaskHub.Models.Playlist.NewBackGroungLogic;

public class ProgressHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}