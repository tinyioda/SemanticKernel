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

        var kernel = builder.Build();
        var plugins = kernel.CreatePluginFromPromptDirectory("Plugins");

        Console.WriteLine("Enter your current career or area of expertise: ");
        var careerFocus = Console.ReadLine();

        var result = await kernel.InvokeAsync(
            plugins["CareerCoach"],
            new()
            {
                { 
                    "careerFocus", careerFocus 
                }
            }
        );

        Console.WriteLine("\nPrompt Result: " + result);
    }
}