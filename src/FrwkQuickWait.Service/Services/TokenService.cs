using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Domain.Interfaces.Services;
using FrwkQuickWait.Service.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrwkQuickWait.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository userRepository;
        private readonly IProduceService produceService;
        
        public TokenService(IUserRepository userRepository, IProduceService produceService)
        {
            this.userRepository = userRepository;
            this.produceService = produceService;
        }

        public async Task<string> GenerateToken(User model)
        {
            string request;
            MessageInput? message;

            var response = await GetByUserName(UserHelper.GenerateMD5(model.Password), model.Username);

            if (response is null)
            {
                request = "Usuário ou senha estão incorretos";
                message = new MessageInput(404, MethodConstant.POST, request);
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Settings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, response.Username),
                    new Claim(ClaimTypes.Role, response.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                request = tokenHandler.WriteToken(token);
                message = new MessageInput(200, MethodConstant.POST, request);
            }

            await produceService.Call(message);

            return await Task.FromResult(request);
        }

        private async Task<User> GetByUserName(string password, string username)
            => await userRepository.Get(username, password);

    }
}
