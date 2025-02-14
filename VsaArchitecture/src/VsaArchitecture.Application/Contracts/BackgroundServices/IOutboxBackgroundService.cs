namespace VsaArchitecture.Application.Contracts.BackgroundServices;

public interface IOutboxBackgroundService
{
    Task Run();
}