using Microsoft.VisualStudio.TestTools.UnitTesting;
using CdsServiceClient.Model;
using SAPCAPTools.Tools;

namespace SAPCAPTools.Tests
{
    [TestClass]
    public class GenerateSAPCAPEntityToolTests
    {
        [TestMethod]
        public void GenerateSAPCAPEntity_BasicEntity_ReturnsCorrectCdsDefinition()
        {
            // Arrange
            string entityName = "Product";
            bool entityIsManaged = true;
            string entityDescription = "Products available in the shop";
            
            var entityProperties = new List<CdsEntityField>
            {
                new CdsEntityField 
                { 
                    Name = "name", 
                    Type = "String", 
                    Length = "100", 
                    IsNullable = false, 
                    Description = "Product name" 
                },
                new CdsEntityField 
                { 
                    Name = "description", 
                    Type = "String", 
                    Length = "1000", 
                    IsNullable = true, 
                    Description = "Product description" 
                }
            };
            
            var entityAssociations = new List<string>();
            var existingEntities = new List<string>();

            // Act
            string result = GenerateSAPCAPEntityTool.GenerateSAPCAPEntity(
                entityName, 
                entityIsManaged, 
                entityDescription, 
                entityProperties, 
                entityAssociations,
                existingEntities);

            // Assert
            Assert.IsTrue(result.Contains("/* Products available in the shop */"));
            Assert.IsTrue(result.Contains("entity Product : cuid, managed {"));
            Assert.IsTrue(result.Contains("name"));
            Assert.IsTrue(result.Contains("String(100) not null"));
            Assert.IsTrue(result.Contains("// Product name"));
            Assert.IsTrue(result.Contains("description"));
            Assert.IsTrue(result.Contains("String(1000)"));
            Assert.IsTrue(result.Contains("// Product description"));
        }

        [TestMethod]
        public void GenerateSAPCAPEntity_WithDecimalProperty_ReturnsCorrectCdsDefinition()
        {
            // Arrange
            string entityName = "Price";
            bool entityIsManaged = true;
            string entityDescription = "Product prices";
            
            var entityProperties = new List<CdsEntityField>
            {
                new CdsEntityField 
                { 
                    Name = "amount", 
                    Type = "Decimal", 
                    Length = "10", 
                    Precision = "2",
                    IsNullable = false, 
                    Description = "Price amount" 
                }
            };
            
            var entityAssociations = new List<string>();
            var existingEntities = new List<string>();

            // Act
            string result = GenerateSAPCAPEntityTool.GenerateSAPCAPEntity(
                entityName, 
                entityIsManaged, 
                entityDescription, 
                entityProperties, 
                entityAssociations,
                existingEntities);

            // Assert
            Assert.IsTrue(result.Contains("/* Product prices */"));
            Assert.IsTrue(result.Contains("entity Price : cuid, managed {"));
            Assert.IsTrue(result.Contains("amount"));
            Assert.IsTrue(result.Contains("Decimal(10,2) not null"));
            Assert.IsTrue(result.Contains("// Price amount"));
        }

        [TestMethod]
        public void GenerateSAPCAPEntity_WithAssociations_ReturnsCorrectCdsDefinition()
        {
            // Arrange
            string entityName = "Order";
            bool entityIsManaged = true;
            string entityDescription = "Customer orders";
            
            var entityProperties = new List<CdsEntityField>
            {
                new CdsEntityField 
                { 
                    Name = "orderDate", 
                    Type = "DateTime", 
                    IsNullable = false, 
                    Description = "Order date" 
                }
            };
            
            var entityAssociations = new List<string>
            {
                "customer"
            };
            var existingEntities = new List<string> { "Customer" };

            // Act
            string result = GenerateSAPCAPEntityTool.GenerateSAPCAPEntity(
                entityName, 
                entityIsManaged, 
                entityDescription, 
                entityProperties, 
                entityAssociations,
                existingEntities);

            // Assert
            Assert.IsTrue(result.Contains("/* Customer orders */"));
            Assert.IsTrue(result.Contains("entity Order : cuid, managed {"));
            Assert.IsTrue(result.Contains("orderDate"));
            Assert.IsTrue(result.Contains("DateTime not null"));
            Assert.IsTrue(result.Contains("// Order date"));
            Assert.IsTrue(result.Contains("customerID : UUID;"));
            Assert.IsTrue(result.Contains("customer : Association to Customer on customer.ID = $self.customerID;"));
        }

        [TestMethod]
        public void GenerateSAPCAPEntity_WithoutManagedFlag_ReturnsCorrectCdsDefinition()
        {
            // Arrange
            string entityName = "TestEntity";
            bool entityIsManaged = false;
            string entityDescription = "Test entity without managed aspect";
            
            var entityProperties = new List<CdsEntityField>
            {
                new CdsEntityField 
                { 
                    Name = "testField", 
                    Type = "String", 
                    Length = "50", 
                    IsNullable = true
                }
            };
            
            var entityAssociations = new List<string>();
            var existingEntities = new List<string>();

            // Act
            string result = GenerateSAPCAPEntityTool.GenerateSAPCAPEntity(
                entityName, 
                entityIsManaged, 
                entityDescription, 
                entityProperties, 
                entityAssociations,
                existingEntities);

            // Assert
            Assert.IsTrue(result.Contains("/* Test entity without managed aspect */"));
            Assert.IsTrue(result.Contains("entity TestEntity : cuid {"));
            Assert.IsFalse(result.Contains("entity TestEntity : cuid, managed {"));
            Assert.IsTrue(result.Contains("testField"));
            Assert.IsTrue(result.Contains("String(50)"));
        }

        [TestMethod]
        public void GenerateSAPCAPEntity_WithoutLengthOrPrecision_ReturnsCorrectCdsDefinition()
        {
            // Arrange
            string entityName = "Rating";
            bool entityIsManaged = true;
            string entityDescription = "Product ratings";
            
            var entityProperties = new List<CdsEntityField>
            {
                new CdsEntityField 
                { 
                    Name = "score", 
                    Type = "Integer", 
                    IsNullable = false, 
                    Description = "Rating score" 
                }
            };
            
            var entityAssociations = new List<string>();
            var existingEntities = new List<string>();

            // Act
            string result = GenerateSAPCAPEntityTool.GenerateSAPCAPEntity(
                entityName, 
                entityIsManaged, 
                entityDescription, 
                entityProperties, 
                entityAssociations,
                existingEntities);

            // Assert
            Assert.IsTrue(result.Contains("/* Product ratings */"));
            Assert.IsTrue(result.Contains("entity Rating : cuid, managed {"));
            Assert.IsTrue(result.Contains("score"));
            Assert.IsTrue(result.Contains("Integer not null"));
            Assert.IsTrue(result.Contains("// Rating score"));
            Assert.IsFalse(result.Contains("Integer()"));
        }
    }
}