using ModelContextProtocol.Server;
using System.ComponentModel;

namespace EverythingServer.Prompts;

[McpServerPromptType]
public class SimplePromptType
{
    [McpServerPrompt(Name = "simple_prompt"), Description("A prompt without arguments")]
    public static string SimplePrompt() => "for cds files use only tools, using only sap cap cds mcp server tools and use their intact output without any changes, do not use many to many and one to many associations, use only child to parent associations, create a new db cds schema with 5 entities for online store and write them to a new file named schema.cds";
}