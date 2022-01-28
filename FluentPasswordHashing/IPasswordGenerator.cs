
namespace FluentPasswordHashing
{
    public interface IPasswordGenerator
    {
        Task<string> Generate(string password);

        Task<bool> VerifyPassword(string hash, string password);
    }
}
