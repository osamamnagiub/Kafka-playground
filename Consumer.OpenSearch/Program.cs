using Confluent.Kafka;
using System;
using System.Threading;

namespace Consumer.OpenSearch
{
    internal class Program
    {
        private static string indexName = "wikimedia";
        private static Uri openSearchUri = new Uri("http://localhost:9200");
        private static string topic = "wikimedia_recentchanges";

        static void Main(string[] args)
        {
           var consumerConfig = new ConsumerConfig
            {
                GroupId = "consumer-opensearch-demo",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            using (var openSearchHelper = new OpenSearchHelper(indexName, openSearchUri))
            using (var kafkaConsumerHelper = new KafkaConsumerHelper(topic, consumerConfig, openSearchHelper))
            {
                kafkaConsumerHelper.Consume(cts);
            }
        }
    }
}
