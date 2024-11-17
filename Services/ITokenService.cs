namespace WebApplication1.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string role);

    }
}
