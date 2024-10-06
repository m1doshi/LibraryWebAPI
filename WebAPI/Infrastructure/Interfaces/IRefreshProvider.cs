namespace WebAPI.Infrastructures.Interfaces
{
    public interface IRefreshProvider
    {
        string GenerateRefreshToken();
    }
}
