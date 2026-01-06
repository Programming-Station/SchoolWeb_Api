using School.Domain.Auth;
using School_DTOs;
using School_DTOs.Account;
using School.Models.Account;
using School.Models.Configuration;
using School.Infrastructure;
using School.Infrastructure.JWTAuthenticationManager.Interfaces;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Infrastructure.JWTAuthenticationManager
{
    public class JWTAuthenticationManager : Repository<RefreshToken>, IJWTAuthenticationManager
    {
        private readonly AppSettings _appSetting;
        private readonly IUnitOfWork _unitOfWork;
        public JWTAuthenticationManager(IOptions<AppSettings> appSetting, DbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _appSetting = appSetting.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticationDto> Authenticate(string userId, Claim[] claims, string accessChannel = "", string accessPlatform = "")
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _appSetting.Issuer,
                audience: _appSetting.Audience,
                  claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_appSetting.ExpireTime),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshTokenValue = GenerateToken();

            RefreshToken obj = new RefreshToken();
            var data = await List(expression: x => Equals(x.UserId, userId)).ToListAsync();

            if (data.Any())
            {
                if (accessChannel != null && accessPlatform != null && data.Where(x => !string.IsNullOrEmpty(x.AccessChannel) && !string.IsNullOrEmpty(x.AccessPlatform)
                     && Equals(x.AccessChannel, accessChannel) && Equals(x.AccessPlatform, accessPlatform)).Any())
                {
                    obj = data.Where(x => Equals(x.AccessChannel, accessChannel) && Equals(x.AccessPlatform, accessPlatform)).First();
                }
                else
                {
                    obj = data.First();
                }
            }

            if (data.Count() != 0 && obj != null)
            {
                obj.ReplacedByToken = refreshTokenValue;
                obj.Token = token;
                obj.ReplacedByToken = refreshTokenValue;
                obj.Revoked = DateTime.Now;
                //obj.IsRevoked = false;
                obj.Expires = DateTime.Now.AddMinutes(_appSetting.ExpireTime);
                Update(obj);

            }
            else
            {
                RefreshToken refreshToken = new RefreshToken
                {
                    UserId = userId,
                    ReplacedByToken = refreshTokenValue,
                    Token = token,
                    CreatedByIp = userId,
                    Created = DateTime.Now,
                    Expires = DateTime.Now.AddMinutes(_appSetting.ExpireTime + 1)
                };
                Add(refreshToken);
            }
            await _unitOfWork.CommitAsync();

            return new AuthenticationDto
            {
                AccessToken = token,
                RefreshToken = refreshTokenValue,
                ExpireTime = DateTime.Now.AddMinutes(_appSetting.ExpireTime)
            };
        }
        public async Task<APIResponse<AuthenticationDto>> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
        {
            APIResponse<AuthenticationDto> aPIResponse = new APIResponse<AuthenticationDto>();

            try
            {
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSetting.SecretKey));
                var tokenHandler = new JwtSecurityTokenHandler();
                var pricipal = tokenHandler.ValidateToken(refreshTokenModel.AccessToken,
                   new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = _appSetting.Issuer,

                       ValidateAudience = true,
                       ValidAudience = _appSetting.Audience,

                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = key,

                       ValidateLifetime = false
                   }, out SecurityToken validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtToken))
                {
                    throw new SecurityTokenException("Invalid token passed!");
                }
                var obj = await FindAsync(expression: x => x.Token == refreshTokenModel.AccessToken
                                                   && x.ReplacedByToken == refreshTokenModel.RefreshToken);

                if (obj != null)
                {
                    AuthenticationDto authenticationResponse = await Authenticate(refreshTokenModel.UserId, pricipal.Claims.ToArray());
                    aPIResponse.Message = "Token Refreshed Successfully.";
                    aPIResponse.Success = true;
                    aPIResponse.Data = authenticationResponse;
                    aPIResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    aPIResponse.Message = "Invalid token passed!";
                    aPIResponse.Success = true;
                    aPIResponse.StatusCode = HttpStatusCode.Continue;
                }

                return aPIResponse;
            }
            catch (Exception ex)
            {

                var ApiCommonDto = new APIResponse<AuthenticationDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = ex.Message
                };
                return ApiCommonDto;
            }
        }

        private string GenerateToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
