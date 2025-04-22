using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.DataAccess.Validation;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;

namespace Shop.UnitAndIntegrationTests.DataAccess.Validation;

[TestClass]
public class AtLeastOneRequiredTests
{
    [TestMethod]
    public void AtLeastOneRequired_ValidatesCorrectly_WhenAtLeastOnePropertyIsSet()
    {
        // Arrange
        var validator = new AtLeastOneRequired(nameof(TestClass.PropertyA), nameof(TestClass.PropertyB));
        var testObject = new TestClass
        {
            PropertyA = "Test",
            PropertyB = string.Empty,
            PropertyC = string.Empty
        };
        var validationContext = new ValidationContext(testObject);

        // Act
        var result = validator.GetValidationResult(testObject, validationContext);

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    [TestMethod]
    public void AtLeastOneRequired_ValidatesCorrectly_WhenNoPropertiesAreSet()
    {
        // Arrange
        var validator = new AtLeastOneRequired(nameof(TestClass.PropertyA), nameof(TestClass.PropertyB));
        var testObject = new TestClass
        {
            PropertyA = string.Empty,
            PropertyB = string.Empty,
            PropertyC = string.Empty
        };
        var validationContext = new ValidationContext(testObject);

        // Act
        var result = validator.GetValidationResult(testObject, validationContext);

        // Assert
        result.Should().NotBe(ValidationResult.Success);
    }

    [AtLeastOneRequired(nameof(PropertyA), nameof(PropertyB), ErrorMessage = "You must enter either Customer Name or Company Name.")]
    private class TestClass
    {
        public string PropertyA { get; set; } = string.Empty;

        public string PropertyB { get; set; } = string.Empty;

        public string PropertyC { get; set; } = string.Empty;
    }
}
