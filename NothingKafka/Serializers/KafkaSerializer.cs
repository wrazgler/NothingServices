using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace NothingKafka.Serializers;

/// <summary>
/// Сериализатор сообщений в Kafka
/// </summary>
public class KafkaSerializer<TData> : ISerializer<TData>, IDeserializer<TData?>
    where TData : class
{
    private readonly JsonSerializerOptions _options = new()
    {
        IncludeFields = true,
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
        },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
    };

    /// <summary>
    /// Сериализовать сообщений в Kafka
    /// </summary>
    /// <param name="data">Сообщение в Kafka</param>
    /// <param name="context">Контекст сериализации Kafka</param>
    /// <returns>Массив байт в Kafka</returns>
    public byte[] Serialize(TData? data, SerializationContext context)
    {
        if (data is null)
            return [];

        var stringData = JsonSerializer.Serialize(data, _options);
        return Encoding.UTF8.GetBytes(stringData);
    }

    /// <summary>
    /// Десериализовать сообщений из Kafka
    /// </summary>
    /// <param name="data">Массив байт из Kafka</param>
    /// <param name="isNull">Флаг отсутствия сообщения</param>
    /// <param name="context">Контекст сериализации Kafka</param>
    /// <returns>Сообщение из Kafka</returns>
    public TData? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            return null;

        var stringData = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<TData>(stringData, _options);
    }
}