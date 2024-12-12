using VsaArchitecture.Domain.Common;

namespace VsaArchitecture.Domain.Entities;

public class User: AuditableEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
