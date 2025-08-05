using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;

public class Program
{
    private static async Task Main(string[] args)
    {
        var exit = "n";
        var model = "gemma3:12b";
        var uri = new Uri("http://localhost:11434");

        Console.WriteLine("Model: " + model);
        Console.WriteLine("Ollama Uri: " + uri.ToString());

        var builder = Kernel.CreateBuilder();
        builder.AddOllamaChatCompletion(model, uri);

        var kernel = builder.Build();

        do
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("What would you like to know?");

            var userPropmt = Console.ReadLine();

            var result = await kernel.InvokePromptAsync(userPropmt);
            Console.WriteLine(result);

            Console.WriteLine("\n\nWould you like to exit? y/n");
        } while (exit == Console.ReadLine());
    }
}