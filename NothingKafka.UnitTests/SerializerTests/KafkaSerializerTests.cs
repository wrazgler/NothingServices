using Confluent.Kafka;
using NothingKafka.Serializers;

namespace NothingKafka.UnitTests.SerializerTests;

public class KafkaSerializerTests
{
    [Fact]
    public void Serialize_Success()
    {
        //Arrange
        var kafkaSerializer = new KafkaSerializer<Test>();
        var data = new Test
        {
            Name = "test",
        };

        //Act
        var result =kafkaSerializer.Serialize(data, SerializationContext.Empty);

        //Assert
        var expected = "{\n  \"name\": \"test\"\n}"u8.ToArray();
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void Deserialize_Success()
    {
        //Arrange
        var kafkaSerializer = new KafkaSerializer<Test>();
        var data = "{\n  \"name\": \"test\"\n}"u8.ToArray();

        //Act
        var result =kafkaSerializer.Deserialize(data, false, SerializationContext.Empty);

        //Assert
        var expected = new Test
        {
            Name = "test",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void Deserialize_IsNull_Success()
    {
        //Arrange
        var kafkaSerializer = new KafkaSerializer<Test>();
        var data = "{\n  \"name\": \"test\"\n}"u8.ToArray();

        //Act
        var result =kafkaSerializer.Deserialize(data, true, SerializationContext.Empty);

        //Assert
        Assert.Null(result);
    }

    private class Test
    {
        public required string Name { get; set; }
    }
}