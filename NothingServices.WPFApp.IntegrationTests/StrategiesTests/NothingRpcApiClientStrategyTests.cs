using System.Diagnostics;

namespace NothingServices.WPFApp.IntegrationTests.StrategiesTests;

public class NothingRpcApiClientStrategyTests
{
    private static async Task StartApp(int delay = 10000)
    {
        var projectPath = Path.GetFullPath("../../../");
        var dockerFilePath = Path.Combine(projectPath, "docker-compose.nothing-grpc-api-client-strategy-test.yml");
        await Process.Start("docker", $"compose -f {dockerFilePath} up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 10000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v wpf_nothing_grpc_api_client_strategy_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f wpf_nothing_grpc_api_client_strategy_test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f wpf_nothing_grpc_api_client_strategy_test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f wpf_nothing_grpc_api_client_strategy_test_nothing_services_wpf_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}