using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;

public class KafkaConsumerHelper : IDisposable
{
    private readonly string topic;
    private readonly ConsumerConfig config;
    private readonly OpenSearchHelper openSearchHelper;
    private IConsumer<Ignore, string> consumer;
    private bool disposed = false;

    public KafkaConsumerHelper(string topic, ConsumerConfig config, OpenSearchHelper openSearchHelper)
    {
        this.topic = topic;
        this.config = config;
        this.openSearchHelper = openSearchHelper;
        consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    public void Consume(CancellationTokenSource cts)
    {
        consumer.Subscribe(topic);

        const int batchSize = 1000;
        const int timeout = 1000;
        var batch = new List<ConsumeResult<Ignore, string>>();

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(timeout);
                if (consumeResult != null)
                {
                    batch.Add(consumeResult);

                    if (batch.Count >= batchSize)
                    {
                        openSearchHelper.ProcessBatch(batch);
                        batch.Clear();
                    }
                }
                else if (batch.Count > 0)
                {
                    openSearchHelper.ProcessBatch(batch);
                    batch.Clear();
                }
            }
        }
        catch (ConsumeException e)
        {
            Console.WriteLine($"Error occurred: {e.Error.Reason}");
        }
        finally
        {
            consumer.Close();
            Console.WriteLine($"Consumer closed");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
                consumer?.Dispose();
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~KafkaConsumerHelper()
    {
        Dispose(false);
    }
}
