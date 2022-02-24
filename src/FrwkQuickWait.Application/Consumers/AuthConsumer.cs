using Confluent.Kafka;
using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace FrwkQuickWait.Service.Consumers
{
    public class AuthConsumer : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly ConsumerConfig consumerConfig;
        private readonly IConfiguration _configuration;
        public AuthConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.topicName = Topics.topicNameAuth;
            this.serviceProvider = serviceProvider;
            _configuration = configuration;

            this.consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _configuration.GetSection("Kafka")["host"],
                GroupId = $"{topicName}-group-1",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = Task.Run(() => ProcessQueue(stoppingToken), stoppingToken);

            return task;
        }

        private void ProcessQueue(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(topicName);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        Task.Run(async () => { await InvokeService(consumeResult); }, stoppingToken);
                    }
                    catch (ConsumeException ex)
                    { }
                }
            }
            catch (OperationCanceledException ex)
            {
                consumer.Close();
            }
        }

        private async Task InvokeService(ConsumeResult<Ignore, string> message)
        {
            var mensagem = JsonConvert.DeserializeObject<MessageInput>(message.Message.Value);

            using var scope = serviceProvider.CreateScope();
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

            switch (mensagem.Method)
            {
                case MethodConstant.POST:
                    await tokenService.GenerateToken(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
            }
        }
    }
}
