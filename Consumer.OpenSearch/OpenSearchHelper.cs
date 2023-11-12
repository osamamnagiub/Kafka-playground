using Confluent.Kafka;
using OpenSearch.Client;
using System;

public class OpenSearchHelper : IDisposable
{
    private readonly OpenSearchClient client;
    private readonly string indexName;
    private bool disposed = false;
    public OpenSearchHelper(string indexName, Uri openSearchUri)
    {
        this.indexName = indexName;
        var settings = new ConnectionSettings(openSearchUri)
            .DefaultIndex(indexName)
            .DisableDirectStreaming(false);

        client = new OpenSearchClient(settings);
        CreateIndexIfNeeded();
    }

    private void CreateIndexIfNeeded()
    {
        var indexExists = client.Indices.Exists(indexName).Exists;

        if (!indexExists)
        {
            var createIndexResponse = client.Indices.Create(indexName, c => c.Map(m => m.AutoMap()));
            if (!createIndexResponse.IsValid)
            {
                throw new Exception($"Failed to create index: {createIndexResponse.DebugInformation}");
            }
        }
    }

    public void ProcessBatch(List<ConsumeResult<Ignore, string>> batch)
    {
        var bulkDescriptor = new BulkDescriptor();

        foreach (var result in batch)
        {
            var wikimediaRecentChange = WikimediaRecentChange.Deserialize(result.Value);
            bulkDescriptor.Index<WikimediaRecentChange>(i => i
                .Document(wikimediaRecentChange)
                .Id(wikimediaRecentChange.Meta.Id)
                .Index(indexName));
        }

        var bulkResponse = client.Bulk(bulkDescriptor);

        if (bulkResponse.IsValid)
        {
            Console.WriteLine($"Handled batch size: {batch.Count}");
        }
        else
        {
            Console.WriteLine($"Failed to index batch: {bulkResponse.DebugInformation}");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
                client?.ConnectionSettings.Dispose();
                Console.WriteLine($"Connection to OpenSearch Closed");
            }

            // Note: No unmanaged resources in this class, so no need to free them

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~OpenSearchHelper()
    {
        Dispose(false);
    }
}
