using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MawSolrIndexer.Solr;

public class SolrUploader
{
    private static readonly HttpClient _client = new();

    readonly string _solrUpdateUrl;

    public SolrUploader(string solrUpdateUrl)
    {
        if(string.IsNullOrWhiteSpace(solrUpdateUrl))
        {
            throw new ArgumentNullException(nameof(solrUpdateUrl));
        }

        _solrUpdateUrl= solrUpdateUrl;
    }

    public async Task ClearIndex()
    {
        var json = "{ \"delete\": { \"query\": \"*:*\" }}";

        await PostContentAsync(json);
    }

    public async Task UploadFullIndex(IEnumerable<MultimediaCategory> categories)
    {
        var opts = new JsonSerializerOptions
        {
            Converters = {
                new DateTimeJsonConverter()
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(categories, opts);

        await PostContentAsync(json);
    }

    async Task PostContentAsync(string json)
    {
        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _client.PostAsync(_solrUpdateUrl, content);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();

            throw new ApplicationException($"Unable to clear index: {message}");
        }
    }
}
