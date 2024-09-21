namespace WebAPI.Infrastructure.Interfaces
{
    public interface IRefreshProvider
    {
        string GenerateRefreshToken();
    }
}
