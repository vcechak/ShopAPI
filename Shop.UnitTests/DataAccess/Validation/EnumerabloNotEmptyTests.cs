using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.DataAccess.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.UnitAndIntegrationTests.DataAccess.Validation;

[TestClass]
public class EnumerabloNotEmptyTests
{
    [TestMethod]
    public void EnumerableNotEmpty_ValidatesCorrectly_WhenListIsNotEmpty()
    {
        // Arrange
        var validator = new EnumerableNotEmpty();
        var testObject = new TestClass
        {
            PropertyA = new List<object>
            {
                new object() {}
            },
            PropertyB = string.Empty,
        };
        var validationContext = new ValidationContext(testObject.PropertyA);

        // Act
        var result = validator.GetValidationResult(testObject.PropertyA, validationContext);

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    [TestMethod]
    public void EnumerableNotEmpty_ValidatesCorrectly_WhenListIsEmpty()
    {
        // Arrange
        var validator = new EnumerableNotEmpty();
        var testObject = new TestClass
        {
            PropertyA = new List<string>(),
            PropertyB = string.Empty,
        };
        var validationContext = new ValidationContext(testObject.PropertyA);

        // Act
        var result = validator.GetValidationResult(testObject.PropertyA, validationContext);

        // Assert
        result.Should().NotBe(ValidationResult.Success);
    }

    [TestMethod]
    public void EnumerableNotEmpty_ValidatesCorrectly_WhenListIsNull()
    {
        // Arrange
        var validator = new EnumerableNotEmpty();
        var testObject = new TestClass
        {
            PropertyA = null,
            PropertyB = string.Empty,
        };
        var validationContext = new ValidationContext(testObject)
        {
            MemberName = nameof(testObject.PropertyA)
        };

        // Act
        var result = validator.GetValidationResult(testObject.PropertyA, validationContext);

        // Assert
        result.Should().NotBe(ValidationResult.Success);
    }

    private class TestClass
    {
        [EnumerableNotEmpty]
        public IEnumerable<object> PropertyA { get; set; }
        public string PropertyB { get; set; } = string.Empty;
    }
}

