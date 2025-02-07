using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace NothingKafka.Serializers;

public class KafkaSerializer<TMessage> : ISerializer<TMessage>, IDeserializer<TMessage?>
    where TMessage : class
{
    private readonly JsonSerializerOptions _options = new()
    {
    };

    public byte[] Serialize(TMessage? data, SerializationContext context)
    {
        if (data is null)
            return Array.Empty<byte>();

        var stringData = JsonSerializer.Serialize(data, _options);
        return Encoding.UTF8.GetBytes(stringData);
    }

    public TMessage? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null;
        }

        var stringData = Encoding.UTF8.GetString(data);

        return JsonSerializer.Deserialize<TMessage>(stringData, _options);
    }
}