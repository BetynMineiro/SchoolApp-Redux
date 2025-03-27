using SchoolApp.CrossCutting.Enum.Common;

namespace SchoolApp.Domain.Entities.Base;

public abstract class EntityBase
{

    public string CreatedBy { get;  set; } = string.Empty;
    public DateTimeOffset CreatedAt { get;  set; }
    public string UpdatedBy { get;  set; }= string.Empty;
    public DateTimeOffset? UpdatedAt { get;  set; }
    public Status Status { get; set; } = Status.Active;


    public void SetCreationInfo(string createdBy)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public void SetUpdateInfo(string updatedBy)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;
    }
}