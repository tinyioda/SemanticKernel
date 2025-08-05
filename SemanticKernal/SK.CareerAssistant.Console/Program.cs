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

        var careerFocus = "Software Developer";
        var prompt = $@"The following is a conversation with an AI career coaching assistant.
            The assistant is helpful, creative, and very friendly.

            <message role=""user"">Could you please give me some career suggestions? I would like to be able to travel more often.</message>

            <message role=""assistant"">Of course! What is your current career or area of expertise?</message>

            <message role=""user"">${careerFocus}</message>";

        var result = await kernel.InvokePromptAsync(prompt);

        Console.WriteLine("\nPrompt Result: " + result);
    }
}