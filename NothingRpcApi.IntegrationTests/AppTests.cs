using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using NothingRpcApi.IntegrationTests.Clients;
using NothingRpcApi.IntegrationTests.Dtos;

namespace NothingRpcApi.IntegrationTests;

public class AppTests
{
    private const string AppUrl = "https://localhost:8459/nothing-grpc-api";

    [Fact]
    public async Task Get_Result_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var client = GetNothingRpcServiceClient();

            //Act
            var responseStream = client.GetStream(new Empty()).ResponseStream;
            var result = new List<NothingModelDto>(1);
            while (await responseStream.MoveNext(CancellationToken.None))
            {
                result.Add(responseStream.Current);
            }

            //Assert
            var expected = new NothingModelDto[]
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                }
            };
            Assert.Equivalent(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Get_Id_Result_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var client = GetNothingRpcServiceClient();
            var request = new NothingModelIdDto()
            {
                Id = 1
            };

            //Act
            var result = await client.GetAsync(request);

            //Assert
            var expected = new NothingModelDto()
            {
                Id = request.Id,
                Name = "Test",
            };
            Assert.Equivalent(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Create_Result_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var client = GetNothingRpcServiceClient();
            var createNothingModelDto = new CreateNothingModelDto()
            {
                Name = "New model",
            };

            //Act
            var result = await client.CreateAsync(createNothingModelDto);

            //Assert
            var expected = new NothingModelDto()
            {
                Id = 2,
                Name = createNothingModelDto.Name,
            };
            Assert.Equivalent(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Update_Result_Equal()
	{
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var client = GetNothingRpcServiceClient();
            var updateNothingModelDto = new UpdateNothingModelDto()
            {
                Id = 1,
                Name = "New name",
            };

            //Act
            var result = await client.UpdateAsync(updateNothingModelDto);

            //Assert
            var expected = new NothingModelDto()
            {
                Id = updateNothingModelDto.Id,
                Name = updateNothingModelDto.Name,
            };
            Assert.Equivalent(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Delete_Result_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var client = GetNothingRpcServiceClient();
            var request = new NothingModelIdDto()
            {
                Id = 1
            };

            //Act
            var result = await client.DeleteAsync(request);

            //Assert
            var expected = new NothingModelDto()
            {
                Id = request.Id,
                Name = "Test",
            };
            Assert.Equivalent(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    private static NothingRpcService.NothingRpcServiceClient GetNothingRpcServiceClient()
    {
        var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), "localhost.crt");
        if(!File.Exists(certificatePath))
            throw new FileNotFoundException($"Could not find {certificatePath}");
        var certificate = new X509Certificate2(certificatePath, "localhost");
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);
        var httpClient = new HttpClient(handler);
        var grpcChannelOptions = new GrpcChannelOptions
        {
            HttpClient = httpClient
        };
        var grpcChannel = GrpcChannel.ForAddress(AppUrl, grpcChannelOptions);
        var client = new NothingRpcService.NothingRpcServiceClient(grpcChannel);
        return client;
    }

    private static async Task StartApp(int delay = 10000)
    {
        await Process.Start("docker", "compose up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 15000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f -v test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f test_nothing_grpc_api_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}