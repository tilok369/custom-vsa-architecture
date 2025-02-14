using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Application.Consumers.Models;

public record UserCreated(int Id, User User);