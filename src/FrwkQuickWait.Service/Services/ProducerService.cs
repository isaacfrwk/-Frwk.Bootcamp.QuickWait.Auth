using Confluent.Kafka;
using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Domain.Entities;
using FrwkQuickWait.Domain.Interfaces.Services;
using Newtonsoft.Json;

namespace FrwkQuickWait.Service.Services
{
    public class ProducerService : IProduceService
    {
        private readonly ClientConfig cloudConfig;
        private readonly string topicName;
        public ProducerService() 
        {
            this.topicName = Topics.topicNameAuthResponse;

            cloudConfig = new ClientConfig
            {
                BootstrapServers = Settings.Kafkahost
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
