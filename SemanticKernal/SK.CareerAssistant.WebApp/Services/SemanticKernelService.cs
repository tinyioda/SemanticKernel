using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using SK.CareerAssistant.WebApp.Data;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace SK.CareerAssistant.WebApp.Services;

public class SemanticKernelService
{
    private readonly Kernel _kernel;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public SemanticKernelService(IConfiguration configuration)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOllamaChatCompletion(
            endpoint: new Uri("http://localhost:11434"),
            modelId: "gemma3:12b"
        );

        _kernel = builder.Build();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userMessage"></param>
    /// <returns></returns>
    public async Task<string> GetChatResponseAsync(string userMessage)
    {
        var result = await _kernel.InvokePromptAsync(userMessage);

        return result.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userMessage"></param>
    /// <param name="messages"></param>
    /// <returns></returns>
    public async Task<string> GetChatResponseWithHistoryAsync(string userMessage, IEnumerable<ChatMessage> messages)
    {
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        ChatHistory chatHistory = [];

        foreach (var message in messages)
        {
            if (message.Sender == "User")
            {
                chatHistory.AddUserMessage(message.Message);
            }
            else
            {
                chatHistory.AddAssistantMessage(message.Message);
            }
        }

        chatHistory.AddUserMessage(userMessage);

        var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
            chatHistory: chatHistory,
            kernel: _kernel
        );

        var response = new StringBuilder();

        await foreach (var chunk in result)
        {
            response.Append(chunk);
        }

        return response.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userMessage"></param>
    /// <returns></returns>
    public async Task<string> GetChatResponseWithRagAsync(string userMessage)
    {
        string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "career-profiles.pdf");

        string content = PdfUtilities.ExtractTextFromPdf(pdfPath);

        var prompt = $"""
                You are a career guidance assistant. You will be provided with a list of career profiles, each containing a title and a description.

                Your task is to help users find suitable career options based on their interests and skills. When a user provides their interests or skills, identify and list the careers that align with the provided information."

                Here is the list of career profiles:

                {content}

                If a match can't be found within the provided list of career profiles, please speculate and give the user 10 other related careers in a bullet point list.

                User's interests and skills: {userMessage}
                """;
        
        var result = await _kernel.InvokePromptAsync(prompt);

        return result.ToString();
    }
}
