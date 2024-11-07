# NothingServices.Abstractions
## [OpenApiVersionAttribute](Attributes/OpenApiVersionAttribute.cs)
### `dotnet-tools.json`
```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "swashbuckle.aspnetcore.cli": {
      "version": "6.9.0",
      "commands": [
        "swagger"
      ]
    }
  }
}
```
### `.csproj`
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

    <PropertyGroup>
        <OpenApiName>openapi</OpenApiName>
        <OpenApiVersion>1.0</OpenApiVersion>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="NothingServices.Abstractions" Version="1.0.0.0" />
        <PackageReference Include="Swashbuckle.AspNetCore" Version="6.9.0" />
    </ItemGroup>


    <ItemGroup>
        <AssemblyAttribute Include="NothingServices.Abstractions.Attributes.OpenApiVersionAttribute">
            <_Parameter1>$(OpenApiVersion)</_Parameter1>
        </AssemblyAttribute>
    </ItemGroup>

    <Target Name="Generate OpenAPI" AfterTargets="Build" Condition="$(Configuration)=='Debug'">
        <Exec Command="dotnet tool restore" />
        <Exec Command="dotnet swagger tofile --output $(OpenApiName)_v$(OpenApiVersion).yaml --yaml $(OutputPath)$(AssemblyName).dll v$(OpenApiVersion)" WorkingDirectory="$(ProjectDir)" />
    </Target>

</Project>
```
## [PostgresConfig](Configs/PostgresConfig.cs)
### `appsettings.json`
```json
{
    "POSTGRES_HOST": "localhost",
    "POSTGRES_PORT": "5432",
    "POSTGRES_DB": "nothing_db",
    "POSTGRES_USER": "nothing",
    "POSTGRES_PASSWORD": "nothing"
}
```
### GetConfig
```csharp
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .Build()
    .GetConfig<PostgresConfig>();
```
### IServiceCollection
```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var config = new ServiceCollection()
    .Configure<PostgresConfig>(configuration)
    .BuildServiceProvider()
    .GetRequiredService<IOptions<PostgresConfig>>()
    .Value;
```
### IHostBuilder
```csharp
var config = Host.CreateDefaultBuilder(args)
    .Build()
    .Services
    .GetRequiredService<IOptions<PostgresConfig>>()
    .Value;
```
## [ConfigurationExtensions](Extensions/ConfigurationExtensions.cs)
### Config
```csharp
private class Config
{
    [ConfigurationKeyName("CONFIG_NAME")]
    public required string Name { get; init; }
}
```
### GetConfig\<TConfig\>
```csharp
var dictionary = new Dictionary<string, string>(5)
{
    {"CONFIG_NAME", "Test"},
};
var config = new ConfigurationBuilder()
    .AddInMemoryCollection(dictionary!)
    .Build()
    .GetConfig<Config>();
```
## [EnumExtensions](Extensions/EnumExtensions.cs)
### Enum
```csharp
public enum Enum
{
    [Description("description")]
    [PgName("pgName")]
    Value,
}
```
### GetDescription
```csharp
var description = Enum.Value.GetDescription();
```
### GetPgName
```csharp
var pgName = Enum.Value.GetPgName();
```
## [TypeExtensions](Extensions/TypeExtensions.cs)
### Class
```csharp
[PgName("pg_name")]
[Table("tableName")]
public class Class
{
    [DefaultValue("defaultValue")]
    [Description("description")]
    [JsonPropertyName("jsonPropertyName")]
    public required string Property { get; init; }
}
```
### GetDefaultValue
```csharp
var defaultValue = typeof(Class).GetDefaultValue(nameof(Class.Property));
```
### GetDefaultValue\<T\>
```csharp
var defaultValue = typeof(Class).GetDefaultValue<string>(nameof(Class.Property));
```
### GetDescription
```csharp
var description = typeof(Class).GetDescription(nameof(Class.Property));
```
### GetJsonPropertyName
```csharp
var jsonPropertyName = typeof(Class).GetJsonPropertyName(nameof(Class.Property));
```
### GetTableName
```csharp
var tableName = typeof(Class).GetTableName();
```
### GetPgName
```csharp
var pgName = typeof(Class).GetPgName();
```