namespace Shop.Api.Services
{
    public interface IOrderNumberGenerator
    {
        Task<string> GenerateOrderNumberAsync();
    }
}
