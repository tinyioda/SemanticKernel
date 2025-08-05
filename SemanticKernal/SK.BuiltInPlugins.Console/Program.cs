using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Plugins.Core;

public class Program
{
    private static async Task Main(string[] args)
    {
        var model = "gemma3:12b";
        var uri = new Uri("http://localhost:11434");

        Console.WriteLine("Model: " + model);
        Console.WriteLine("Ollama Uri: " + uri.ToString());

        var builder = Kernel.CreateBuilder();
        builder.AddOllamaChatCompletion(model, uri);
        builder.Plugins.AddFromType<TimePlugin>();
        builder.Plugins.AddFromType<ConversationSummaryPlugin>();

        var kernel = builder.Build();

        var now = await kernel.InvokeAsync(nameof(TimePlugin), "Now");
        var timeZone = await kernel.InvokeAsync(nameof(TimePlugin), "TimeZoneName");

        Console.WriteLine("\n\n* --- TimePlugin ---*");
        Console.WriteLine("Now: " + now);
        Console.WriteLine("Time Zone: " + timeZone);

        Console.WriteLine("\n\n* --- ConversationSummaryPlugin ---*");
        Console.WriteLine("Enter your inquiry: ");
        Console.WriteLine("Example: I love to travel. When would be the best time to go to Jamacia? Please book a flight using that information.");
        var userInput = Console.ReadLine();

        var result = await kernel.InvokeAsync(nameof(ConversationSummaryPlugin),
            "GetConversationActionItems", 
            new() {
                {"input", userInput }
            }
        );

        Console.WriteLine("\nAction Items: " + result);

        result = await kernel.InvokeAsync(nameof(ConversationSummaryPlugin),
            "GetConversationTopics",
            new() {
                {"input", userInput }
            }
        );

        Console.WriteLine("\nConversation Topics: " + result);

        result = await kernel.InvokeAsync(nameof(ConversationSummaryPlugin),
            "SummarizeConversation",
            new() {
                {"input", userInput }
            }
        );

        Console.WriteLine("\nSummarize Conversation: " + result);

        Console.WriteLine("\n\n* --- Basic Prompt Template ---*");
        var careerHistory = "I am a software developer and have also been a professor of computer science for the last 15 years. I would like some advice on some career choices that would let me travel more often.";
        var prompt = @"This is some information about the users background: {{$careerHistory}}

            Given this users background, provide a list of relevant career choices.";

        result = await kernel.InvokePromptAsync(prompt,
            new() {
                {"careerHistory", careerHistory }
            }
        );

        Console.WriteLine("\nPrompt Result: " + result);
    }
}