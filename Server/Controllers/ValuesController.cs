using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Dtos;
using Server.Hubs;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private AppDbContext context = new();
    private readonly IHubContext<ChatHub> _hubContext;

    public ValuesController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        User user = await context.Users.FirstOrDefaultAsync(p=> p.Username == login.UserName);
        if (user == null) throw new Exception("Kullanıcı bulunamadı");

        return Ok(user);
    }

    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUsers(int userId)
    {
        IList<User> users = await context.Users.Where(p=> p.Id != userId).ToListAsync();

        return Ok(users);
    }    

    [HttpPost("[action]")]
    public async Task<ActionResult> GetChatMessages(GetMessagesDto request)
    {
        var chatId = context.ChatParticipants
                     .Where(cp => cp.UserId == request.UserId)
                     .Select(cp => cp.ChatId)
                     .Intersect(
                          context.ChatParticipants
                                  .Where(cp => cp.UserId == request.ToUserId)
                                  .Select(cp => cp.ChatId)
                     )
                     .FirstOrDefault();

        // Eğer chatId 0 ise, yeni bir chat oluşturulmalıdır.
        if (chatId == 0)
        {
            // Yeni bir chat oluşturulur.
            var chat = new Chat
            {
                Name = $"Private chat between {request.UserId} and {request.ToUserId}",
                CreationDate = DateTime.Now
            };
            context.Chats.Add(chat);

            // Veritabanındaki değişiklikler kaydedilir.
            await context.SaveChangesAsync();

            // Yeni oluşturulan chatId alınır.
            chatId = chat.Id;

            // İhsan ve Ayşe'nin bu chat'in katılımcıları olduğu belirtilir.
            var userChatParticipant = new ChatParticipant { UserId = request.UserId, ChatId = chatId };
            var toUserChatParticipant = new ChatParticipant { UserId = request.ToUserId, ChatId = chatId };

            context.ChatParticipants.AddRange(userChatParticipant, toUserChatParticipant);

            // Veritabanındaki değişiklikler kaydedilir.
            await context.SaveChangesAsync();
        }

        var messages = await context.Messages
                       .Where(m => m.ChatId == chatId)
                       .Include(p=> p.User)
                       .OrderBy(m => m.Timestamp)
                       .ToListAsync();

        MessagesDto response = new()
        {
            Messages = messages,
            ChatId = chatId
        };

        return Ok(response);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<Message>> PostMessage(MessageDto request)
    {
        Message message = new()
        {
            ChatId = request.ChatId,
            UserId = request.UserId,
            Text = request.Text,
            Timestamp = DateTime.Now
        };

        context.Messages.Add(message);
        await context.SaveChangesAsync();

        User user = await context.Users.FirstOrDefaultAsync(p=> p.Id == request.UserId);

        await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);

        return Ok(new { Message = "Message başarıyla gönderildi!" });
    }

    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        User user = await context.Users.FirstOrDefaultAsync(p=> p.Id == userId);
        if (user == null) throw new Exception("Kullanıcı bulunamadı!");

        return Ok(user);
    }
}
