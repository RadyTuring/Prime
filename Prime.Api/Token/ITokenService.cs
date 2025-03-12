

using Dto;

namespace Token;

public interface ITokenService
{
    Task<FeedBackReturn> RegisterAsync(UserDto model);
    Task<AuthModel> LoginAsync(UserLoginDto model);
    Task<AuthModel> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
    public bool isUserExist(string userName);
}