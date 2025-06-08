using Anthropic.SDK;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

// Get command and arguments for MCP server
var (command, arguments) = GetCommandAndArguments([@"D:\DEV\projects\agents\anthropic\McpServersOfficialRepo\servers\src\filesystem"]);

var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "Dotnet File - System - MCP Client",
    Command = command,
    Arguments = arguments,
});

await using var mcpClient = await McpClientFactory.CreateAsync(clientTransport);

var tools = await mcpClient.ListToolsAsync();
foreach (var tool in tools)
{
    Console.WriteLine($"Connected to server with tools: {tool.Name}");
}

// Create the Anthropic client with function invocation support
var anthropicClient = new AnthropicClient(new APIAuthentication(builder.Configuration["ANTHROPIC_API_KEY"]))
    .Messages
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

var options = new ChatOptions
{
    MaxOutputTokens = 1000,
    ModelId = "claude-3-5-sonnet-20241022",
    Tools = [.. tools]
};

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("MCP Client Started!");
Console.ResetColor();

PromptForInput();
while(Console.ReadLine() is string query && !"exit".Equals(query, StringComparison.OrdinalIgnoreCase))
{
    if (string.IsNullOrWhiteSpace(query))
    {
        PromptForInput();
        continue;
    }

    await foreach (var message in anthropicClient.GetStreamingResponseAsync(query, options))
    {
        Console.Write(message);
    }
    Console.WriteLine();

    PromptForInput();
}

static void PromptForInput()
{
    Console.WriteLine("Enter a command (or 'exit' to quit):");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("> ");
    Console.ResetColor();
}

static (string command, string[] arguments) GetCommandAndArguments(string[] args)
{
    return args switch
    {
        [var script] when script.EndsWith(".py") 
            => ("python", args),

        [var script] when script.EndsWith(".js")  
            => ("node", args),

        [var script] when script.EndsWith(".ts")  
            => ("ts-node", args),

        [var dir] when Directory.Exists(dir) 
                       && File.Exists(Path.Combine(dir, "dist", "index.js"))  
            => ("node", new[] { Path.Combine(dir, "dist", "index.js") }),

        [var script] when Directory.Exists(script)
                          || (File.Exists(script) && script.EndsWith(".csproj"))  
            => ("dotnet", new[] { "run", "--project", script, "--no-build" }),

        _ => throw new NotSupportedException(
            "An unsupported server script was provided. Supported scripts are .py, .js, .ts, JS builds in dist/, or .csproj")
    };
}