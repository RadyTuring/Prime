
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Settings;
using Dto;
using Entities;
using Services;
using Azure;

namespace Token;

public class TokenService : ITokenService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppService _appService;
    private string _imagesPath = "files\\images\\users\\";
    public TokenService(IUnitOfWork unitOfWork, IAppService appService, IOptions<JWT> jwt)
    {
        _unitOfWork = unitOfWork;
        _appService = appService;
        _jwt = jwt.Value;
    }

    public async Task<FeedBackReturn> RegisterAsync(UserDto model)
    {
        FeedBackReturn registerReturn = new FeedBackReturn();
        if (isUserExist(model.UserName))
        {
            registerReturn.Message = "User Name Already Exist";
            return (registerReturn);
        }
        string _extension = "", _defaultImage = "";
        if (model.ImageFile != null)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            _extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
            if (!imageExtensions.Contains(_extension))
            {
                registerReturn.Message = "Invalid Image File";
                return (registerReturn);
            }
            _defaultImage = _appService.Uploade(model.ImageFile, _imagesPath);
        }
        User newUser = new User()
        {
            UserName = model.UserName,
            FullName = model.FullName,
            IsActiveUser = true,
            CountryId = model.CountryId,
          
            RoleId = 1,
            IsNewUser = true,
            ImageFile = _defaultImage,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),

        };
        await _unitOfWork.Users.AddAsync(newUser);
        _unitOfWork.Save();
        registerReturn.Message = $"User {model.UserName} Created";
        registerReturn.IsSuccess = true;
        return registerReturn;

    }

    public async Task<AuthModel> LoginAsync(UserLoginDto model)
    {
        var authModel = new AuthModel();
        Entities.User _currentUser = null;

        _currentUser = await _unitOfWork.Users.FindAsync(m => m.UserName == model.UserName || m.AdminCode == model.UserName);

        if (_currentUser is null)
        {
            authModel.Message = "Invalid User ";
            return (authModel);
        }
        if (!_currentUser.IsActiveUser)
        {
            authModel.Message = "Blocked User ";
            return (authModel);
        }
        bool verified = BCrypt.Net.BCrypt.Verify(model.Password, _currentUser.PasswordHash);
        if (!verified)
        {
            authModel.Message = "Invalid User ";
            return (authModel);
        }
        authModel.IsAuthenticated = true;
        authModel.Message = "Success";
        authModel.UserId = _currentUser.UserId;
        authModel.RoleId = _currentUser.RoleId;
        authModel.IsNewUser = _currentUser.IsNewUser;
        authModel.ImageFile = _currentUser.ImageFile;
        authModel.LogOfflineTimes = _currentUser.LogOfflineTimes;

        _currentUser.IsOnline = true;

        _unitOfWork.Users.Update(_currentUser);

        var jwtSecurityToken = await CreateJwtToken(_currentUser);


        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        authModel.Username = _currentUser.UserName;
        authModel.ExpiresOn = jwtSecurityToken.ValidTo;

        var activeRefreshToken = _unitOfWork.RefreshTokens.Last(m => m.UserId == _currentUser.UserId, m => m.Id);
        if (activeRefreshToken != null && activeRefreshToken.IsActive)
        {
            activeRefreshToken.RevokedOn = DateTime.UtcNow;
            _unitOfWork.RefreshTokens.Update(activeRefreshToken);
            _unitOfWork.Save();
        }

        var refreshToken = GenerateRefreshToken();
        refreshToken.UserId = _currentUser.UserId;
        authModel.RefreshToken = refreshToken.Token;
        authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
        _unitOfWork.RefreshTokens.Add(refreshToken);
        _unitOfWork.Save();
        return (authModel);
    }
    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        var authModel = new AuthModel();
        var refreshToken = await _unitOfWork.RefreshTokens.FindAsync(m => m.Token == token);

        if (refreshToken == null)
        {
            authModel.Message = "Invalid token";
            return authModel;
        }
        var _currentUser = await _unitOfWork.Users.FindAsync(m => m.UserId == refreshToken.UserId);
        if (refreshToken.IsActive)
        {
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
        }
        else
        {
            authModel.Message = "Invalid token";
            return authModel;
            
        }

        var jwtToken = await CreateJwtToken(_currentUser);
        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        authModel.Username = _currentUser.UserName;

        authModel.RoleId = _currentUser.RoleId;


        return authModel;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var refreshToken = await _unitOfWork.RefreshTokens.FindAsync(m => m.Token == token);

        if (refreshToken == null)
        {
            return false;
        }


        refreshToken.RevokedOn = DateTime.UtcNow;
        _unitOfWork.RefreshTokens.Update(refreshToken);
        _unitOfWork.Save();
        return true;
    }

    public bool isUserExist(string userName)
    {
        return _unitOfWork.Users.IsExist(m => m.UserName == userName);
    }

    #region privateMethods

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }


    private async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("role", user.RoleId.ToString())
            };
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }


    #endregion



}