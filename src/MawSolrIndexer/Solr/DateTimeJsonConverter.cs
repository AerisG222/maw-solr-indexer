using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MawSolrIndexer.Solr;

public class DateTimeJsonConverter
    : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => DateTime
            .ParseExact(reader.GetString()!, "o", CultureInfo.InvariantCulture);

    public override void Write(
        Utf8JsonWriter writer,
        DateTime dateTimeValue,
        JsonSerializerOptions options) => writer
            .WriteStringValue(dateTimeValue
                .ToUniversalTime()
                .ToString("o", CultureInfo.InvariantCulture));
}
