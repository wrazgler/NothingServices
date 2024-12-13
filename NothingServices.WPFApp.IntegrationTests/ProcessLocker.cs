namespace NothingServices.WPFApp.IntegrationTests;

public static class ProcessLocker
{
    public static Semaphore Semaphore { get; } = new(1, 1);
}