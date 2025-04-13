using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace SAPCAPTools.Tools
{
    [McpServerToolType]
    public static class SAPCAPCDSInitApplicationStructureTool
    {
        // Create a static logger for this class
        private static readonly ILogger logger = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .CreateLogger(nameof(SAPCAPCDSInitApplicationStructureTool));

        static SAPCAPCDSInitApplicationStructureTool()
        {
            logger.LogInformation("SAPCAPCDSInitApplicationStructureTool static constructor called");
        }

        [McpServerTool, Description("Initialize SAP CAP CDS Application Structure")]
        public static List<string> SAPCAPCDSInitializeApplicationStructure(
            [Description("Project name for the SAP CAP CDS application")] string projectName,
            [Description("Initialize application structure with recommended folders and files")] bool initializeStructure = true
        )
        {
            logger.LogInformation("InitializeApplicationStructure called with projectName: {projectName}, initializeStructure: {initializeStructure}", 
                projectName, initializeStructure);
            
            var initSteps = new List<string>();
            
            if (string.IsNullOrWhiteSpace(projectName))
            {
                logger.LogError("Project name cannot be empty");
                throw new McpException("Project name cannot be empty", 400);
            }

            if (initializeStructure)
            {
                logger.LogInformation("Reading initialization steps from Resources...");
                
                var assembly = typeof(SAPCAPCDSInitApplicationStructureTool).Assembly;
                using var stream = assembly.GetManifestResourceStream("SAPCAPTools.Resources.init_sap_cap_structure.txt");
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    initSteps = reader.ReadToEnd()
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                }
                else
                {
                    logger.LogError("Initialization steps embedded resource not found");
                    throw new McpException("Initialization steps embedded resource not found", 500);
                }
                
                logger.LogInformation("Loaded {count} initialization steps", initSteps.Count);
                logger.LogInformation("Initialization steps prepared successfully.");
            }
            else
            {
                logger.LogWarning("No initialization requested.");
                initSteps.Add($"To initialize the SAP CAP CDS application structure for {projectName}, set initializeStructure to true.");
            }
            
            return initSteps;
        }
    }
}
