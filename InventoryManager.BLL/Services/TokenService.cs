using InventoryManager.BLL.DTO;
using InventoryManager.BLL.Interfaces;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.InnerResponse;
using InventoryManager.Domain.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManager.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _options;
        private readonly IService<Account> _accountService;
        private readonly IRepository<TokenData> _tokenDataRepository;

        public TokenService(IOptions<JWTSettings> options, IService<Account> accountService, IRepository<TokenData> tokenDataRepository)
        {
            _options = options.Value;
            _accountService = accountService;
            _tokenDataRepository = tokenDataRepository;
        }

        public string GetToken(Account client)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(CustomClaimType.AccountId, client.Id.ToString()),
                new Claim(CustomClaimType.AccountLogin, client.Login),
                new Claim(ClaimTypes.Role, ((int)client.Role).ToString()),
                new Claim(ClaimTypes.Email, client.Email),
                new Claim(CustomClaimType.Status, ((int)client.Status).ToString()),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(StandardConst.StartJWTTokenLifeTime)),
                    notBefore: DateTime.Now,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string Password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<StandardResponse<AuthDTO>> RefreshJWTToken(Guid accountId, string refreshTokenStr)
        {
            var refreshToken = await _tokenDataRepository.GetOneWhereAsync(x => x.AccountId == accountId && x.RefreshToken == refreshTokenStr);
            var clientResponse = await _accountService.GetEntityAsync(x => x.Id == accountId);
            if (clientResponse.InnerStatusCode == InnerStatusCode.EntityNotFound || refreshToken == null)
            {
                return new StandardResponse<AuthDTO>()
                {
                    Message = "token or account not found",
                    InnerStatusCode = InnerStatusCode.EntityNotFound
                };
            }

            var AuthDTO = (await UpdateRefreshToken(clientResponse.Data)).Data;

            return new StandardResponse<AuthDTO>()
            {
                Data = AuthDTO,
                InnerStatusCode = InnerStatusCode.AccountAuthenticate
            };
        }

        public async Task<StandardResponse<AuthDTO>> UpdateRefreshToken(Account account)
        {
            var newRefreshToken = GetRefreshToken();
            var newDataToken = new TokenData(newRefreshToken, (Guid)account.Id);

            var dataToken = await _tokenDataRepository.GetOneWhereAsync(x => x.AccountId == account.Id);
            if (dataToken == null)
            {
                await _tokenDataRepository.AddAsync(newDataToken);
            }
            else
            {
                dataToken.RefreshToken = newDataToken.RefreshToken;
                _tokenDataRepository.Update(dataToken);
            }
            await _tokenDataRepository.SaveAsync();

            return new StandardResponse<AuthDTO>()
            {
                Data = new(GetToken(account), (Guid)account.Id, newRefreshToken),
                InnerStatusCode = InnerStatusCode.EntityUpdate
            };
        }

        public string GetRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return refreshToken;
        }
    }
}