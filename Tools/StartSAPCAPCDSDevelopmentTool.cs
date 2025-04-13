using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace SAPCAPTools.Tools
{
    [McpServerToolType]
    public static class StartSAPCAPCDSDevelopmentTool
    {
        // Create a static logger for this class
        private static readonly ILogger logger = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .CreateLogger(nameof(StartSAPCAPCDSDevelopmentTool));

        static StartSAPCAPCDSDevelopmentTool()
        {
            logger.LogInformation("StartSAPCAPCDSDevelopmentTool static constructor called");
        }

        [McpServerTool, Description("Task suggested steps for Starting SAP CAP CDS Development")]
        public static List<string>  GetAndExecuteTaskStepsForStartSAPCAPCDSDevelopment(
            [Description("Get and execute instructions for starting SAP CAP CDS Development")] bool getTaskSuggestedSteps
        )
        {
            logger.LogInformation("InstructionsForStartSAPCAPCDSDevelopmentTool called with getTaskSuggestedSteps: {getTaskSuggestedSteps}", getTaskSuggestedSteps);
            var taskSteps = new List<string>();
            if(getTaskSuggestedSteps)
            {
                logger.LogInformation("Reading task steps from Resources...");
                
                var assembly = typeof(InstructionsForStartSAPCAPCDSDevelopmentTool).Assembly;
                using var stream = assembly.GetManifestResourceStream("SAPCAPTools.Resources.start_sap_cap_cds.txt");
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    taskSteps = reader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                {
                    logger.LogError("Task steps embedded resource not found");
                    throw new McpException("Task steps embedded resource not found", 500);
                }
                logger.LogInformation("Loaded {count} task steps", taskSteps.Count);
                
                logger.LogInformation("Task steps initialized successfully.");
            }
            else
            {
                logger.LogWarning("No task steps to initialize.");
            }
            return taskSteps;
        }
    }
}