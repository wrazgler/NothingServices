using Microsoft.Extensions.Configuration;
using NothingKafka.Services;

namespace NothingKafka.Configs;

/// <summary>
/// Конфигурация сервиса <see cref="SubscriberService"/>
/// </summary>
public sealed class SubscriberServiceConfig
{
    /// <summary>
    /// Задержка между итерациями работы сервиса в секундах
    /// </summary>
    [ConfigurationKeyName("SUBSCRIBE_ITERATION_DELAY")]
    public required int IterationDelay { get; init; } = 30;
}