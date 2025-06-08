# .NET MCP Client for Anthropic

A .NET console application that connects to a Model Context Protocol (MCP) filesystem server and integrates with Anthropic's Claude API to provide AI-powered file system operations.

## Features

- Connects to MCP filesystem server
- Provides AI chat interface with file system tools
- Supports file operations like reading, writing, editing, and directory management
- Real-time streaming responses from Claude

## Prerequisites

- .NET 9.0 or later
- Node.js (for running the MCP filesystem server)
- Anthropic API key

## Setup

### 1. Clone and Build

```bash
git clone <repository-url>
cd AnotherDotNetMcpClient
dotnet build
```

### 2. Configure API Key

Create a `.env` file in the `AnotherDotNetMcpClient` project directory:

```
ANTHROPIC_API_KEY=your-anthropic-api-key-here
```

Replace `your-anthropic-api-key-here` with your actual Anthropic API key (starts with `sk-ant-`).

### 3. Configure File System Access

Edit `Program.cs` and update the directory path in the `GetCommandAndArguments` method:

```csharp
=> ("node", new[] { Path.Combine(dir, "dist", "index.js"), @"D:\Jesus\DAM" }),
```

Change `@"D:\Jesus\DAM"` to the directory you want the MCP server to have access to.

### 4. MCP Server Setup

Make sure you have the MCP filesystem server available at:
```
D:\DEV\projects\agents\anthropic\McpServersOfficialRepo\servers\src\filesystem
```

The server should have a compiled `dist/index.js` file.

## Running the Application

Navigate to the project directory and run:

```bash
cd AnotherDotNetMcpClient
dotnet run
```

## Usage

1. The application will start and connect to the MCP filesystem server
2. You'll see a list of available tools (read_file, write_file, list_directory, etc.)
3. Type your commands at the prompt to interact with the AI
4. The AI can perform file operations on the configured directory
5. Type `exit` to quit the application

## Available MCP Tools

- `read_file` - Read contents of a file
- `read_multiple_files` - Read multiple files at once
- `write_file` - Write content to a file
- `edit_file` - Edit an existing file
- `create_directory` - Create a new directory
- `list_directory` - List directory contents
- `directory_tree` - Show directory structure as a tree
- `move_file` - Move or rename files
- `search_files` - Search for files by content or name
- `get_file_info` - Get file metadata
- `list_allowed_directories` - Show directories the server can access

## Configuration

### File System Access Directory

The directory that the MCP server has access to is configured in the `GetCommandAndArguments` method in `Program.cs`. Currently set to:

```
D:\Jesus\DAM
```

To change this, update the path in the method and rebuild the application.

### MCP Server Location

The MCP filesystem server is expected to be located at:

```
D:\DEV\projects\agents\anthropic\McpServersOfficialRepo\servers\src\filesystem
```

Make sure this path exists and contains the compiled server (`dist/index.js`).

## Troubleshooting

### "Invalid API Key" Error
- Ensure your `.env` file contains a valid Anthropic API key
- Make sure the key starts with `sk-ant-`

### "MCP server process exited unexpectedly"
- Check that the MCP server path is correct
- Ensure the directory you're giving access to exists
- Verify Node.js is installed and accessible

### Connection Issues
- Ensure the MCP filesystem server is built and the `dist/index.js` file exists
- Check that the configured access directory exists and is readable
