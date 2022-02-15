using Confluent.Kafka;
using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Domain.Interfaces.Services;
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
        private readonly string topicName;
        private readonly ClientConfig cloudConfig;
        public TokenService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.topicName = Topics.topicNameAuthResponse;

            cloudConfig = new ClientConfig
            {

                 BootstrapServers = Settings.Kafkahost
                //BootstrapServers = CloudKarafka.Brokers,
                //SaslUsername = CloudKarafka.Username,
                //SaslPassword = CloudKarafka.Password,
                //SaslMechanism = SaslMechanism.ScramSha256,
                //SecurityProtocol = SecurityProtocol.SaslSsl,
                //EnableSslCertificateVerification = false
            };
        }
        public async Task<string> GenerateToken(User user)
        {
            string request;
            User? response;

            if (!string.IsNullOrEmpty(user.CNPJ))
                response = await GetByCnpj(user.Password, user.CNPJ);
            else
                response = await GetByUserName(user.Password, user.Username);

            MessageInput? message;

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

            await Call(message);

            return await Task.FromResult(request);
        }

        private async Task<User> GetByCnpj(string password, string cnpj)
             => await userRepository.GetByCnpj(password, cnpj);

        private async Task<User> GetByUserName(string password, string username)
            => await userRepository.Get(username, password);


        protected async Task Call(MessageInput message)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);

            using var producer = new ProducerBuilder<string, string>(cloudConfig).Build();

            var key = new Guid().ToString();

            await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = stringfiedMessage });

            producer.Flush(TimeSpan.FromSeconds(2));
        }

    }
}
