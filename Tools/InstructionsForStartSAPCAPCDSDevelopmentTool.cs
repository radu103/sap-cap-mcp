using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace SAPCAPTools.Tools
{
    [McpServerToolType]
    public static class InstructionsForStartSAPCAPCDSDevelopmentTool
    {
        // Create a static logger for this class
        private static readonly ILogger logger = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .CreateLogger(nameof(InstructionsForStartSAPCAPCDSDevelopmentTool));

        static InstructionsForStartSAPCAPCDSDevelopmentTool()
        {
            logger.LogInformation("InstructionsForStartSAPCAPCDSDevelopmentTool static constructor called");
        }

        [McpServerTool, Description("Instructions for starting SAP CAP CDS Development")]
        public static List<string>  InstructionsForStartSAPCAPCDSDevelopment(
            [Description("Get and execute instructions for starting SAP CAP CDS Development")] bool getSystemPrompts
        )
        {
            logger.LogInformation("InstructionsForStartSAPCAPCDSDevelopmentTool called with getSystemPrompts: {getSystemPrompts}", getSystemPrompts);
            var systemPrompts = new List<string>();
            if(getSystemPrompts)
            {
                logger.LogInformation("Reading system prompts from Resources...");
                
                var assembly = typeof(InstructionsForStartSAPCAPCDSDevelopmentTool).Assembly;
                using var stream = assembly.GetManifestResourceStream("SAPCAPTools.Resources.system_prompts.txt");
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    systemPrompts = reader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                {
                    logger.LogError("System prompts embedded resource not found");
                    throw new McpException("System prompts embedded resource not found", 500);
                }
                logger.LogInformation("Loaded {count} system prompts", systemPrompts.Count);
                
                logger.LogInformation("System prompts initialized successfully.");
            }
            else
            {
                logger.LogWarning("No system prompts to initialize.");
            }
            return systemPrompts;
        }
    }
}