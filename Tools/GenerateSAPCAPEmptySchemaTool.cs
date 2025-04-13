using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;

namespace SAPCAPTools.Tools;

[McpServerToolType]
public static class GenerateSAPCAPEmptySchemaTool
{
    // Create a static logger for this class
    private static readonly ILogger logger = LoggerFactory.Create(builder => 
        builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
        .CreateLogger(nameof(GenerateSAPCAPEmptySchemaTool));

    static GenerateSAPCAPEmptySchemaTool()
    {
        logger.LogInformation("GenerateSAPCAPEmptySchemaTool static constructor called");
    }

    [McpServerTool, Description("Generate SAP CDS DB Empty Schema")]
    public static string GenerateSAPCAPCDSEmptySchema([Description("Namespace")] string namespce) 
    {
        logger.LogInformation("GenerateEmptySchema called with namespace: {Namespace}", namespce);
        
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {namespce.ToLower()};"); // Generate the namespace
            sb.AppendLine("");
            sb.AppendLine($"using {{ cuid, managed }} from '@sap/cds/common';");
            sb.AppendLine("");
            
            logger.LogInformation("Successfully generated empty schema with namespace {Namespace}", namespce);
            return sb.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating empty schema for namespace {Namespace}", namespce);
            throw;
        }
    }
}