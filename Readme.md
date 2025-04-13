# SAP CAP MCP Server

This is a Model Context Protocol (MCP) server for SAP Cloud Application Programming (CAP) development, providing specialized tools to generate CDS (Core Data Services) schemas and entities.

## Features

- Generate empty CDS schema files with proper namespace and imports
- Create fully featured SAP CAP entities with properties and associations
- Support for managed entities with automatic timestamps and user tracking
- Tools accessible through the Model Context Protocol

## Deployment as Local Executable

Follow these steps to deploy the SAP CAP MCP server as a self-contained local executable with all dependencies embedded:

### Prerequisites

- .NET SDK 8.0 or later
- Windows, macOS, or Linux operating system

### Build Steps

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd sap-cap-mcp
   ```

2. **Build as self-contained executable**

   For Windows:
   ```cmd
   dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
   rm -r ./deploy/*.*
   cp ./bin/Release/net8.0/win-x64/publish/sap-cap-mcp.exe ./deploy
   cp ./bin/Release/net8.0/win-x64/publish/appsettings.json ./deploy
   ```

3. **Find the executable**

   The executable will be generated in the `bin/Release/net8.0/{runtime-identifier}/publish/` directory.

   - Windows: `sap-cap-mcp.exe`
   - macOS/Linux: `sap-cap-mcp`

4. **Make executable (macOS/Linux only)**

   ```bash
   chmod +x bin/Release/net8.0/linux-x64/publish/sap-cap-mcp
   ```

### Running the Server

Run the executable directly:

```bash
# Windows
.\bin\Release\net8.0\win-x64\publish\sap-cap-mcp.exe

# macOS/Linux
./bin/Release/net8.0/linux-x64/publish/sap-cap-mcp
```

Configure the server using environment variables or command-line parameters:

```bash
# Example with environment variables
$env:ASPNETCORE_ENVIRONMENT="Production"
$env:ASPNETCORE_URLS="http://localhost:5123"
.\bin\Release\net8.0\win-x64\publish\sap-cap-mcp.exe
```

### Available Tools

The SAP CAP MCP server provides the following tools:

1. **GenerateEmptySchema**
   - Creates a new CDS schema file with proper namespace and imports
   - Parameter: `namespace` - The namespace for your CDS schema

2. **GenerateSAPCAPEntity**
   - Creates a new entity definition for SAP CAP CDS
   - Parameters:
     - `entityName` - Name of the entity
     - `entityDescription` - Description of the entity
     - `entityIsManaged` - Whether the entity inherits from managed aspect
     - `entityProperties` - Array of properties with type, length, etc.
     - `entityAssociations` - Array of associations to other entities

## Development

To work on the SAP CAP MCP server:

1. Clone the repository
2. Open the solution in Visual Studio or VS Code
3. Restore dependencies: `dotnet restore`
4. Run the server: `dotnet run`

## Testing

Run the tests using:

```bash
dotnet test
```

## License

[Specify your license information here]