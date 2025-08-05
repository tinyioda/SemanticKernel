using System;
using Microsoft.EntityFrameworkCore;
using SK.CareerAssistant.WebApp.Data;

namespace SK.CareerAssistant.WebApp.Services;

/// <summary>
/// 
/// </summary>
/// <param name="_dbContext"></param>
public class ChatService(AppDbContext _dbContext)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="message"></param>
    /// <param name="sender"></param>
    /// <returns></returns>
    public async Task AddMessageAsync(string sessionId, string message, string sender)
    {
        var chatMessage = new ChatMessage
        {
            Sender = sender,
            SessionId = sessionId,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        _dbContext.ChatMessages.Add(chatMessage);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ChatMessage>> GetMessagesAsync(string sessionId)
    {
        return await _dbContext.ChatMessages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }
}
