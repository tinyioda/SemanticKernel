using Microsoft.AspNetCore.Mvc;
using SK.CareerAssistant.WebApp.Services;

namespace SK.CareerAssistant.WebApp.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="_semanticKernelService"></param>
/// <param name="_chatService"></param>
[Route("api/[controller]")]
[ApiController]
public class ChatController(SemanticKernelService _semanticKernelService, ChatService _chatService) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [HttpGet("{sessionId}")]
    public async Task<ActionResult> GetMessagesAsync(string sessionId)
    {
        return Ok(await _chatService.GetMessagesAsync(sessionId));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> GetChatResponseAsync([FromBody] ChatRequest chatRequest)
    {
        var sessionId = chatRequest.SessionId;

        var messages = await _chatService.GetMessagesAsync(sessionId); 
        
        await _chatService.AddMessageAsync(sessionId, chatRequest.UserMessage, "User");
        // var response = await _semanticKernelService.GetChatResponseAsync(chatRequest.UserMessage);
        // var response = await _semanticKernelService.GetChatResponseWithHistoryAsync(chatRequest.UserMessage, messages);
        
        var response = await _semanticKernelService.GetChatResponseWithRagAsync(chatRequest.UserMessage);
        await _chatService.AddMessageAsync(sessionId, response, "Bot");
        
        return Ok(response);
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="UserMessage"></param>
/// <param name="SessionId"></param>
public record ChatRequest(string UserMessage, string SessionId);
