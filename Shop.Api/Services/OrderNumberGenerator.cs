using Microsoft.EntityFrameworkCore;
using Shop.DataAccess.Data;
using Shop.DataAccess.Models;

namespace Shop.Api.Services
{
    public class OrderNumberGenerator : IOrderNumberGenerator
    {
        private readonly ShopDbContext _shopDbContext;

        public OrderNumberGenerator(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        // Note - solution picked for SQLLite, for SQL I would pick a Sequence approach in db
        public async Task<string> GenerateOrderNumberAsync()
        {
            var today = DateTime.UtcNow.Date;

            using var transaction = await _shopDbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            var sequence =  await _shopDbContext.OrderNumberSequences.SingleOrDefaultAsync(s => s.Date == today);
            if (sequence == null)
            {
                sequence = new OrderNumberSequence
                {
                    Date = today,
                    Counter = 1
                };
                _shopDbContext.OrderNumberSequences.Add(sequence);
            }
            else
            {
                sequence.Counter++;
            }

            await _shopDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return $"ORD-{sequence.Date.ToString("yyyyMMdd")}-{sequence.Counter:D4}";
        }
    }
}
