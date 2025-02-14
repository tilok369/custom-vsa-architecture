using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using VsaArchitecture.Application.Consumers.Models;
using VsaArchitecture.Application.Contracts.BackgroundServices;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Application.BackgroundServices;

public class OutboxBackgroundService(
    IUnitOfWork unitOfWork,
    IOutboxMessageRepository outboxMessageRepository,
    ILogger<OutboxBackgroundService> logger,
    IPublishEndpoint publishEndpoint,
    IConfiguration configuration)
    : IOutboxBackgroundService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOutboxMessageRepository _outboxMessageRepository = outboxMessageRepository;
    private readonly ILogger<OutboxBackgroundService> _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IConfiguration _configuration = configuration;

    public async Task Run()
    {
        var unProcessedMessages = (await _outboxMessageRepository
            .FindAsync(o => o.ProcessedOn == null))
            .OrderBy(o=>o.ProcessedOn)
            .Take(int.Parse(_configuration["Outbox:BatchSize"]??"0"))
            .ToList();
        foreach (var outboxMessage in unProcessedMessages)
        {
            var user = JsonSerializer.Deserialize<User>(outboxMessage.Message);
            if(user == null) continue;
            await _publishEndpoint.Publish(new UserCreated(user.Id, user));
            outboxMessage.ProcessedOn = DateTime.UtcNow;
            await _outboxMessageRepository.EditAsync(outboxMessage);
        }
        if(unProcessedMessages.Any())
            await _unitOfWork.CommitAsync();
    }
}