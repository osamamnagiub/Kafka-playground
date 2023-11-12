using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Producer.WikimediaStream
{
    public class KafkaProducer : IDisposable
    {
        public const string TOPIC = "wikimedia_recentchanges";
        private readonly ProducerConfig _config;
        private IProducer<string, string> _producer;
        private bool _disposed = false;

        public KafkaProducer(string bootstrapServers)
        {
            this._config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,

                // set safe producer, this will set the remaining required props
                EnableIdempotence = true,
                Acks = Acks.All,
                MessageSendMaxRetries = int.MaxValue,
                StickyPartitioningLingerMs = 20,
                BatchSize = 32 * 1024,
                CompressionType = CompressionType.Snappy
            };


            _producer = new ProducerBuilder<string, string>(_config).Build();
        }


        public void SendAsync(Message<string, string> message)
        {
            _producer.Produce(TOPIC, message);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Suppress finalization.
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any managed objects here.
                _producer?.Dispose();
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }


        // Destructor
        ~KafkaProducer()
        {
            Dispose(false);
        }
    }
}
