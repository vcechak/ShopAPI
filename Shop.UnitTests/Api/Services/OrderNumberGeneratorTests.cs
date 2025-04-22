using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Api.Services;
using Shop.DataAccess.Data;
using System.Threading.Tasks;

namespace Shop.UnitAndIntegrationTests.Api.Services;

[TestClass]
public class OrderNumberGeneratorTests
{
    private DbContextOptions<ShopDbContext> _options;

    [TestInitialize]
    public void TestInitialize()
    {
        // Configure a new SQLite in-memory database for each test
        _options = new DbContextOptionsBuilder<ShopDbContext>()
            .UseSqlite("Filename=:memory:") // Use SQLite in-memory database
            .Options;
    }

    [TestMethod]
    public async Task GenerateOrderNumberAsync_ShouldGenerateUniqueOrderNumber()
    {
        // Arrange
        using var context = new ShopDbContext(_options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var orderNumberGenerator = new OrderNumberGenerator(context);

        // Act
        var orderNumber1 = await orderNumberGenerator.GenerateOrderNumberAsync();
        var orderNumber2 = await orderNumberGenerator.GenerateOrderNumberAsync();

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(orderNumber1));
        Assert.IsFalse(string.IsNullOrEmpty(orderNumber2));
        Assert.AreNotEqual(orderNumber1, orderNumber2);
    }
}