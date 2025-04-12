using CdsServiceClient.Model;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using SAPCAPTools.StringHelpers;
using System.ComponentModel;
using System.Text;

namespace SAPCAPTools.Tools
{
    [McpServerToolType]
    public static class GenerateSAPCAPEntityTool
    {
        // Create a static logger for this class
        private static readonly ILogger logger = LoggerFactory.Create(builder =>
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .CreateLogger(nameof(GenerateSAPCAPEntityTool));

        static GenerateSAPCAPEntityTool()
        {
            logger.LogInformation("GenerateSAPCAPEntityTool static constructor called");
        }

        [McpServerTool, Description("Generate SAP CDS DB Entity without Associations")]
        public static string GenerateSAPCAPEntity(
            [Description("Entity Name")] string entityName,
            [Description("Entity is managed")] bool entityIsManaged,
            [Description("Entity Description")] string entityDescription,
            [Description("Entity Properties as List of Cds Entity field structure : ")]
            List<CdsEntityField> entityProperties)
        {
            logger.LogDebug("GenerateSAPCAPEntity called with entityName: {EntityName}", entityName);
            logger.LogDebug("Entity is managed: {IsManaged}", entityIsManaged);
            logger.LogDebug("Entity description: {Description}", entityDescription);
            logger.LogDebug("Properties count: {Count}", entityProperties?.Count ?? 0);

            try
            {
                // Validate inputs
                entityProperties = entityProperties ?? new List<CdsEntityField>();

                var fieldsLength = entityProperties.Count > 0 ? entityProperties.Select(f => f.Name.Length).Max() : 30;

                // Ensure the lengths are at least 30 characters
                var maxLength = Math.Max(30, fieldsLength);

                // Generate the entity definition
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"/* {entityDescription} */");
                sb.Append($"entity {entityName.ToUpperFirstChar()} : cuid");
                sb.AppendLine(entityIsManaged.Equals(true) ? ", managed {" : " {");

                // add property fields
                foreach (var property in entityProperties)
                {
                    logger.LogDebug("Processing property: {PropertyName}, Type: {PropertyType}, Nullable: {IsNullable}",
                        property.Name, property.Type, property.IsNullable);

                    var propName = property.Name.ToLowerFirstChar().PadRight(maxLength);
                    var propType = property.Type.ToUpperFirstChar();

                    int lineLength = 0;
                    if (property.Type.ToLower().Equals("decimal"))
                    {
                        if (!string.IsNullOrEmpty(property.Precision))
                        {
                            var str = $"  {propName} : {propType}({property.Length},{property.Precision})";
                            sb.Append(str);
                            lineLength += str.Length;
                        }
                        else
                        {
                            var str = $"  {propName} : {propType}({property.Length})";
                            sb.Append(str);
                            lineLength += str.Length;
                        }
                        if (!property.IsNullable)
                        {
                            sb.Append(" not null");
                            lineLength += " not null".Length;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(property.Length))
                        {
                            var str = $"  {propName} : {propType}({property.Length})";
                            sb.Append(str);
                            lineLength += str.Length;
                        }
                        else
                        {
                            var str = $"  {propName} : {propType}";
                            sb.Append(str);
                            lineLength += str.Length;
                        }
                        if (!property.IsNullable)
                        {
                            sb.Append(" not null");
                            lineLength += " not null".Length;
                        }
                    }
                    sb.Append(";");
                    lineLength += 1;

                    // add padding to the line to make it 70 characters long
                    for (int i = lineLength; i < 70; i++)
                    {
                        sb.Append(" ");
                    }
                    sb.AppendLine($"// {property.Description}");
                }

                // close the entity definition
                sb.AppendLine("}");

                logger.LogInformation("Successfully generated entity definition for {EntityName}", entityName);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating entity definition for {EntityName}", entityName);
                throw new McpException($"Error generating entity definition: {ex.Message}", 500);
            }
        }

        [McpServerTool, Description("Generate Custom Associations for SAP CDS DB Entity with ID field and Asssociation field better than best practice without on specific field and without ID field")]
        public static string GenerateSAPCAPEntityAssociations(
            [Description("Entity Name")] string entityName,
            [Description("Entity Associations as List of strings, associations of type many should not be used")] List<string> entityAssociations,
            [Description("Existing Entities in Database Schema are mandatory for Associations")] List<string> existingEntities)
        {
            logger.LogDebug("GenerateSAPCAPEntityAssociations called with entityName: {EntityName}", entityName);
            logger.LogDebug("Associations count: {Count}", entityAssociations?.Count ?? 0);
            logger.LogDebug("Existing entities count: {Count}", existingEntities?.Count ?? 0);

            try
            {
                // Validate inputs
                entityAssociations = entityAssociations ?? new List<string>();
                existingEntities = existingEntities ?? new List<string>();

                // Validate associations
                if(entityAssociations.Count > 0 && existingEntities.Count == 0)
                {
                    logger.LogWarning("Associations provided but no existing entities to validate against.");
                    throw new McpException("Associations provided but no existing entities to validate against.", 400);
                }
                ValidateAssociations(entityAssociations, existingEntities);

                var associationsLength = entityAssociations.Count > 0 ? entityAssociations.Select(f => f.Length).Max() + 2 : 30; // +2 for ID

                // Generate the entity definition
                StringBuilder sb = new StringBuilder();
                
                // add associations - direct output of association strings
                sb.AppendLine();
                foreach (var association in entityAssociations)
                {
                    logger.LogDebug("Processing association: {Association}", association);
                    if (association.IndexOf(":") >= 0 || association.IndexOf(" ") >= 0)
                    {
                        logger.LogWarning("Association {Association} is not a valid association name :", association);
                        return "Association {Association} is not a valid association name because it is not am entity name";
                    }

                    var associationField = association.ToLowerFirstChar();
                    var associationEntity = association.ToUpperFirstChar();
                    sb.AppendLine($"  {(associationField + "ID").PadRight(associationsLength)} : UUID;");
                    sb.AppendLine($"  {associationField.PadRight(associationsLength)} : Association to {associationEntity} on {associationField}.ID = $self.{associationField}ID;");
                }

                logger.LogInformation("Successfully generated entity associations definition for {EntityName}", entityName);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating entity associations definition for {EntityName}", entityName);
                throw new McpException($"Error generating entity associations definition: {ex.Message}", 500);
            }
        }

        private static void ValidateAssociations(List<string> entityAssociations, List<string> existingEntities)
        {
            foreach (var association in entityAssociations)
            {
                if (!existingEntities.Contains(association.ToUpperFirstChar()))
                {
                    logger.LogWarning("Association {Association} is not a valid association name", association);
                    throw new McpException($"Association {association} is not a valid association name because it is not an entity name", 400);
                }
            }
        }
    }
}