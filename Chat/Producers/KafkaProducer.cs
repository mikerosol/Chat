using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using Chat.Extensions;
using Chat.Messages;

namespace Chat.Producers
{
    public class KafkaProducer
    {
        private ProducerConfig _producerConfig { get; set; }
        private IProducer<string, string> _producer { get; set; }

        public KafkaProducer()
        {
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = "localhost:29092"
            };
            _producer = new ProducerBuilder<string, string>(_producerConfig).Build();
        }

        /// <summary>
        /// Producer will deliver message to consumers in order by time.
        /// Performance limited to a single partition.
        /// </summary>
        public async Task ProduceAsync(object message, string topic, int partition)
        {
            var result = await _producer.ProduceAsync(
                new TopicPartition(topic, partition),
                new Message<string, string> { Key = null, Value = message.ToJson() });

            Output(result);
        }

        /// <summary>
        /// Producer will deliver message to consumers in order by time.  
        /// Performance limited to a single partition.
        /// </summary>
        public async Task ProduceAsync(object message, string topic, string key)
        {
            var result = await _producer.ProduceAsync(
                topic,
                new Message<string, string> { Key = key, Value = message.ToJson() });

            Output(result);
        }

        /// <summary>
        /// Producer will deliver message to consumers in "random" order. 
        /// Allows better performance by using all partitions.
        /// </summary>
        public async Task ProduceAsync(KafkaMessage message)
        {
            var result = await _producer.ProduceAsync(
                message.MetaData.MessageType.ToString(),
                new Message<string, string> { Key = null, Value = message.ToJson() });

            Output(result);
        }

        ///// <summary>
        ///// Producer will deliver message to consumers in "random" order. 
        ///// Allows better performance by using all partitions.
        ///// </summary>
        public async Task ProduceAsync(object message, string topic)
        {
            var result = await _producer.ProduceAsync(
                topic,
                new Message<string, string> { Key = null, Value = message.ToJson() });

            Output(result);
        }


        private void Output(DeliveryResult<string, string> result)
        {
            Console.WriteLine($"Message sent on partition: {result.Partition} with offset: {result.Offset} at {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
        }
    }
}