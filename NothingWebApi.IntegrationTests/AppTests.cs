using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using NothingWebApi.Dtos;

namespace NothingWebApi.IntegrationTests;

public class AppTests
{
    private const string AppUrl = "https://localhost:8259/nothing-web-api/NothingWebApi";

    [Fact]
    public async Task Get_Result_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var httpClient = GetHttpClient();

            //Act
            using var response = await httpClient.GetAsync(AppUrl);
            var result = await response.Content.ReadFromJsonAsync<NothingModelDto[]>();

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
            var httpClient = GetHttpClient();
            var id = 1;

            //Act
            using var response = await httpClient.GetAsync($"{AppUrl}/{id}");
            var result = await response.Content.ReadFromJsonAsync<NothingModelDto>();

            //Assert
            var expected = new NothingModelDto()
            {
                Id = id,
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
            var httpClient = GetHttpClient();
            var createNothingModelDto = new CreateNothingModelDto()
            {
                Name = "New model",
            };
            var content = JsonContent.Create(createNothingModelDto);

            //Act
            using var response = await httpClient.PostAsync(AppUrl, content);
            var result = await response.Content.ReadFromJsonAsync<NothingModelDto>();

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
            var httpClient = GetHttpClient();
            var updateNothingModelDto = new UpdateNothingModelDto()
            {
                Id = 1,
                Name = "New name",
            };
            var content = JsonContent.Create(updateNothingModelDto);

            //Act
            using var response = await httpClient.PutAsync(AppUrl, content);
            var result = await response.Content.ReadFromJsonAsync<NothingModelDto>();

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
            var httpClient = GetHttpClient();
            var id = 1;

            //Act
            using var response = await httpClient.DeleteAsync($"{AppUrl}/{id}");
            var result = await response.Content.ReadFromJsonAsync<NothingModelDto>();

            //Assert
            var expected = new NothingModelDto()
            {
                Id = id,
                Name = "Test",
            };
            Assert.Equivalent(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    private static HttpClient GetHttpClient()
    {
        var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), "localhost.crt");
        if(!File.Exists(certificatePath))
            throw new FileNotFoundException($"Could not find {certificatePath}");
        var certificate = new X509Certificate2(certificatePath, "localhost");
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);
        var httpClient = new HttpClient(handler);
        return httpClient;
    }

    private static async Task StartApp(int delay = 10000)
    {
        await Process.Start("docker", "compose up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 10000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f -v test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f test_nothing_web_api_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}