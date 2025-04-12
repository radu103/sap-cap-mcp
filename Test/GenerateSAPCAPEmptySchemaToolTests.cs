using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAPCAPTools.Tools;

namespace SAPCAPTools.Tests
{
    [TestClass]
    public class GenerateSAPCAPEmptySchemaToolTests
    {
        [TestMethod]
        public void GenerateEmptySchema_WithValidNamespace_ReturnsCorrectSchema()
        {
            // Arrange
            string testNamespace = "test.namespace";
            string expected = 
                "namespace test.namespace;\r\n" +
                "\r\n" +
                "using { cuid, managed } from '@sap/cds/common';\r\n" +
                "\r\n";

            // Act
            string result = GenerateSAPCAPEmptySchemaTool.GenerateSAPCAPCDSEmptySchema(testNamespace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GenerateEmptySchema_WithUppercaseNamespace_ReturnsLowercasedNamespace()
        {
            // Arrange
            string testNamespace = "TEST.NAMESPACE";
            string expected = 
                "namespace test.namespace;\r\n" +
                "\r\n" +
                "using { cuid, managed } from '@sap/cds/common';\r\n" +
                "\r\n";

            // Act
            string result = GenerateSAPCAPEmptySchemaTool.GenerateSAPCAPCDSEmptySchema(testNamespace);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GenerateEmptySchema_WithEmptyNamespace_ReturnsSchemaWithEmptyNamespace()
        {
            // Arrange
            string testNamespace = "";
            string expected = 
                "namespace ;\r\n" +
                "\r\n" +
                "using { cuid, managed } from '@sap/cds/common';\r\n" +
                "\r\n";

            // Act
            string result = GenerateSAPCAPEmptySchemaTool.GenerateSAPCAPCDSEmptySchema(testNamespace);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}