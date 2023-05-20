using Microsoft.AspNetCore.SignalR;
using Server.Context;

namespace Server.Hubs;

public sealed class ChatHub : Hub
{
    public async Task SendMessage(Message message)
    {
        await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinGroup(string chatId)
    {        int chatIdInt;
        bool isParsed = Int32.TryParse(chatId, out chatIdInt);
        if (isParsed && chatIdInt > 0)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
    }

    public async Task LeaveGroup(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}
