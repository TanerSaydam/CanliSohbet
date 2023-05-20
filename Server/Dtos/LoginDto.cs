using Server.Context;

namespace Server.Dtos;

public sealed class LoginDto
{
    public string UserName { get; set; }
}

public sealed class GetMessagesDto
{
    public int UserId { get; set; }
    public int ToUserId { get; set; }
}

public sealed class MessagesDto
{
    public IEnumerable<Message> Messages { get; set; }
    public int ChatId { get; set; }
}

public sealed class MessageDto
{
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
}
