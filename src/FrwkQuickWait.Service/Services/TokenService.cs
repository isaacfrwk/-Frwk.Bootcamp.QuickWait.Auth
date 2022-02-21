using Confluent.Kafka;
using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Domain.Interfaces.Services;
using FrwkQuickWait.Service.Validators;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        public async Task<string> GenerateToken(User user)
        {
            var userValidator = new UserValidator();
            var validator = userValidator.Validate(user);

            MessageInput message;
            string request;

            if (!validator.IsValid)
                message = new MessageInput(500, MethodConstant.POST, JsonConvert.SerializeObject(validator.Errors));

            var response = await GetByUserName(user.Password, user.Username);

            if (response == null)
            {
                request = "Usuário ou senha estão incorretos";
                message = new MessageInput(404, MethodConstant.POST, JsonConvert.SerializeObject(request));
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
                message = new MessageInput(200, MethodConstant.POST, JsonConvert.SerializeObject(request));
            }

            await produceService.Call(message);

            return await Task.FromResult(request);
        }

        private async Task<User> GetByCnpj(string password, string cnpj)
             => await userRepository.GetByCnpj(password, cnpj);

        private async Task<User> GetByUserName(string password, string username)
            => await userRepository.Get(username, password);

    }
}
