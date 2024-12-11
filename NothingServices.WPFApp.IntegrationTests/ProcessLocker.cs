namespace NothingServices.WPFApp.IntegrationTests;

public static class ProcessLocker
{
    public static Mutex Mutex { get; } = new();
}