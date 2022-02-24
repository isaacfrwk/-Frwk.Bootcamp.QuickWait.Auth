using Confluent.Kafka;
using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FrwkQuickWait.Service.Services
{
    public class ProducerService : IProduceService
    {
        private readonly ClientConfig cloudConfig;
        private readonly string topicName;
        private readonly IConfiguration _configuration;
        public ProducerService(IConfiguration configuration) 
        {
            this.topicName = Topics.topicNameAuthResponse;
            _configuration = configuration;

            cloudConfig = new ClientConfig
            {
                BootstrapServers = _configuration.GetSection("Kafka")["host"]
            };
        }

        public async Task Call(MessageInput message)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);

            using var producer = new ProducerBuilder<string, string>(cloudConfig).Build();

            var key = new Guid().ToString();

            await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = stringfiedMessage });

            producer.Flush(TimeSpan.FromSeconds(2));
        }
    }
}
