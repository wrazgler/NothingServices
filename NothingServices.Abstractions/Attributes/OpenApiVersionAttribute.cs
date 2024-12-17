namespace NothingServices.Abstractions.Attributes;

/// <summary>
/// Аттрибут версии документа OpenApi
/// </summary>
/// <param name="version">Версия документа OpenApi</param>
[AttributeUsage(AttributeTargets.Assembly)]

public sealed class OpenApiVersionAttribute(string version) : Attribute
{
    /// <summary>
    /// Версия документа OpenApi
    /// </summary>
    public Version Version { get; } = new(version);
}