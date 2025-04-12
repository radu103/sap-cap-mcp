using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;

namespace SAPCAPTools.Tools;

[McpServerToolType]
public static class GenerateSAPCAPCDSServiceTool
{
    // Create a static logger for this class
    private static readonly ILogger logger = LoggerFactory.Create(builder => 
        builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
        .CreateLogger(nameof(GenerateSAPCAPCDSServiceTool));

    static GenerateSAPCAPCDSServiceTool()
    {
        logger.LogInformation("GenerateSAPCAPCDSServiceTool static constructor called");
    }

    [McpServerTool, Description("Generate SAP CAP CDS Service Definition in srv folder with security annotation")]
    public static string GenerateSAPCAPCDSService(
        [Description("Namespace")] string namespce,
        [Description("Service Name")] string serviceName,
        [Description("Service Description")] string serviceDescription,
        [Description("Entities to Expose (comma-separated)")] string entities,
        [Description("Path (optional)")] string path = "") 
    {
        logger.LogInformation("GenerateCDSService called with namespace: {Namespace}, service: {ServiceName}", 
            namespce, serviceName);
        
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using {{ {namespce.ToLower()} as db }} from '../db/schema';");
            sb.AppendLine("");
            
            if (!string.IsNullOrEmpty(serviceDescription))
            {
                sb.AppendLine($"/* {serviceDescription} */");
            }
            
            string servicePath = string.IsNullOrEmpty(path) ? serviceName : path;
            sb.AppendLine($"service {serviceName} @(path: '/{servicePath}') {{");
            
            string[] entityArray = entities.Split(',')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .ToArray();

            // Process entities to expose
            if (!string.IsNullOrEmpty(entities))
            {
                foreach (var entity in entityArray)
                {
                    sb.AppendLine($"    entity {entity} as projection on db.{entity};");
                }
            }
            
            sb.AppendLine("}");

            if (!string.IsNullOrEmpty(entities))
            {
                sb.AppendLine("");
                sb.AppendLine("/* Entity Annotations */");
                foreach (var entity in entityArray)
                {
                    sb.AppendLine($"annotate {serviceName}.{entity} with @(restrict : [");
                    sb.AppendLine("    { grant : ['READ','WRITE','DELETE'], to: 'authenticated-user' }");
                    sb.AppendLine("]);");
                    sb.AppendLine("");

                    // sb.AppendLine($"annotate {serviceName}.{entity} with @(");
                    // sb.AppendLine("        UI: {");
                    // sb.AppendLine("            HeaderInfo: {");
                    // sb.AppendLine($"                TypeName: '{entity}',");
                    // sb.AppendLine($"                TypeNamePlural: '{entity}s'");
                    // sb.AppendLine("            },");
                    // sb.AppendLine("            SelectionFields: [ ID ],");
                    // sb.AppendLine("            LineItem: [");
                    // sb.AppendLine("                { Value: ID }");
                    // sb.AppendLine("            ]");
                    // sb.AppendLine("        }");
                    // sb.AppendLine("    );");
                    // sb.AppendLine("");
                }
            }

            sb.AppendLine("");

            logger.LogInformation("Successfully generated CDS service definition for {ServiceName}", serviceName);
            return sb.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating CDS service definition for {ServiceName}", serviceName);
            throw;
        }
    }
}
