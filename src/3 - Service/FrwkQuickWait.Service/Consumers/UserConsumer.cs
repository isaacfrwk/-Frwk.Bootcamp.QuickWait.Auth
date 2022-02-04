using Confluent.Kafka;
using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace FrwkQuickWait.Service.Consumers
{
    public class UserConsumer : BackgroundService
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly ConsumerConfig consumerConfig;
        public UserConsumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.topicName = Settings.topicName;

            this.consumerConfig = new ConsumerConfig
            {
                BootstrapServers = Settings.Kafka,
                GroupId = $"{topicName}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _cancellationTokenSource = new CancellationTokenSource();
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
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();

            switch (mensagem.Method)
            {
                case MethodConstant.POST:
                    await service.Save(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
                case MethodConstant.PUT:
                    service.Update(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
                case MethodConstant.DELETE:
                    service.Delete(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
                case MethodConstant.DELETEMANY:
                    service.DeleteMany(JsonConvert.DeserializeObject<IEnumerable<User>>(mensagem.Content));
                    break;
            }
        }


    }
}
