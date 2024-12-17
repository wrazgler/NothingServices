using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace NothingServices.WPFApp.Configs;

/// <summary>
/// Конфигурация подключения к сервису NothingRpcApi
/// </summary>
public sealed class NothingRpcApiClientConfig
{
    /// <summary>
    /// Адрес сервиса NothingRpcApi
    /// </summary>
    [ConfigurationKeyName("NOTHING_GRPC_API_URL")]
    public required string BaseUrl { get; init; }

    /// <summary>
    /// Канал соединения с сервисом NothingRpcApi
    /// </summary>
    public GrpcChannel GrpcChannel => GrpcChannel.ForAddress(BaseUrl);
}