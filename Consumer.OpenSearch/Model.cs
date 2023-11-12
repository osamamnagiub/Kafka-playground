using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class WikimediaRecentChange
{
    [JsonPropertyName("$schema")]
    public string Schema { get; set; }

    [JsonPropertyName("meta")]
    public MetaData Meta { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("namespace")]
    public int Namespace { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("title_url")]
    public string TitleUrl { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("user")]
    public string User { get; set; }

    [JsonPropertyName("bot")]
    public bool Bot { get; set; }

    [JsonPropertyName("notify_url")]
    public string NotifyUrl { get; set; }

    [JsonPropertyName("minor")]
    public bool Minor { get; set; }

    [JsonPropertyName("patrolled")]
    public bool Patrolled { get; set; }

    [JsonPropertyName("length")]
    public ChangeLength Length { get; set; }

    [JsonPropertyName("revision")]
    public Revision Revision { get; set; }

    [JsonPropertyName("server_url")]
    public string ServerUrl { get; set; }

    [JsonPropertyName("server_name")]
    public string ServerName { get; set; }

    [JsonPropertyName("server_script_path")]
    public string ServerScriptPath { get; set; }

    [JsonPropertyName("wiki")]
    public string Wiki { get; set; }

    [JsonPropertyName("parsedcomment")]
    public string ParsedComment { get; set; }


    public static string Serialize(WikimediaRecentChange change)
    {
        return JsonSerializer.Serialize(change);
    }

    public static WikimediaRecentChange? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<WikimediaRecentChange>(json);
    }
}

public class MetaData
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("request_id")]
    public string RequestId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("dt")]
    public string Dt { get; set; }

    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    [JsonPropertyName("stream")]
    public string Stream { get; set; }

    [JsonPropertyName("topic")]
    public string Topic { get; set; }

    [JsonPropertyName("partition")]
    public int Partition { get; set; }

    [JsonPropertyName("offset")]
    public long Offset { get; set; }
}

public class ChangeLength
{
    [JsonPropertyName("old")]
    public int Old { get; set; }

    [JsonPropertyName("new")]
    public int New { get; set; }
}

public class Revision
{
    [JsonPropertyName("old")]
    public long Old { get; set; }

    [JsonPropertyName("new")]
    public long New { get; set; }
}
