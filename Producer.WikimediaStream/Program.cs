using LaunchDarkly.EventSource;
using System.Diagnostics.Tracing;

namespace Producer.WikimediaStream
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var url = new Uri("https://stream.wikimedia.org/v2/stream/recentchange");

            var configuration = Configuration.Builder(url).Build();
            var kafkaProducer = new KafkaProducer("localhost:9092");


            using (var eventSource = new LaunchDarkly.EventSource.EventSource(configuration))
            {
                eventSource.Opened += (sender, e) =>
                {
                    Console.WriteLine("Opened connection.");
                };

                eventSource.Error += (sender, e) =>
                {
                    Console.WriteLine($"Encountered error: {e.Exception?.Message ?? "Unknown error"}");
                    kafkaProducer.Dispose();
                };

                eventSource.Closed += (sender, e) =>
                {
                    Console.WriteLine("Closed connection.");
                    kafkaProducer.Dispose();
                };

                eventSource.MessageReceived += (sender, e) =>
                {
                    kafkaProducer.SendAsync(new Confluent.Kafka.Message<string, string>
                    {
                        Value = e.Message.Data
                    });
                };

                await eventSource.StartAsync();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}