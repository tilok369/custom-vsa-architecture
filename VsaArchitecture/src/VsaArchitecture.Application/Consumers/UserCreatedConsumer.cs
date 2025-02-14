using MassTransit;
using VsaArchitecture.Application.Consumers.Models;

namespace VsaArchitecture.Application.Consumers;

public class UserCreatedConsumer: IConsumer<UserCreated>
{
    public Task Consume(ConsumeContext<UserCreated> context)
    {
        Console.WriteLine($"UserCreatedConsumer: {context.Message.Id}");
        return Task.CompletedTask;
    }
}